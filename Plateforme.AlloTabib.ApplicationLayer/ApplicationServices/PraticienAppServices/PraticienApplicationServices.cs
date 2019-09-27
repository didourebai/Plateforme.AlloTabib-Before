using System;
using System.Collections.Generic;
using Plateforme.AlloTabib.ApplicationLayer.DTO;
using Plateforme.AlloTabib.ApplicationLayer.Factories;
using Plateforme.AlloTabib.DomainLayer.DomainServices.PraticienDomainServices;
using Plateforme.AlloTabib.DomainLayer.Entities;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;

namespace Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.PraticienAppServices
{
    public class PraticienApplicationServices:IPraticienApplicationServices
    {
        #region Private Properties


        private readonly IPraticienDomainServices _praticienDomainServices;
        #endregion

        #region Constructor

            public PraticienApplicationServices(IPraticienDomainServices praticienDomainServices)
            {
                _praticienDomainServices = praticienDomainServices;
            }
        #endregion

        public IEnumerable<Praticien> GetAll()
        {
            return _praticienDomainServices.GetAll();
        }

        public void AddNewPraticien(Praticien praticien)
        {
            _praticienDomainServices.AddNewPraticien(praticien);
        }

        public void ModifyPraticien(Praticien praticien)
        {
            _praticienDomainServices.ModifyPraticien(praticien);
        }

        public void DeletePraticien(object id)
        {
           _praticienDomainServices.DeletePraticien(id);
        }

        public ResultOfType<PraticienResultModel> GetPraticiens(int take = 0, int skip = 0)
        {
            return _praticienDomainServices.GetPraticiens();
        }

        public ResultOfType<PraticienResultDataModel> PostNewPraticien(PraticienDTO praticienDto)
        {
            return _praticienDomainServices.PostNewPraticien(EntitiesFactory.ConvertToPraticienDataModel(praticienDto));
        }

        public ResultOfType<PraticienDataModel> DeleteAPraticien(string cin)
        {
            return _praticienDomainServices.DeleteAPraticien(cin);
        }


        public ResultOfType<PraticienResultDataModel> GetPraticienByEmail(string email)
        {
            return _praticienDomainServices.GetPraticienByEmail(email);
        }

        public ResultOfType<PraticienResultModel> GetPraticiensOpenSearch(string q, int take = 0, int skip = 0)
        {
            return _praticienDomainServices.GetPraticiensOpenSearch(q, take, skip);
        }


        public ResultOfType<PraticienResultDataModel> PatchPraticien(PraticienDTO praticienDto)
        {
            return _praticienDomainServices.PatchNewPraticien(EntitiesFactory.ConvertToPraticienDataModel(praticienDto));
        }


        public ResultOfType<PraticienResultModel> SearchForPraticien(string gouvernerat, string specialite, string nomPraticien, int take = 0, int skip = 0)
        {
            return _praticienDomainServices.SearchForPraticien(gouvernerat, specialite, nomPraticien, take, skip);
        }

        public ResultOfType<RendezVousResultModel> GetListOfRendezVousTousEnCours(string praticienEmail)
        {
            return _praticienDomainServices.GetListOfRendezVousTousEnCours(praticienEmail);
        }


        public ResultOfType<IList<SpecialiteGouverneratModel>> GetListSpecialiteGouvernerat()
        {
            return _praticienDomainServices.GetListSpecialiteGouvernerat();
        }


        public ResultOfType<PraticienResultDataModel> GetPraticienByNomPrenom(string nomPrenom)
        {
            return _praticienDomainServices.GetPraticienByNomPrenom(nomPrenom);
        }
    }
}
