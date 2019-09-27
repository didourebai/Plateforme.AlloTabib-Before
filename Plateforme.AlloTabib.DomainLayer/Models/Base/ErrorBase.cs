using PlateformeAlloTabib.Standards.Domain;

namespace Plateforme.AlloTabib.DomainLayer.Models.Base
{
    public class ErrorBase : IAmError
    {
        #region Properties

        public EErrorType Type { get; set; }

        public string Code { get; set; }

        public string Message { get; set; }

        #endregion
    }
}