using Plateforme.AlloTabib.ApplicationLayer.DTO;
using Plateforme.AlloTabib.ApplicationLayer.Factories;
using Plateforme.AlloTabib.DomainLayer.DomainServices.RendezVousDomainServices;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;
using System;


namespace Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.RendezVousAppServices
{
    public class RendezVousAppServices : IRendezVousAppServices
    {
        #region Private Properties

        private readonly IRendezVousDomainServices _rendezVousDomainServices;

        #endregion


        public RendezVousAppServices(IRendezVousDomainServices rendezVousDomainServices)
        {
            if (rendezVousDomainServices == null)
                throw new ArgumentNullException("rendezVousDomainServices");
            _rendezVousDomainServices = rendezVousDomainServices;
        }


        public ResultOfType<RendezVousResultDataModel> PostNewRendezVous(RendezVousDTO rendezVousDto)
        {
            return _rendezVousDomainServices.PostNewRendezVous(EntitiesFactory.ConvertToRendezVousDataModel(rendezVousDto));
        }

        public ResultOfType<RendezVousResultModel> GetRendezVousByDateAndPraticien(string praticien, string dateCurrent)
        {
            return _rendezVousDomainServices.GetRendezVousByDateAndPraticien(praticien,dateCurrent);
        }

        public ResultOfType<RendezVousResultDataModel> PatchNewRendezVous(RendezVousDTO rendezVousDto)
        {
            return _rendezVousDomainServices.PatchNewRendezVous(EntitiesFactory.ConvertToRendezVousDataModel(rendezVousDto));
        }


        public ResultOfType<Null> DeleteRendezVous(string rendezVousId)
        {
            return _rendezVousDomainServices.DeleteRendezVous(rendezVousId);
        }


        public ResultOfType<PatientResultModel> GetPatientsParPraticien(string praticien)
        {
            return _rendezVousDomainServices.GetPatientsParPraticien(praticien);
        }


        public ResultOfType<RendezVousResultDataModel> CreneauAyantRendezVous(string praticien, string dateCurrent, string heureDebut)
        {
            return _rendezVousDomainServices.CreneauAyantRendezVous(praticien, dateCurrent, heureDebut);
        }

        public ResultOfType<RdvResultModel> GetAllRendezVousParPatientEnCours(string patientEmail)
        {
            return _rendezVousDomainServices.GetAllRendezVousParPatientEnCours(patientEmail);
        }


        public ResultOfType<RendezVousResultModel> GetAllRendezVousNonConfirmeOuRejete(string praticienEmail)
        {
            return _rendezVousDomainServices.GetAllRendezVousNonConfirmeOuRejete(praticienEmail);
        }
    }
}
