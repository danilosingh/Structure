using System;

namespace Structure.ExceptionHandling
{
    public interface IErrorInfoBuilder
    {
        ErrorInfo BuildInfo(Exception exception);
    }
}
