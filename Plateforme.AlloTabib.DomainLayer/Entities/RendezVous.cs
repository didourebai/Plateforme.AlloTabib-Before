using System;
using Plateforme.AlloTabib.DomainLayer.Base.Classes;

namespace Plateforme.AlloTabib.DomainLayer.Entities
{
    public class RendezVous : BasicEntity
    {
        public virtual Guid Id { get; set; }
       
      
        public virtual string NoteConsultation { get; set; }
        public virtual string Statut { get; set; }

        //du creneau on récupère la date de consultation,  heure de début de consultation ainsi que heure de fin de consultation
        public virtual  Creneaux Creneaux { get; set; }
     
        public virtual UserAccount Patient { get; set; }
        public virtual Praticien Praticien { get; set; }

        

        public RendezVous()
        {
            CreationDate = DateTime.UtcNow;
            LastModificationDate = DateTime.UtcNow;

            Patient = new UserAccount();
            Praticien = new Praticien();
        }
    }
}
