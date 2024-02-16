using WishList.Domain.Exceptions;
using WishList.Storage.CommandOptions;

namespace System;

public static class StringExtensions
{
    public static void ParseCommand(this string command,  out Command way, out CommandStep step)
    {
        var commands = command.Split('/');
        if (!Enum.TryParse<Command>(commands[0], out var parseWay) ||
            !Enum.TryParse<CommandStep>(commands[1], out var parseStep))
            throw new DomainException("Команда не распознана");
        way = parseWay;
        step = parseStep;
    }
}