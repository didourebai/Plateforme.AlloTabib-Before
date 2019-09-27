using PlateformeAlloTabib.Standards.Domain;

namespace PlateformeAlloTabib.Standards.Helpers
{
    public interface IFinalize<T>
    {
        ResultOfType<T> WithDefaultResult();

        ResultOfType<T> WithResult(T result);
    }
}
