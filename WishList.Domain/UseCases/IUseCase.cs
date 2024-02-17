namespace WishList.Domain.UseCases;

public interface IUseCase
{
    Task Execute(CancellationToken cancellationToken);
}