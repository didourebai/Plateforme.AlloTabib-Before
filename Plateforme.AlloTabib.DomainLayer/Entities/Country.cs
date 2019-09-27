using System;
using Plateforme.AlloTabib.DomainLayer.Base.Classes;

namespace Plateforme.AlloTabib.DomainLayer.Entities
{
    public class Country : BasicEntity
    {
        public virtual int Id { get; set; }
        public virtual string CountryCd { get; set; }
        public virtual string CountryName { get; set; }


        public Country()
        {
            CreationDate = DateTime.UtcNow;
            LastModificationDate = DateTime.UtcNow;
        }
    }
}
