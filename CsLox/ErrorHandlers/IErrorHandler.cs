using GCS.Exceptions;
using GCS.Tokens;

namespace GCS.ErrorHandlers
{
    interface IErrorHandler
    {
        bool SyntaxError { get; }
        bool RuntimeError { get; }

        void Reset();
        void Error(int line, string message);
        void Error(Token token, string message);
        void Error(RuntimeErrorException error);

    }
}
