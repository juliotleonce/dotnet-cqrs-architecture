using MediatR;
using poketra_vyrt_api.Application.WalletSystem.Command;
using poketra_vyrt_api.Domain.Entity;
using poketra_vyrt_api.Domain.Port;

namespace poketra_vyrt_api.Application.WalletSystem.CommandHandler;

public class CreatePersonalWalletHandler(IUnitOfWork unitOfWork, IWalletRepository walletRepository) : IRequestHandler<CreatePersonalWalletCommand>
{
    public Task Handle(CreatePersonalWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = PersonalWallet.Create(request.WalletOwnerId);
        walletRepository.AddPersonalWallet(wallet);
        return unitOfWork.CommitAsync(cancellationToken);
    }
}