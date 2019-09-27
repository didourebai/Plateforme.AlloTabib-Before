using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using NHibernate.Mapping;
using Plateforme.AlloTabib.CrossCuttingLayer.Logging;
using Plateforme.AlloTabib.DomainLayer.Base.Interfaces;
using Plateforme.AlloTabib.DomainLayer.Entities;
using Plateforme.AlloTabib.DomainLayer.Helpers;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;
using PlateformeAlloTabib.Standards.Helpers;

namespace Plateforme.AlloTabib.DomainLayer.DomainServices.RendezVousDomainServices
{
    public class RendezVousDomainServices : IRendezVousDomainServices
    {
        #region Private Properties

        private readonly IRepository<Creneaux> _creneauRepository;
        private readonly IRepository<UserAccount> _userAccountRepository;
        private readonly IRepository<RendezVous> _rendezVousRepository;
        private readonly IRepository<Praticien> _praticienRepository;
        private readonly IRepository<Patient> _patientRepository;
        private readonly IRepository<HistoriqueRendezVous> _historiqueRdvRepository;
       
        #endregion

        #region constructor

        public RendezVousDomainServices(IRepository<Creneaux> creneauRepository, IRepository<RendezVous> rendezVousRepository, IRepository<Praticien> praticienRepository, IRepository<Patient> patientRepository, IRepository<HistoriqueRendezVous> historiqueRdvRepository, IRepository<UserAccount> userAccountRepository)
        {
            if (creneauRepository == null)
                throw new ArgumentNullException("creneauRepository");
            _creneauRepository = creneauRepository;

            if (rendezVousRepository == null)
                throw new ArgumentNullException("rendezVousRepository");
            _rendezVousRepository = rendezVousRepository;

            if (praticienRepository == null)
                throw new ArgumentNullException("praticienRepository");
            _praticienRepository = praticienRepository;

            if (patientRepository == null)
                throw new ArgumentNullException("patientRepository");
            _patientRepository = patientRepository;
            if(historiqueRdvRepository == null)
                throw new ArgumentNullException("historiqueRdvRepository");
            _historiqueRdvRepository = historiqueRdvRepository;
            if (userAccountRepository == null)
                throw new ArgumentNullException("userAccountRepository");
            _userAccountRepository = userAccountRepository;
        }

        #endregion

        public ResultOfType<RendezVousResultDataModel> PostNewRendezVous(RendezVousDataModel rendezVousToAdd)
        {
            try
            {

                Logger.LogInfo("PostNewRendezVous : Start.");
                if (rendezVousToAdd == null)
                    return new Return<RendezVousResultDataModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                        null, "Les données sont vides.").WithDefaultResult();

                Logger.LogInfo(string.Format("Post New rendez vous : Start --- PatientEmail = {0}, PatientEmail = {1}",
                                               rendezVousToAdd.PatientEmail, rendezVousToAdd.PatientEmail));

                var validationResult = ValidateNewRendezVousProperties(rendezVousToAdd);

                if (validationResult != null)
                {
                    Logger.LogInfo(string.Format("Post New rendez vous : End --- Status = {0}, Message= {1}",
                                                 validationResult.Status, validationResult.Errors[0].Message));
                    return validationResult;
                }

                var rdv = _rendezVousRepository.GetAll().FirstOrDefault(r => r.Creneaux.CurrentDate.Equals(rendezVousToAdd.CurrentDate) && r.Creneaux.HeureDebut.Equals(rendezVousToAdd.HeureDebut) && r.Praticien.Cin.Equals(rendezVousToAdd.PraticienCin));

                if(rdv != null)
                    return new Return<RendezVousResultDataModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                      null, "Un rendez vous à cette date et à cette heure pour  ce praticien a été pris déjà.").WithDefaultResult();
                //vérifier si le patient a pris déjà un rendez dans ce jour pour le même médecin ! bloquer ça pour ne pas abuser et garder tjr horaire pour le reste
                var rdvthatday = _rendezVousRepository.GetAll().FirstOrDefault(r => r.Creneaux.CurrentDate.Equals(rendezVousToAdd.CurrentDate) && r.Praticien.Cin.Equals(rendezVousToAdd.PraticienCin) && r.Patient.Email.Equals(rendezVousToAdd.PatientEmail));

                if (rdvthatday != null)
                    return new Return<RendezVousResultDataModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                      null, "Vous avez pris déjà un rendez à cette date pour  ce praticien, vous ne pouvez pas prendre plus qu'un rendez vous à une date donnée.").WithDefaultResult();

