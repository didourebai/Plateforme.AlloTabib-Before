using System;
using PlateformeAlloTabib.Standards.Domain;

namespace PlateformeAlloTabib.Standards.Helpers
{
    public interface IAddGenericErrors<T>
    {
        IAddErrorsOrFinalize<T> AddingError(IAmError error);

        IAddErrorsOrFinalize<T> AddingGenericError(string Code, string Message);

        IAddErrorsOrFinalize<T> AddingException(Exception exception);

        IAddErrorsOrFinalize<T> AddingDataAccessError();
    }
}
