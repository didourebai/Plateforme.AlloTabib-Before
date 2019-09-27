namespace Plateforme.AlloTabib.DomainLayer.Models
{
    public class FacebookUserModel
    {
        public string FirstName { get; set; } //nom
        public string LastName { get; set; } //prenom
        public string Name { get; set; } //Nom complet
        public string Link { get; set; } //Lien facebook
        public string Locality { get; set; } //pays
        public string Gender { get; set; } //Sexe
    }
}
