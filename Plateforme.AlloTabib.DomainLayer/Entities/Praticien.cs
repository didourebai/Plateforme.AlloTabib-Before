using System;
using System.Collections.Generic;
using Plateforme.AlloTabib.DomainLayer.Base.Classes;

namespace Plateforme.AlloTabib.DomainLayer.Entities
{
    public class Praticien : BasicEntity
    {
        public virtual string Cin { get; set; }
        public virtual byte[] Password { get; set; }
        public virtual string Email { get; set; }
        public virtual string NomPrenom { get; set; }
        public virtual string Adresse { get; set; }
        public virtual string Telephone { get; set; }
        public virtual string Fax { get; set; }
        public virtual string Gouvernerat { get; set; }
        public virtual string Delegation { get; set; }
        public virtual string Specialite { get; set; }

        //attribution de nombre d'étoiles
        //public virtual int Reputation { get; set; }
        public virtual string LanguesParles { get; set; }
        public virtual string Diplomes { get; set; }
        public virtual string Formations { get; set; }
        public virtual string Cursus { get; set; }
        public virtual string Publication { get; set; }
        public virtual string MoyensPaiement { get; set; }
        public virtual string ParcoursHospitalier { get; set; }
        public virtual bool Conventionne { get; set; }
        public virtual string InformationsPratique { get; set; }
        public virtual string PrixConsultation { get; set; }
        public virtual string PresentationCabinet { get; set; }

        public virtual string AdresseGoogle { get; set; }
        

        public virtual string ReseauxSociaux { get; set; }


        public virtual IList<Consultation> Consultations { get; set; }
        public virtual bool IsIndexed { get; set; }

        public Praticien()
        {
            CreationDate = DateTime.UtcNow;
            LastModificationDate = DateTime.UtcNow;
            Consultations = new List<Consultation>();
            IsIndexed = false;
        }
    }
}
