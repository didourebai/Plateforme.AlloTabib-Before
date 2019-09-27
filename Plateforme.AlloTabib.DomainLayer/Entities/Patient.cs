
using System;
using Plateforme.AlloTabib.DomainLayer.Base.Classes;

namespace Plateforme.AlloTabib.DomainLayer.Entities
{
    public class Patient : BasicEntity
    {
        public virtual string Cin { get; set; }
        public virtual byte[] Password { get; set; }
        public virtual string Email { get; set; }
        public virtual string NomPrenom { get; set; }
        public virtual string Adresse { get; set; }
        public virtual string Telephone { get; set; }
        public virtual string DateNaissance { get; set; }
        public virtual string Sexe { get; set; }

        public virtual bool IsIndexed { get; set; }


        public Patient()
        {
            CreationDate = DateTime.UtcNow;
            LastModificationDate = DateTime.UtcNow;
            IsIndexed = false;
        }
    }
}
