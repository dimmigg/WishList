using MediatR;
using WishList.Domain.Models;

namespace WishList.Domain.UseCases.Builder;

public interface IUseCaseBuilder
{
    IRequest Build(UseCaseParam param);
}