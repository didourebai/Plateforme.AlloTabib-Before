using System;
using Plateforme.AlloTabib.DomainLayer.Models.Base;

namespace Plateforme.AlloTabib.DomainLayer.Models
{
    public class InternalError : ErrorBase
    {
        #region Properties

        public Exception Exception { get; set; }

        #endregion Properties
    }
}