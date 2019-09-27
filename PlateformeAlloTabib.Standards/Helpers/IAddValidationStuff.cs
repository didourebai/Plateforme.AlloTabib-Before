using PlateformeAlloTabib.Standards.Domain;

namespace PlateformeAlloTabib.Standards.Helpers
{
    public interface IAddValidationStuff<T>
    {
        IAddErrorsOrFinalize<T> AddingValidationResponse(ValidationResponse validation);

        IAddErrorsOrFinalize<T> AddingValidationError(ValidationError validation);

        IAddErrorsOrFinalize<T> AddingValidationError(string attemptedValue, string message, string propertyName);
    }
}
