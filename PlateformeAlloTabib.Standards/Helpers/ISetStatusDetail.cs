
using PlateformeAlloTabib.Standards.Domain;

namespace PlateformeAlloTabib.Standards.Helpers
{
    public interface ISetStatusDetail<T>
    {
        IAddErrorsOrFinalize<T> As(EStatusDetail statusDetail);

        IAddErrorsOrFinalize<T> AsNotFound();

        IAddValidationStuff<T> AsValidationFailure();

        IFinalize<T> AsValidationFailure(ValidationResponse validation);

        IFinalize<T> AsValidationFailure(ValidationError validation);

        IFinalize<T> AsValidationFailure(string attemptedValue, string message, string propertyName);

        IAddAllKindOfErrors<T> AsGenericError();
    }
}
