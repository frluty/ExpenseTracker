
using Microsoft.IdentityModel.Tokens;

namespace TT.Application.Common.Exceptions;

public class ValidationException:Exception
{
    public ValidationException(IDictionary<string, string[]> errors)
        : base("One or more validation failures have occurred.")
    {
    }

    public ValidationException( string message)
        : base(message)
    {
    }
}