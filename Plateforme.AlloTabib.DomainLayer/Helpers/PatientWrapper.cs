
using Plateforme.AlloTabib.DomainLayer.Entities;
using Plateforme.AlloTabib.DomainLayer.Models;

namespace Plateforme.AlloTabib.DomainLayer.Helpers
{
    public static class PatientWrapper
    {
        public static PatientResultDataModel ConvertPatientEntityToDataModel(Patient patient)
        {
            if (patient == null)
                return null;

            var patientResultDataModel = new PatientResultDataModel();
            if (!string.IsNullOrEmpty(patient.Adresse))
                patientResultDataModel.Adresse = patient.Adresse;
            if (!string.IsNullOrEmpty(patient.Cin))
                patientResultDataModel.Cin = patient.Cin;
            if (!string.IsNullOrEmpty(patient.NomPrenom))
                patientResultDataModel.NomPrenom = patient.NomPrenom;
            if (!string.IsNullOrEmpty(patient.DateNaissance))
                patientResultDataModel.DateNaissance = patient.DateNaissance;
            if (patient.Password != null)
                patientResultDataModel.Password =
                    CrossCuttingLayer.Encryption.RijndaelEncryption.Decrypt(patient.Password);
            if (!string.IsNullOrEmpty(patient.Telephone))
                patientResultDataModel.Telephone = patient.Telephone;
            if (!string.IsNullOrEmpty(patient.Sexe))
                patientResultDataModel.Sexe = patient.Sexe;
            patientResultDataModel.Email = patient.Email; 
            return patientResultDataModel;
        }

        public static PatientResultDataModel ConvertPratientEntityToDataModel(Praticien patient)
        {
            if (patient == null)
                return null;

            var patientResultDataModel = new PatientResultDataModel();
            if (!string.IsNullOrEmpty(patient.Adresse))
                patientResultDataModel.Adresse = patient.Adresse;
            if (!string.IsNullOrEmpty(patient.Cin))
                patientResultDataModel.Cin = patient.Cin;
            if (!string.IsNullOrEmpty(patient.NomPrenom))
                patientResultDataModel.NomPrenom = patient.NomPrenom;
            //if (!string.IsNullOrEmpty(patient.DateNaissance))
            //    patientResultDataModel.DateNaissance = patient.DateNaissance;
            if (patient.Password != null)
                patientResultDataModel.Password =
                    CrossCuttingLayer.Encryption.RijndaelEncryption.Decrypt(patient.Password);
            if (!string.IsNullOrEmpty(patient.Telephone))
                patientResultDataModel.Telephone = patient.Telephone;
            //if (!string.IsNullOrEmpty(patient.Sexe))
            //    patientResultDataModel.Sexe = patient.Sexe;
            patientResultDataModel.Email = patient.Email;
            return patientResultDataModel;
        }
    }
}
