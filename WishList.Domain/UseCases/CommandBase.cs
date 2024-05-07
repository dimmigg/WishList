using MediatR;
using WishList.Domain.Models;

namespace WishList.Domain.UseCases;

public abstract class CommandBase : IRequest
{
    public UseCaseParam Param { get; set; }

    internal CommandBase(UseCaseParam param)
    {
        Param = param;
    }
}