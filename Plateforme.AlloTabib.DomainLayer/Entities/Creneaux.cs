
using System;
using Plateforme.AlloTabib.DomainLayer.Base.Classes;

namespace Plateforme.AlloTabib.DomainLayer.Entities
{
    /// <summary>
    /// Le créneau définit la disponibilité des praticiens 
    /// </summary>
    public class Creneaux : BasicEntity
    {
        public virtual Guid Id { get; set; }
        public virtual string HeureDebut { get; set; }
        public virtual string HeureFin { get; set; }
        public virtual string Status { get; set; } //disponible ou pas
        public virtual Praticien Praticien { get; set; }
        public virtual string CurrentDate { get; set; }
        public virtual string Commentaire { get; set; }


        public virtual string DateCreation { get; set; }

        public Creneaux()
        {
            var DateCurrentCreation = new DateTime();
            DateCurrentCreation = DateTime.Now;
            DateCreation = DateCurrentCreation.ToString();
        }
    }
}
