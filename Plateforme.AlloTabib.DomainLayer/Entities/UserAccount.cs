
using System;
using Plateforme.AlloTabib.DomainLayer.Base.Classes;

namespace Plateforme.AlloTabib.DomainLayer.Entities
{
    public class UserAccount : BasicEntity
    {
        public virtual byte[] Password { get; set; }
        public virtual string Email { get; set; }
        //Type : doctor or patient
        public virtual string Type { get; set; }

        public virtual bool EstActive { get; set; }

        public UserAccount()
        {
            CreationDate = DateTime.UtcNow;
            LastModificationDate = DateTime.UtcNow;
        }
    }
}