                if (rendezVousToAdd.NoteConsultation == null)
                    rendezVousToAdd.NoteConsultation = string.Empty;
               

                var patient = _userAccountRepository.GetAll().FirstOrDefault(p => p.Email == rendezVousToAdd.PatientEmail);
                var praticien = _praticienRepository.GetAll().FirstOrDefault(p => p.Cin == rendezVousToAdd.PraticienCin);

                if (patient == null)
                {
                      return new Return<RendezVousResultDataModel>()
                     .Error().AsValidationFailure(null, "Pas de patient en cours.", "patient")
                     .WithDefaultResult();

                }
                   
                if(praticien == null)
                    return new Return<RendezVousResultDataModel>()
                  .Error().AsValidationFailure(null, "Pas de praticien en cours.", "praticien")
                  .WithDefaultResult();
                
                //add a new creneau before
                var creneau = new Creneaux
                {
                    CurrentDate = rendezVousToAdd.CurrentDate,
                    HeureDebut = rendezVousToAdd.HeureDebut,
                    HeureFin = rendezVousToAdd.HeureDebut,
                    Status = "ND", //Sera confirmé soit on supprime tout l'élément
                    Praticien = praticien
                };

                _creneauRepository.Add(creneau);

                //récupérer le créneau
                //var cren =
                //    _creneauRepository.GetAll()
                //        .FirstOrDefault(
                //            c =>
                //                c.CurrentDate.Equals(creneau.CurrentDate) && c.HeureDebut.Equals(creneau.HeureDebut) &&
                //                c.Praticien.Cin.Equals(praticien.Cin));
                //if (cren == null)
                //{
                //    cren = creneau;
                //}
                var rendezvous = new RendezVous
                {
                    Creneaux = creneau,
                    Praticien = praticien,
                    Patient = patient,
                    Statut = "NC",
                    NoteConsultation = rendezVousToAdd.NoteConsultation
                };

                _rendezVousRepository.Add(rendezvous);
              
