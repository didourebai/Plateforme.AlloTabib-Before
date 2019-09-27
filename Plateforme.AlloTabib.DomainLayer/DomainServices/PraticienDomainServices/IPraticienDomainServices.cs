using System.Collections.Generic;
using Plateforme.AlloTabib.DomainLayer.Entities;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;

namespace Plateforme.AlloTabib.DomainLayer.DomainServices.PraticienDomainServices
{
    public interface IPraticienDomainServices
    {
        ResultOfType<PraticienResultModel> GetPraticiens(int take = 0, int skip = 0);
        ResultOfType<PraticienResultModel> GetPraticiensOpenSearch(string q, int take = 0, int skip = 0);
        IEnumerable<Praticien> GetAll();
        void AddNewPraticien(Praticien praticien);
        void ModifyPraticien(Praticien praticien);
        void DeletePraticien(object id);

        ResultOfType<PraticienResultDataModel> GetPraticienByEmail(string email);
        ResultOfType<PraticienResultDataModel> PostNewPraticien(PraticienDataModel praticienToAdd);
        ResultOfType<PraticienDataModel> DeleteAPraticien(string cin);
        ResultOfType<PraticienResultDataModel> PatchNewPraticien(PraticienDataModel praticienToAdd);
        ResultOfType<PraticienResultModel> SearchForPraticien(string gouvernerat, string specialite,
                    string nomPraticien, int take = 0, int skip = 0);

        ResultOfType<RendezVousResultModel> GetListOfRendezVousTousEnCours(string praticienEmail);
        ResultOfType<IList<SpecialiteGouverneratModel>> GetListSpecialiteGouvernerat();
        ResultOfType<PraticienResultDataModel> GetPraticienByNomPrenom(string nomPrenom);
    }
}
