using System.Linq;
using Plateforme.AlloTabib.DomainLayer.Entities;
using Plateforme.AlloTabib.DomainLayer.Models;

namespace Plateforme.AlloTabib.DomainLayer.Helpers
{
    public static class PraticienWrapper
    {
        public static PraticienResultDataModel ConvertPraticienEntityToDataModel(Praticien praticien)
        {
            if (praticien == null)
                return null;
            else
            {
                string latLong = praticien.AdresseGoogle;
                
                string lat = "36.813988";
                string longi = "10.154027";
                if (!string.IsNullOrEmpty(latLong))
                {
                    var splits = latLong.Split(',');
                    if (splits.Count() == 2)
                     lat = splits[0];
                     longi = splits[1];
                }

                return new PraticienResultDataModel
                {
                    Adresse = praticien.Adresse,
                    Cin = praticien.Cin,
                    Conventionne = praticien.Conventionne.ToString(),
                    Cursus = praticien.Cursus,
                    Delegation = praticien.Delegation,
                    Diplomes = praticien.Diplomes,
                    Email = praticien.Email,
                    Fax = praticien.Fax,
                    Formations = praticien.Formations,
                    Gouvernerat = praticien.Gouvernerat,
                    InformationsPratique = praticien.InformationsPratique,
                    LanguesParles = praticien.LanguesParles,
                    MoyensPaiement = praticien.MoyensPaiement,
                    NomPrenom = praticien.NomPrenom,
                    ParcoursHospitalier = praticien.ParcoursHospitalier,
                    Password = CrossCuttingLayer.Encryption.RijndaelEncryption.Decrypt(praticien.Password),
                    PresentationCabinet = praticien.PresentationCabinet,
                    PrixConsultation = praticien.PrixConsultation,
                    Publication = praticien.Publication,
                    ReseauxSociaux = praticien.ReseauxSociaux,
                    Specialite = praticien.Specialite,
                    Telephone = praticien.Telephone,
                    AdresseGoogleLag = lat,
                    AdresseGoogleLong = longi
                };
            }

            
        }

        public static PraticienToIndexModel PraticienEntityToPraticienToIndexModel(Praticien praticien)
        {
            if (praticien == null)
                return null;
            string latLong = praticien.AdresseGoogle;

            string lat = "36.813988";
            string longi = "10.154027";
            if (!string.IsNullOrEmpty(latLong))
            {
                var splits = latLong.Split(',');
                if (splits.Count() == 2)
                    lat = splits[0];
                longi = splits[1];
            }


            return new PraticienToIndexModel
            {
                Adresse = praticien.Adresse,
                Cin = praticien.Cin,
                Conventionne = praticien.Conventionne.ToString(),
                Cursus = praticien.Cursus,
                Delegation = praticien.Delegation,
                Diplomes = praticien.Diplomes,
                Email = praticien.Email,
                Fax = praticien.Fax,
                Formations = praticien.Formations,
                Gouvernerat = praticien.Gouvernerat,
                InformationsPratique = praticien.InformationsPratique,
                LanguesParles = praticien.LanguesParles,
                MoyensPaiement = praticien.MoyensPaiement,
                NomPrenom = praticien.NomPrenom,
                ParcoursHospitalier = praticien.ParcoursHospitalier,
                Password = CrossCuttingLayer.Encryption.RijndaelEncryption.Decrypt(praticien.Password),
                PresentationCabinet = praticien.PresentationCabinet,
                PrixConsultation = praticien.PrixConsultation,
                Publication = praticien.Publication,
                ReseauxSociaux = praticien.ReseauxSociaux,
                Specialite = praticien.Specialite,
                Telephone = praticien.Telephone,
                AdresseGoogleLag = lat,
                AdresseGoogleLong = longi
            };
        }
    }
}
