
using System;
using Plateforme.AlloTabib.DomainLayer.Base.Classes;

namespace Plateforme.AlloTabib.DomainLayer.Entities
{
    public class GoogleMapsCoordinations : BasicEntity
    {
        public virtual Guid Id { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        public virtual double Latitude { get; set; }
        public virtual double Longitude { get; set; }
        public virtual string FormattedAddress { get; set; }

        public GoogleMapsCoordinations()
        {
            CreationDate = DateTime.UtcNow;
            LastModificationDate = DateTime.UtcNow;
        }
    }
}