                Logger.LogInfo("PostNewRendezVous : End. " );
                return
                    new Return<RendezVousResultDataModel>().OK()
                        .WithResult(new RendezVousResultDataModel
                        {
                            NoteConsultation = rendezvous.Statut,
                            PraticienEmail = rendezvous.Praticien.Email,
                            PatientCin = rendezvous.Patient.Email,
                            Statut = rendezvous.Statut,
                            CurrentDate = creneau.CurrentDate,
                            HeureFin = creneau.HeureDebut,
                            HeureDebut = creneau.HeureFin
                        });
            }
            catch (Exception ex)
            {
                throw;
            }

          

        }
        public ResultOfType<PatientResultModel> GetPatientsParPraticien(string praticien, int take = 0, int skip = 0)
        {
            try
            {

               
                Logger.LogInfo("GetPatientsParPraticien : Start ." );

                if (string.IsNullOrEmpty(praticien))
                    return new Return<PatientResultModel>()
                           .Error().AsValidationFailure(null, "Veuillez introduire votre praticien Cin.", "praticien")
                           .WithDefaultResult();
                var totalCount = _rendezVousRepository.GetCount();
                var totalPages = (take != 0) ? (int)Math.Ceiling((double)totalCount / take) : 0;

                var paginationHeader = new PaginationHeader
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages
                };
                var patients = (take == 0 && skip == 0)
                                  ? _rendezVousRepository
                                       .GetAll()
                                       .Where(x => x.Praticien.Email.Equals(praticien))
                                       .Select(x => x.Patient)
                                       .ToList()
                                       .Distinct()
                                  : _rendezVousRepository
                                       .GetAll()
                                       .Where(x => x.Praticien.Email.Equals(praticien))
                                       .Select(x => x.Patient)
                                       .Skip(skip)
                                       .Take(take)
                                       .Distinct()
                                       .ToList();

                var data = new PatientResultModel();

                patients.ForEach(account =>
                {
                    var patient = _patientRepository.GetAll().FirstOrDefault(p => p.Email.Equals(account.Email));
                    if (patient != null)
                    {
                        var dataModel = PatientWrapper.ConvertPatientEntityToDataModel(patient);
                        data.Items.Add(dataModel);
                    }
                    else
                    {
                        //le patient peut être un praticien donc on sélectionne même les praticiens
                        var prat = _praticienRepository.GetAll().FirstOrDefault(p => p.Email.Equals(account.Email));
                        var dataModel = PatientWrapper.ConvertPratientEntityToDataModel(prat);
                        data.Items.Add(dataModel);
                    }
                });

                data.PaginationHeader = paginationHeader;

                Logger.LogInfo("Get Patients With Take And Skip Parameters : End --- Status : OK");
                return new Return<PatientResultModel>().OK().WithResult(data);

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ResultOfType<RendezVousResultModel> GetRendezVousByDateAndPraticien(string praticien, string dateCurrent)
        {
            try
            {
               
                Logger.LogInfo("GetRendezVousByDateAndPraticien : Start." );

                if (string.IsNullOrEmpty(praticien))
                    return new Return<RendezVousResultModel>()
                           .Error().AsValidationFailure(null, "Veuillez introduire votre praticien Cin.", "praticien")
                           .WithDefaultResult();

                if (string.IsNullOrEmpty(dateCurrent))
                    return new Return<RendezVousResultModel>()
                           .Error().AsValidationFailure(null, "Veuillez introduire votre date en cours.", "dateCurrent")
                           .WithDefaultResult();

                var rendezvous =
                    _rendezVousRepository.GetAll()
                        .Where(r => r.Praticien.Email == praticien && r.Creneaux.CurrentDate.Equals(dateCurrent));

                var data = new RendezVousResultModel();


                rendezvous.ForEach(rdv =>
                {
                    var dataModel = RendezVousWrapper.ConvertPatientEntityToDataModel(rdv);
                    data.Items.Add(dataModel);
                });


                Logger.LogInfo("Get RendezVous By Date And Praticien : End --- Status : OK");
                return new Return<RendezVousResultModel>().OK().WithResult(data);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public ResultOfType<RdvResultModel> GetAllRendezVousParPatientEnCours(string patientEmail)
        {

            try
            {
                
                Logger.LogInfo("GetAllRendezVousParPatientEnCours : Start." );

                if (string.IsNullOrEmpty(patientEmail))
                    return new Return<RdvResultModel>()
                           .Error().AsValidationFailure(null, "Veuillez introduire votre patient Cin.", "praticien")
                           .WithDefaultResult();
                // Specify exactly how to interpret the string.
                IFormatProvider culture = new System.Globalization.CultureInfo("fr-FR", true);

                var rendezvous =
                 _rendezVousRepository.GetAll()
                     .Where(r => r.Patient.Email.Equals(patientEmail) );


                var data = new RdvResultModel();


                rendezvous.ForEach(rdv =>
                {
                    if (rdv.Creneaux != null && !string.IsNullOrEmpty(rdv.Creneaux.CurrentDate))
                    {
                        DateTime dt2 = DateTime.Parse(rdv.Creneaux.CurrentDate, culture,
                            System.Globalization.DateTimeStyles.AssumeLocal);
                        int result = DateTime.Compare(dt2, DateTime.Now);
                        if (result >= 0)
                        {
                            var dataModel = RendezVousWrapper.ConvertPatientEntityToRdvResultModel(rdv);
                        
                                //récupérer praticien
                           
                                var praticien = _praticienRepository.GetAll().FirstOrDefault(p => p.Cin.Equals(rdv.Praticien.Cin));
                                if (praticien != null)
                                {
                                    dataModel.PraticienNomPrenom = praticien.NomPrenom;
                                    dataModel.PraticienAdresse = string.Format("{0} {1} {2}", praticien.Adresse, praticien.Delegation, praticien.Gouvernerat);
                                    dataModel.PraticienSpecialite = praticien.Specialite;
                                }
                           
                            //get praticien info 
                           
                            data.Items.Add(dataModel);
                        }
                    }

                 
                });


                Logger.LogInfo("Get RendezVous By Date And Praticien : End --- Status : OK");
                return new Return<RdvResultModel>().OK().WithResult(data);


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Récupérer la liste des rendez vous non confirmé de chaque médecin
        /// </summary>
        /// <param name="praticienEmail"></param>
        /// <returns></returns>
        public ResultOfType<RendezVousResultModel> GetAllRendezVousNonConfirmeOuRejete(string praticienEmail)
        {
            DateTime currentDate = DateTime.Today;
               // Specify exactly how to interpret the string.
                IFormatProvider culture = new System.Globalization.CultureInfo("fr-FR", true);

            try
            {
                Logger.LogInfo("GetAllRendezVousNonConfirmeOuRejete : Start.");
                if (string.IsNullOrEmpty(praticienEmail))
                    return new Return<RendezVousResultModel>()
                           .Error().AsValidationFailure(null, "Veuillez introduire votre praticien Cin.", "praticien")
                           .WithDefaultResult();

                var rendezvous =
                       _rendezVousRepository.GetAll()
                           .Where(r => r.Praticien.Email == praticienEmail && r.Statut.Equals("NC") && DateTime.Parse(r.Creneaux.CurrentDate, culture, System.Globalization.DateTimeStyles.AssumeLocal).CompareTo(currentDate)>=0).OrderBy(o=>o.Creneaux.CurrentDate).Take(5);

                var data = new RendezVousResultModel();
               

                rendezvous.ForEach(rdv =>
                {
                    var dataModel = RendezVousWrapper.ConvertPatientEntityToDataModel(rdv);
                     //get patient info
                    var patient = _patientRepository.GetAll().FirstOrDefault(p => p.Email.Equals(dataModel.PatientCin));
                    if (patient != null)
                    {
                        if (!string.IsNullOrEmpty(patient.DateNaissance))
                        dataModel.PatientDateNaissance = patient.DateNaissance;
                        if (!string.IsNullOrEmpty(patient.NomPrenom))
                            dataModel.PatientNomPrenom = patient.NomPrenom;
                        if (!string.IsNullOrEmpty(patient.Adresse))
                            dataModel.PatientAdresse = patient.Adresse;
                        if (!string.IsNullOrEmpty(patient.Telephone))
                            dataModel.PatientTelephone = patient.Telephone;
                    }

                    data.Items.Add(dataModel);

                });


                Logger.LogInfo("Get RendezVous By Date And Praticien : End --- Status : OK");
                return new Return<RendezVousResultModel>().OK().WithResult(data);
            }
            catch (Exception ex)
            {
                throw;
            }
            

        }

        private static ResultOfType<RendezVousResultDataModel> IsValidGuidIdAndGeneratedByRendezVousApi(string rendezvousId)
        {
            Guid guidRendezVousIdToValidate;

            if (!Guid.TryParse(rendezvousId, out guidRendezVousIdToValidate))
                return new Return<RendezVousResultDataModel>()
                    .Error().AsValidationFailure(null, "Erreur dans l'Id du rendez vous.", "rendezvousId")
                    .WithDefaultResult();
            return null;
        }

        public ResultOfType<RendezVousResultDataModel> PatchNewRendezVous(RendezVousDataModel rendezVousToAdd)
        {
            try
            {
              
                Logger.LogInfo("PatchNewRendezVous : Start." );
                if (rendezVousToAdd == null)
                    return new Return<RendezVousResultDataModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                        null, "Les données sont vides.").WithDefaultResult();

                Logger.LogInfo(string.Format("Patch New rendez vous : Start --- PatientEmail = {0}, PatientEmail = {1}",
                                               rendezVousToAdd.PatientEmail, rendezVousToAdd.PatientEmail));

                var validationResult = ValidateNewRendezVousProperties(rendezVousToAdd);

                if (validationResult != null)
                {
                    Logger.LogInfo(string.Format("Patch New rendez vous : End --- Status = {0}, Message= {1}",
                                                 validationResult.Status, validationResult.Errors[0].Message));
                    return validationResult;
                }

                if (string.IsNullOrEmpty(rendezVousToAdd.HeureDebut))
                    return new Return<RendezVousResultDataModel>()
                           .Error().AsValidationFailure(null, "Veuillez introduire heure début.", "HeureDebut")
                           .WithDefaultResult();

                if (string.IsNullOrEmpty(rendezVousToAdd.CurrentDate))
                    return new Return<RendezVousResultDataModel>()
                           .Error().AsValidationFailure(null, "Veuillez introduire date courrante.", "CurrentDate")
                           .WithDefaultResult();

                //var validationId = IsValidGuidIdAndGeneratedByRendezVousApi(rendezVousToAdd.Id);
                //if (validationId != null)
                //{
                //    Logger.LogInfo(string.Format("Patch New rendez vous : End --- Status = {0}, Message= {1}",
                //                                validationId.Status, validationId.Errors[0].Message));
                //    return validationId;
                //}

                var rdv = _rendezVousRepository.GetAll().FirstOrDefault(r => r.Creneaux.CurrentDate.Equals(rendezVousToAdd.CurrentDate) && r.Creneaux.HeureDebut.Equals(rendezVousToAdd.HeureDebut) && r.Praticien.Email.Equals(rendezVousToAdd.PraticienCin));

                if (rdv == null)
                    return new Return<RendezVousResultDataModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                      null, "Un rendez vous à cette date et à cette heure pour  ce praticien a été pris déjà.").WithDefaultResult();

                var cren = _creneauRepository.GetAll().FirstOrDefault(cr=>cr.CurrentDate.Equals(rendezVousToAdd.CurrentDate) && cr.HeureDebut.Equals(rendezVousToAdd.HeureDebut));
                if(cren ==null)
                {
                           return new Return<RendezVousResultDataModel>()
                     .Error().AsValidationFailure(null, "Le creneau n'existe pas pour ce rendez vous.", "validationCreneauId")
                     .WithDefaultResult();
                }
              

                var patient = _userAccountRepository.GetAll().FirstOrDefault(p => p.Email == rendezVousToAdd.PatientEmail);
                var praticien = _praticienRepository.GetAll().FirstOrDefault(p => p.Email == rendezVousToAdd.PraticienCin);

                
                if (patient != null)
                    rdv.Patient = patient;
                if (praticien != null)
                    rdv.Praticien = praticien;
                RendezVousResultDataModel data;
                //Dans le cas de rejet supprimer créneau lié et garder le RDV en tant que trace
                if (rendezVousToAdd.Statut.Equals("R"))
                {
                    //Ajouter ce rdv dans l'historique
                    var historique = new HistoriqueRendezVous
                    {
                        CreneauxHeureDebut = cren.HeureDebut,
                        CurrentDate = cren.CurrentDate,
                        Patient = rdv.Patient,
                        Praticien = rdv.Praticien,
                        Statut = rdv.Statut
                    };

                    _historiqueRdvRepository.Add(historique);
                    //on doit supprimer les rendez vous ainsi que les créneau non disponible
                    _creneauRepository.Delete(cren.Id);
                    _rendezVousRepository.Delete(rdv.Id);


                     data = new RendezVousResultDataModel
                    {
                        NoteConsultation = historique.Statut,
                        PraticienEmail = historique.Praticien.Email,
                        PatientCin = historique.Patient.Email,
                        Statut = historique.Statut,
                        CurrentDate = historique.CurrentDate,
                        PraticienAdresseDetaille = String.Format("{0} {1} {2}", rdv.Praticien.Adresse, rdv.Praticien.Delegation, rdv.Praticien.Gouvernerat),
                        PraticienSpecialite = rdv.Praticien.Specialite,
                        PraticienTelephone = rdv.Praticien.Telephone,
                        PraticinNomPrenom = rdv.Praticien.NomPrenom,
                        HeureDebut = historique.CreneauxHeureDebut
                    };
                }
                else
                {
                    rdv.Statut = rendezVousToAdd.Statut;
                    //juste on affecte le créneau
                    rdv.Creneaux = cren;

                    if (rendezVousToAdd.NoteConsultation != null)
                        rdv.NoteConsultation = rdv.NoteConsultation;

                    _rendezVousRepository.Update(rdv);
                    data = new RendezVousResultDataModel
                    {
                        NoteConsultation = rdv.Statut,
                        PraticienEmail = rdv.Praticien.Email,
                        PatientCin = rdv.Patient.Email,
                        Statut = rdv.Statut,
                        CurrentDate = rdv.Creneaux.CurrentDate,
                        HeureFin = rdv.Creneaux.HeureDebut,
                        HeureDebut = rdv.Creneaux.HeureFin,
                        PraticienAdresseDetaille = String.Format("{0} {1} {2}", rdv.Praticien.Adresse, rdv.Praticien.Delegation, rdv.Praticien.Gouvernerat),
                        PraticienSpecialite = rdv.Praticien.Specialite,
                        PraticienTelephone = rdv.Praticien.Telephone,
                        PraticinNomPrenom = rdv.Praticien.NomPrenom
                    };

                }
               

                return
                     new Return<RendezVousResultDataModel>().OK()
                         .WithResult(data);
            }
            catch (Exception)
            {
                
                throw;
            }
        
        }

        private ResultOfType<RendezVousResultDataModel> ValidateNewRendezVousProperties(RendezVousDataModel rendezVousToAdd)
        {
           

           if (string.IsNullOrEmpty(rendezVousToAdd.PatientEmail))
                return new Return<RendezVousResultDataModel>()
                       .Error().AsValidationFailure(null, "Veuillez introduire votre Patient Cin.", "PatientEmail")
                       .WithDefaultResult();

            if (string.IsNullOrEmpty(rendezVousToAdd.PraticienCin))
                return new Return<RendezVousResultDataModel>()
                       .Error().AsValidationFailure(null, "Veuillez introduire votre Praticien Cin.", "PraticienCin")
                       .WithDefaultResult();

            if (string.IsNullOrEmpty(rendezVousToAdd.Statut))
                return new Return<RendezVousResultDataModel>()
                       .Error().AsValidationFailure(null, "Veuillez introduire votre Statut.", "Statut")
                       .WithDefaultResult();

         

            return null;
        }

        public ResultOfType<Null> DeleteRendezVous(string rendezVousId)
        {
            if (string.IsNullOrEmpty(rendezVousId))
                return new Return<Null>()
                       .Error().AsValidationFailure(null, "Aucun rendez vous a été passé.", "rendezVousId")
                       .WithDefaultResult();

            try
            {
                Guid idRendezVous = new Guid(rendezVousId);// Guid.Parse(rendezVousId);
                var rendezvous =
                    _rendezVousRepository.GetAll().FirstOrDefault(r => r.Id == idRendezVous);

                if (rendezvous == null)
                {

                    return new Return<Null>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                           null, "Aucun rendez vous existe.").WithDefaultResult();
                }

                _rendezVousRepository.Delete(idRendezVous);
                _creneauRepository.Delete(rendezvous.Creneaux.Id);
                return new Return<Null>().OK().WithDefaultResult();
            }
            catch (Exception ex)
            {
                Logger.LogInfo(string.Format("Delete rendez vous : end error --- Exception = {0}", ex.Message));

                throw;
            }
        }


        public ResultOfType<RendezVousResultDataModel> CreneauAyantRendezVous(string praticien, string dateCurrent, string heureDebut)
        {
           
            Logger.LogInfo("CreneauAyantRendezVous : Start." );

            if (string.IsNullOrEmpty(praticien))
                return new Return<RendezVousResultDataModel>()
                 .Error().AsValidationFailure(null, "Veuillez introduire votre Praticien Cin.", "Praticien")
                 .WithDefaultResult();

            if (string.IsNullOrEmpty(dateCurrent))
                return new Return<RendezVousResultDataModel>()
                .Error().AsValidationFailure(null, "Veuillez introduire votre currentDate.", "DateCurrent")
                .WithDefaultResult();

            if (string.IsNullOrEmpty(heureDebut))
                return new Return<RendezVousResultDataModel>()
                 .Error().AsValidationFailure(null, "Veuillez introduire votre Praticien Cin.", "heureDebut")
                 .WithDefaultResult();
            var creneau = _creneauRepository.GetAll().FirstOrDefault(f => f.CurrentDate.Equals(dateCurrent) && f.HeureDebut.Equals(heureDebut) && f.Praticien.Email.Equals(praticien));
            if (creneau != null)
            {

                var rdv = _rendezVousRepository.GetAll().FirstOrDefault(r => r.Creneaux.Id == creneau.Id);
                if (rdv == null)
                {
                    return
                    new Return<RendezVousResultDataModel>().OK()
                        .WithResult(new RendezVousResultDataModel()
                        {

                        });
                }
                else
                {
                    //if(rdv.Statut.Equals())
                    return
                    new Return<RendezVousResultDataModel>().OK()
                        .WithResult(new RendezVousResultDataModel
                        {
                            NoteConsultation = rdv.Statut,
                            PraticienEmail = rdv.Praticien.Email,
                            PatientCin = rdv.Patient.Email,
                            Statut = rdv.Statut,
                            CurrentDate = rdv.Creneaux.CurrentDate,
                            HeureFin = rdv.Creneaux.HeureDebut,
                            HeureDebut = rdv.Creneaux.HeureFin,
                            PraticienAdresseDetaille = string.Format("{0} {1} {2}", rdv.Praticien.Adresse, rdv.Praticien.Delegation, rdv.Praticien.Gouvernerat),
                            PraticienSpecialite = rdv.Praticien.Specialite,
                            PraticinNomPrenom = rdv.Praticien.NomPrenom,
                            PraticienTelephone = rdv.Praticien.Telephone
                        });
                }
            }
            else
            {
                return new Return<RendezVousResultDataModel>()
                 .Error().AsValidationFailure(null, "aucun créneau à cette date.", "creneau")
                 .WithDefaultResult();
            }

        }

      
      

    }
}
