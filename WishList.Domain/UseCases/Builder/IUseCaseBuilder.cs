using WishList.Domain.Models;

namespace WishList.Domain.UseCases.Builder;

public interface IUseCaseBuilder
{
    IUseCase Build(BuildParam param);
}