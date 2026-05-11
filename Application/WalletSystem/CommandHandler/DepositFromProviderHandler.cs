using MediatR;
using poketra_vyrt_api.Application.WalletSystem.Command;
using poketra_vyrt_api.Domain.Entity;
using poketra_vyrt_api.Domain.Exception;
using poketra_vyrt_api.Domain.Port;

namespace poketra_vyrt_api.Application.WalletSystem.CommandHandler;

public class DepositFromProviderHandler
(
    IWalletRepository walletRepository,
    IWalletTransactionRepository walletTransactionRepository,
    IUnitOfWork unitOfWork
): 
    IRequestHandler<DepositFromProviderCommand, object>
{
    public async Task<object> Handle(DepositFromProviderCommand cmd, CancellationToken ct)
    {
        var personalWallet = await walletRepository.GetPersonalWalletByOwnerId(cmd.WalletUserId);
        var providerWallet = await walletRepository.GetProviderWalletByProviderReference(cmd.ProviderReference);
        if(personalWallet == null) throw new EntityNotFoundException("Utilisateur n'a pas de portefeuille");
        if(providerWallet == null) throw new EntityNotFoundException("Le portefeuille du fournisseur n'existe pas");
        
        var transaction = WalletTransaction.CreateAndInit
        (
                personalWallet, 
                providerWallet, 
                WalletTransactionType.Deposit,
                cmd.Amount
        );
        
        walletTransactionRepository.Add(transaction);
        await unitOfWork.CommitAndDispatchEventsAsync(ct);
        
        return new { TransactionId = transaction.Id };
    }
}