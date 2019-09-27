using System;
using System.Collections.Generic;
using Plateforme.AlloTabib.ApplicationLayer.DTO;
using Plateforme.AlloTabib.ApplicationLayer.Factories;
using Plateforme.AlloTabib.DomainLayer.DomainServices.PatientDomainServices;
using Plateforme.AlloTabib.DomainLayer.Entities;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;

namespace Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.PatientAppServices
{
    public class PatientApplicationServices : IPatientApplicationServices
    {
        #region Private Properties

        private readonly IPatientDomainServices _patientDomainServices;

        #endregion

        #region Constructors

        public PatientApplicationServices(IPatientDomainServices patientDomainServices)
        {
            if (patientDomainServices == null)
                throw new ArgumentNullException("patientDomainServices");

            _patientDomainServices = patientDomainServices;
        }

        #endregion

        public IEnumerable<Patient> GetAll()
        {
            return _patientDomainServices.GetAll();
        }

        public void AddNewPatient(Patient patient)
        {
           _patientDomainServices.AddNewPatient(patient);
        }

        public void ModifyPatient(Patient patient)
        {
            _patientDomainServices.ModifyPatient(patient);
        }

        public void DeletePatient(object id)
        {
            _patientDomainServices.DeletePatient(id);
        }

        public ResultOfType<PatientResultModel> GetPatients(int take = 0, int skip = 0)
        {
            return _patientDomainServices.GetPatients();
        }

        public ResultOfType<PatientResultDataModel> PostNewPatient(PatientDTO patientDto)
        {
            return _patientDomainServices.PostNewPatient(EntitiesFactory.ConvertToPatientDataModel(patientDto));
        }

        public ResultOfType<PatientResultDataModel> DeletePatient(string cin)
        {
            return _patientDomainServices.DeleteAPatient(cin);


        }

        public ResultOfType<PatientResultDataModel> GetPatientByEmail(string email)
        {
            return _patientDomainServices.GetPatientByEmail(email);
        }


        public ResultOfType<PatientResultDataModel> PatchPatient(PatientDTO patient)
        {
            return _patientDomainServices.PatchPatient(EntitiesFactory.ConvertToPatientDataModel(patient));
        }
    }
}
