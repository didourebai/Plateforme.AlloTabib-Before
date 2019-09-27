using System;
using Plateforme.AlloTabib.DomainLayer.Base.Classes;

namespace Plateforme.AlloTabib.DomainLayer.Entities
{
    public class Consultation : BasicEntity
    {
        public virtual int Id { get; set; }
        public virtual string Label { get; set; }

        public virtual string Specialite { get; set; }

        public Consultation()
        {
            CreationDate = DateTime.UtcNow;
            LastModificationDate = DateTime.UtcNow;
        }
    }
}
