using System.Collections.Generic;
using Plateforme.AlloTabib.DomainLayer.Models.Base;

namespace Plateforme.AlloTabib.DomainLayer.Models
{
    public class BaseResult
    {
        #region Properties

        public List<ErrorBase> Errors { get; set; }

        public string Status { get; set; }

        public string StatusDetail { get; set; }

        #endregion Properties

        #region Constructors

        public BaseResult()
        {
            Errors = new List<ErrorBase>();
        }

        #endregion Constructors
    }
}