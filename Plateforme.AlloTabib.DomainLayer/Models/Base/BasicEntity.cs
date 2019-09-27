using System;

namespace Plateforme.AlloTabib.DomainLayer.Models.Base
{
    public class BasicEntity
    {
        [Criteria(PropertyName = "LastModificationDate", IsDefaultOrderProperty = true)]
        public virtual DateTime LastModificationDate { get; set; }

        [Criteria(PropertyName = "CreationDate")]
        public virtual DateTime CreationDate { get; set; }
    }
}
