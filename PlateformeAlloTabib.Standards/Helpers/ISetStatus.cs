
namespace PlateformeAlloTabib.Standards.Helpers
{
    public interface ISetStatus<T>
    {
        ISetStatusDetail<T> Error();

        IFinalize<T> OK();
    }
}
