
namespace Plateforme.AlloTabib.ApplicationLayer.DTO
{
    public class PraticienDTO
    {
        public  string Cin { get; set; }
        public  string Password { get; set; }
        public  string Email { get; set; }
        public  string NomPrenom { get; set; }
        public  string Adresse { get; set; }
        public  string Telephone { get; set; }
        public  string Fax { get; set; }
        public  string Gouvernerat { get; set; }
        public  string Delegation { get; set; }
        public  string Specialite { get; set; }

        //Autres informations

        public  string LanguesParles { get; set; }//
        public  string Diplomes { get; set; }//
        public  string Formations { get; set; }//
        public  string Cursus { get; set; }//
        public  string Publication { get; set; }//
        public  string MoyensPaiement { get; set; }//
        public  string ParcoursHospitalier { get; set; }//
        public  string Conventionne { get; set; }//
        public  string InformationsPratique { get; set; }
        public  string PrixConsultation { get; set; }//
        public  string PresentationCabinet { get; set; }
        public  string EstActive { get; set; }
        public  string ReseauxSociaux { get; set; }//
    }
}
