
namespace PlateformeAlloTabib.Standards.Domain
{
    public interface IAmError
    {
        EErrorType Type { get; }

        string Code { get; }

        string Message { get; }
    }
}
