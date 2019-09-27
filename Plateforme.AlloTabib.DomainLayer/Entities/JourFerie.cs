using System;
using Plateforme.AlloTabib.DomainLayer.Base.Classes;

namespace Plateforme.AlloTabib.DomainLayer.Entities
{
    public class JourFerie : BasicEntity
    {
        public virtual Guid Id { get; set; }
        public virtual string JourFerieNom { get; set; }
        public virtual Praticien Praticien { get; set; }
    }
}
