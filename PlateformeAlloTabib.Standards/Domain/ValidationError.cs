
namespace PlateformeAlloTabib.Standards.Domain
{
    public class ValidationError : IAmError
    {
        public object AttemptedValue { get; set; }

        public object CustomState { get; set; }

        public string PropertyName { get; set; }

        public EErrorType Type
        {
            get
            {
                return EErrorType.VALIDATION_FAILURE;
            }
        }

        public string Code { get; set; }

        public string Message { get; set; }
    }
}
