
namespace PlateformeAlloTabib.Standards.Helpers
{
    public interface IAddErrorsOrFinalize<T> : IAddAllKindOfErrors<T>, IAddValidationStuff<T>, IAddGenericErrors<T>, IFinalize<T>
    {
    }
}
