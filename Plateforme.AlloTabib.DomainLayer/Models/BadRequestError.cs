using Plateforme.AlloTabib.DomainLayer.Models.Base;

namespace Plateforme.AlloTabib.DomainLayer.Models
{
    public class BadRequestError : ErrorBase
    {
        #region Properties

        public string AttemptedValue { get; set; }

        public string CustomState { get; set; }

        public string PropertyName { get; set; }

        #endregion Properties
    }
}