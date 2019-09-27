using System;
using Plateforme.AlloTabib.ApplicationLayer.DTO;
using Plateforme.AlloTabib.DomainLayer.Entities;
using Plateforme.AlloTabib.DomainLayer.Models;

namespace Plateforme.AlloTabib.ApplicationLayer.Factories
{
    public static class EntitiesFactory
    {
        public static PatientDataModel ConvertToPatientDataModel(PatientDTO patientDto)
        {
            if (patientDto == null)
                return null;
            return new PatientDataModel
                {
                    Adresse = patientDto.Adresse,
                    Email = patientDto.Email,
                    Telephone = patientDto.Telephone,
                    Cin = patientDto.Cin,
                    NomPrenom = patientDto.NomPrenom,
                    Password = patientDto.Password,
                    DateNaissance = patientDto.DateNaissance,
                    Sexe = patientDto.Sexe
                };
        }

        public static PraticienDataModel ConvertToPraticienDataModel(PraticienDTO praticienDTO)
        {
            if(praticienDTO == null)
                return null;
            return new PraticienDataModel
            {
                Adresse = praticienDTO.Adresse,
                Password = praticienDTO.Password,
                Email = praticienDTO.Email,
                Telephone = praticienDTO.Telephone,
                Cin = praticienDTO.Cin,
                NomPrenom = praticienDTO.NomPrenom,
                Delegation = praticienDTO.Delegation,
                Fax = praticienDTO.Fax,
                Gouvernerat = praticienDTO.Gouvernerat,
                Conventionne = praticienDTO.Conventionne,
                Cursus = praticienDTO.Cursus,
                Diplomes = praticienDTO.Diplomes,
                EstActive = praticienDTO.EstActive,
                Formations = praticienDTO.Formations,
                InformationsPratique = praticienDTO.InformationsPratique,
                LanguesParles = praticienDTO.LanguesParles,
                MoyensPaiement = praticienDTO.MoyensPaiement,
                ParcoursHospitalier = praticienDTO.ParcoursHospitalier,
                PresentationCabinet = praticienDTO.PresentationCabinet,
                PrixConsultation = praticienDTO.PrixConsultation,
                Publication = praticienDTO.Publication,
                ReseauxSociaux = praticienDTO.ReseauxSociaux,
                Specialite = praticienDTO.Specialite
            };
        }


        public static ConfigurationDataModel ConvertToConfigurationDataModel(ConfigurationPraticienDto configurationDto)
        {
            if (configurationDto == null)
                return null;
            return new ConfigurationDataModel
            {
                DecalageMinute = configurationDto.DecalageMinute.ToString(),
                PraticienCin = configurationDto.PraticienCin,
                HeureDebutMidi = configurationDto.HeureDebutMidi,
                HeureDebutMatin = configurationDto.HeureDebutMatin,
                HeureFinMatin = configurationDto.HeureFinMatin,
                HeureFinMidi = configurationDto.HeureFinMidi,
                MinuteDebutMatin = configurationDto.MinuteDebutMatin,
                MinuteDebutMidi = configurationDto.MinuteDebutMidi,
                MinuteFinMatin = configurationDto.MinuteFinMatin,
                MinuteFinMidi = configurationDto.MinuteFinMidi
            };
        }

        public static RendezVousDataModel ConvertToRendezVousDataModel (RendezVousDTO rendezVousDto)
        {
            if (rendezVousDto == null)
                return null;
            
                return new RendezVousDataModel
                {
                    CreneauId = rendezVousDto.CreneauId,
                    CurrentDate = rendezVousDto.CurrentDate,
                    HeureDebut = rendezVousDto.HeureDebut,
                    HeureFin = rendezVousDto.HeureFin,
                    NoteConsultation = rendezVousDto.NoteConsultation,
                    PatientEmail = rendezVousDto.PatientCin,
                    PraticienCin = rendezVousDto.PraticienCin,
                    Statut = rendezVousDto.Statut,
                    Id = rendezVousDto.Id
                };
        }

        public static JourFerieDataModel ConvertToJourFerieResultDataModel (JourFerieDTO jourFerieDto)
        {
            if (jourFerieDto == null)
                return null;
            return new JourFerieDataModel
            {
                JourFerieNom = jourFerieDto.JourFerieNom,
                PraticienEmail = jourFerieDto.PraticienEmail
            };
        }

        public static CreneauDataModel ConvertToJourFerieDataModel(CreneauDTO creneauDto)
        {
            if (creneauDto == null)
                return null;
            return new CreneauDataModel
            {
                CurrentDate = creneauDto.CurrentDate,
                HeureDebut = creneauDto.HeureDebut,
                HeureFin = creneauDto.HeureFin,
                PraticienEmail = creneauDto.Praticien,
                Status = creneauDto.Status,
                Commentaire = creneauDto.Commentaire
            };
        }
    }
}
