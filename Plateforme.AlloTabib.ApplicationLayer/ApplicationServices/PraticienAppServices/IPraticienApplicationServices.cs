using System.Collections.Generic;
using Plateforme.AlloTabib.ApplicationLayer.DTO;
using Plateforme.AlloTabib.DomainLayer.Entities;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;

namespace Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.PraticienAppServices
{
    public interface IPraticienApplicationServices
    {
        IEnumerable<Praticien> GetAll();
        void AddNewPraticien(Praticien praticien);
        void ModifyPraticien(Praticien praticien);
        void DeletePraticien(object id);
        ResultOfType<PraticienResultModel> GetPraticiens(int take = 0, int skip = 0);

        ResultOfType<PraticienResultDataModel> PostNewPraticien(PraticienDTO praticienDto);
        ResultOfType<PraticienDataModel> DeleteAPraticien(string cin);
        ResultOfType<PraticienResultDataModel> GetPraticienByEmail(string email);

        ResultOfType<PraticienResultModel> GetPraticiensOpenSearch(string q, int take = 0, int skip = 0);
        ResultOfType<PraticienResultDataModel> PatchPraticien(PraticienDTO praticienDto);
        ResultOfType<PraticienResultModel> SearchForPraticien(string gouvernerat, string specialite,
                    string nomPraticien, int take = 0, int skip = 0);

        ResultOfType<RendezVousResultModel> GetListOfRendezVousTousEnCours(string praticienEmail);
        ResultOfType<IList<SpecialiteGouverneratModel>> GetListSpecialiteGouvernerat();
        ResultOfType<PraticienResultDataModel> GetPraticienByNomPrenom(string nomPrenom);
    }
}
