using System;

namespace Plateforme.AlloTabib.DomainLayer.Facebook
{
    public class FacebookEntity
    {
       // public FacebookClient FacebookClient { get; set; }
        public virtual string FirstName { get; set; } //nom
        public virtual string LastName { get; set; } //prenom
        public virtual string Name { get; set; } //Nom complet
        //public virtual dynamic ClientObject { get; set; } //objet complet
        public virtual string Link { get; set; } //Lien facebook
        public virtual string Locality { get; set; } //pays
        public virtual string Gender { get; set; } //Sexe
        public virtual string Username { get; set; } //Sexe
        public virtual string DateCourante { get; set; }

        public FacebookEntity()
        {
            DateCourante = DateTime.UtcNow.ToLongDateString();
        }
       
    }
}
