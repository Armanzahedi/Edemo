using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Ardalis.GuardClauses;

namespace Edemo.Domain.Common.Exceptions;

public static class GuardClausesExtensions
{
    public static T NotFound<T>(this IGuardClause guardClause,
        [NotNull] [ValidatedNotNull] T? input,
        string? message = null)
    {
        if (input is null)
        {
            throw new NotFoundException(message ?? "");
        }

        return input;
    }

    public static T Expression<T>(this IGuardClause guardClause,
        Func<T, bool> func,
        T input,
        string message) where T : struct
    {
        if (func(input))
        {
            throw new ArgumentException(message);
        }

        return input;
    }
}