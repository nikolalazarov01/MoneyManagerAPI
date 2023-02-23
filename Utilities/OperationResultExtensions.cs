using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Utilities;

public static class OperationResultExtensions
{
    public static TOperationResult AppendErrors<TOperationResult>(this TOperationResult principal, OperationResult other)
        where TOperationResult : OperationResult
    {
        if (principal is null) throw new ArgumentNullException(nameof(principal));

        foreach (var error in (other?.Errors).OrEmptyIfNull().IgnoreNullValues()) principal.AddError(error);
        return principal;
    }

    public static OperationResult<TData> WithData<TData>(this OperationResult<TData> principal, TData data)
    {
        if (principal is null) throw new ArgumentNullException(nameof(principal));
        
        principal.Data = data;
        return principal;
    }

    public static void AppendException(this OperationResult principal, Exception ex)
    {
        if (principal is null) throw new ArgumentNullException(nameof(principal));
        if (ex is null) return;

        var error = new Error { IsNotExpected = true, Message = ex.Message };
        principal.AddError(error);
    }
    
#nullable enable
    public static bool ValidateNotNull<TValue>(this OperationResult operationResult, [NotNullWhen(true)] TValue? value, [CallerFilePath] string? filePath = null, [CallerMemberName] string? memberName = null, [CallerLineNumber] int line = -1, [CallerArgumentExpression("value")] string? expression = null)
    {
        if (operationResult is null) throw new ArgumentNullException(nameof(operationResult));
        if (value is not null) return true;

        var error = new Error { Message = $"{FormatErrorMessage(filePath, memberName, line, expression)} should not be null." };
        operationResult.AddError(error);
        return false;
    }
#nullable restore
    
    private static string FormatErrorMessage(string filePath, string memberName, int line, string argumentExpression) => $"{filePath} ({memberName};{line}) - Expression [{argumentExpression}]";
}