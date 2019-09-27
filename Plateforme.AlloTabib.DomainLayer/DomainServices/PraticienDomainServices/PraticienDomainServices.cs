using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NHibernate.Linq;
using Plateforme.AlloTabib.CrossCuttingLayer.Logging;
using Plateforme.AlloTabib.DomainLayer.Base.Interfaces;
using Plateforme.AlloTabib.DomainLayer.Entities;
using Plateforme.AlloTabib.DomainLayer.Helpers;
using Plateforme.AlloTabib.DomainLayer.Models;
using Plateforme.AlloTabib.DomainLayer.SearchEngine;
using PlateformeAlloTabib.Standards.Domain;
using PlateformeAlloTabib.Standards.Helpers;

namespace Plateforme.AlloTabib.DomainLayer.DomainServices.PraticienDomainServices
{
    public class PraticienDomainServices : IPraticienDomainServices
    {
        #region Private Properties

        private readonly IRepository<Praticien> _praticienRepository;
        private readonly IRepository<RendezVous> _rendezVousRepository;
        private readonly IRepository<UserAccount> _userAccountRepository;
        private readonly IRepository<GoogleMapsCoordinations> _googleMapsCoordinations;
        private readonly IRepository<ConfigurationPraticien> _configurationPraticienRepository;
        private readonly ILuceneSearchEngine<PraticienToIndexModel> _searchEngine;

        private RessourceManager _ressourceManager;
        public IRepository<ConfigurationPraticien> ConfigurationPraticienRepository
        {
            get { return _configurationPraticienRepository; }
        }

        #endregion

        public PraticienDomainServices(IRepository<RendezVous> rendezVousRepository,IRepository<Praticien> praticienRepository, IRepository<UserAccount> userAccountRepository, IRepository<GoogleMapsCoordinations> googleMapsCoordinations, IRepository<ConfigurationPraticien> configurationPraticienRepository, ILuceneSearchEngine<PraticienToIndexModel> searchEngine)
        {
            if (praticienRepository == null)
                throw new ArgumentNullException("praticienRepository");
            _praticienRepository = praticienRepository;
            if (userAccountRepository == null)
                throw new ArgumentNullException("userAccountRepository");
            _userAccountRepository = userAccountRepository;
            if (googleMapsCoordinations == null)
                throw new ArgumentNullException("googleMapsCoordinations");
            _googleMapsCoordinations = googleMapsCoordinations;
            if (configurationPraticienRepository == null)
                throw new ArgumentNullException("configurationPraticienRepository");
            _configurationPraticienRepository = configurationPraticienRepository;
            if (searchEngine == null)
                throw new ArgumentNullException("searchEngine");
            _searchEngine = searchEngine;
            if(rendezVousRepository == null)
                throw new ArgumentNullException("rendezVousRepository");
            _rendezVousRepository = rendezVousRepository;
            _ressourceManager = RessourceManager.getInstance();

        }
        public IList<DateTime> PrintSundays()
        {
            var lstSundays = new List<DateTime>();
            int intMonth = DateTime.Now.Month;
            int intYear = DateTime.Now.Year;
            int intDaysThisMonth = DateTime.DaysInMonth(intYear, intMonth);
            var oBeginnngOfThisMonth = new DateTime(intYear, intMonth, 1);
            for (int i = 1; i < intDaysThisMonth + 1; i++)
            {
                if (oBeginnngOfThisMonth.AddDays(i).DayOfWeek == DayOfWeek.Sunday)
                {
                    lstSundays.Add(new DateTime(intYear, intMonth, i));
                }
            }
            return lstSundays;
        }
        public ResultOfType<PraticienResultModel> GetPraticiens(int take = 0, int skip = 0)
        {
            try
            {
                string local = _ressourceManager.LocalIPAddress();
                string l = local;
                Logger.LogInfo("Get Praticiens With Take And Skip Parameters avec @IP : Start." );
                PrintSundays();
              

                var totalCount = _praticienRepository.GetCount();
                var totalPages = (take != 0) ? (int)Math.Ceiling((double)totalCount / take) : 0;

                var paginationHeader = new PaginationHeader
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages
                };

                var praticiens = (take == 0 && skip == 0)
                                   ? _praticienRepository
                                        .GetAll()
                                        .OrderBy(a => a.CreationDate)
                                        .ToList()
                                   : _praticienRepository
                                        .GetAll()
                                        .OrderBy(a => a.CreationDate)
                                        .Skip(skip)
                                        .Take(take)
                                        .ToList();

                var data = new PraticienResultModel();

                praticiens.ForEach(praticien =>
                {
                    var dataModel = PraticienWrapper.ConvertPraticienEntityToDataModel(praticien);
                    data.Items.Add(dataModel);
                });

                data.PaginationHeader = paginationHeader;

                Logger.LogInfo("Get Praticiens With Take And Skip Parameters : End --- Status : OK");
                return new Return<PraticienResultModel>().OK().WithResult(data);
            }
            catch (Exception exception)
            {
                Logger.LogError("Get Praticiens Exception", exception);
                throw;
            }
        }

        public IEnumerable<Praticien> GetAll()
        {
            return _praticienRepository.GetAll();
        }

        public void AddNewPraticien(Praticien praticien)
        {
             _praticienRepository.Add(praticien);
        }

        public void ModifyPraticien(Praticien praticien)
        {
            _praticienRepository.Update(praticien);
        }

        public void DeletePraticien(object id)
        {
           _praticienRepository.Delete(id);
        }

        public ResultOfType<PraticienDataModel> DeleteAPraticien(string cin)
        {
           
            Logger.LogInfo("DeleteAPraticien avec @IP : Start." );

            if (string.IsNullOrEmpty(cin))
                return new Return<PraticienDataModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                    null, "Veuillez introduire un cin ou praticien possédant un cin vide !!!!").WithDefaultResult();
            Logger.LogInfo(string.Format("Delete patient : Start --- CIN = {0}", cin));

            try
            {
                _praticienRepository.Delete(cin);
                var praticien = _praticienRepository.GetAll().FirstOrDefault(p => p.Cin.Equals(cin));
                if(praticien == null)
                    return new Return<PraticienDataModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                   null, "Le praticien mentionnée n'existe pas pour effectuer la suppression !!!!").WithDefaultResult();

                _userAccountRepository.Delete(praticien.Email);
                Logger.LogInfo("DeleteAPraticien avec @IP : Start.");
                return new Return<PraticienDataModel>().OK().WithResult(new PraticienDataModel { Cin = cin });
            }
            catch (Exception ex)
            {
                Logger.LogInfo(string.Format("Delete praticien : end error --- Exception = {0}", ex.Message));
                return new Return<PraticienDataModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                    null, "Erreur suite à une exception avec notre serveur.").WithDefaultResult();

            }

        }

        public ResultOfType<PraticienResultDataModel> PatchNewPraticien(PraticienDataModel praticienToAdd)
        {

            if (praticienToAdd == null)
                return new Return<PraticienResultDataModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                    null, "Les données sont vides.").WithDefaultResult();
          
            Logger.LogInfo(string.Format("Patch New Praticien : Start --- CIN = {0}, Email = {1} ", praticienToAdd.Cin, praticienToAdd.Email));

       

         
            // Id validation
            var validationResult = ValidateUpdatePraticienProperties(praticienToAdd);
            if (validationResult != null)
            {
                Logger.LogInfo(string.Format("Patch New Praticien : End --- Status = {0}, Message= {1}",
                                             validationResult.Status, validationResult.Errors[0].Message));
                return validationResult;
            }

            validationResult = ValidatePasword(praticienToAdd.Password, praticienToAdd.NomPrenom);
            if (validationResult != null)
            {
                Logger.LogInfo(string.Format("Pach New Praticien : End --- Status = {0}, Message= {1}",
                                             validationResult.Status, validationResult.Errors[0].Message));
                return validationResult;
            }

            //Activer tous les praticiens actuellement avant d'être active après le paiement

            if (praticienToAdd.Conventionne == null)
            {
                praticienToAdd.Conventionne = "false";
            }
            if (string.IsNullOrWhiteSpace(praticienToAdd.PrixConsultation))
            {
                praticienToAdd.PrixConsultation = "Non définit";
            }

            var praticien = _praticienRepository.GetAll().FirstOrDefault(p => p.Cin == praticienToAdd.Cin);

            if(praticien == null)
                return new Return<PraticienResultDataModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                   null, "Aucun praticin existe.").WithDefaultResult();

            if (!string.IsNullOrEmpty(praticienToAdd.Adresse))
            praticien.Adresse = praticienToAdd.Adresse;
            if (!string.IsNullOrEmpty(praticienToAdd.Password))
            praticien.Password = CrossCuttingLayer.Encryption.RijndaelEncryption.Encrypt(praticienToAdd.Password);
            if (!string.IsNullOrEmpty(praticienToAdd.NomPrenom))
            praticien.NomPrenom = praticienToAdd.NomPrenom;
            if (!string.IsNullOrEmpty(praticienToAdd.Telephone))
            praticien.Telephone = praticienToAdd.Telephone;
            if (!string.IsNullOrEmpty(praticienToAdd.Delegation))
            praticien.Delegation = praticienToAdd.Delegation;
            if (!string.IsNullOrEmpty(praticienToAdd.Gouvernerat))
            praticien.Gouvernerat = praticienToAdd.Gouvernerat;
            if (!string.IsNullOrEmpty(praticienToAdd.Conventionne))
            praticien.Conventionne = bool.Parse(praticienToAdd.Conventionne);
            if (!string.IsNullOrEmpty(praticienToAdd.Cursus))
            praticien.Cursus = praticienToAdd.Cursus;
            if (!string.IsNullOrEmpty(praticienToAdd.Diplomes))
            praticien.Diplomes = praticienToAdd.Diplomes;
            if (!string.IsNullOrEmpty(praticienToAdd.Formations))
            praticien.Formations = praticienToAdd.Formations;
            if (!string.IsNullOrEmpty(praticienToAdd.InformationsPratique))
            praticien.InformationsPratique = praticienToAdd.InformationsPratique;
            if (!string.IsNullOrEmpty(praticienToAdd.LanguesParles))
            praticien.LanguesParles = praticienToAdd.LanguesParles;
            if (!string.IsNullOrEmpty(praticienToAdd.MoyensPaiement))
            praticien.MoyensPaiement = praticienToAdd.MoyensPaiement;
            if (!string.IsNullOrEmpty(praticienToAdd.ParcoursHospitalier))
            praticien.ParcoursHospitalier = praticienToAdd.ParcoursHospitalier;
            if (!string.IsNullOrEmpty(praticienToAdd.PresentationCabinet))
            praticien.PresentationCabinet = praticienToAdd.PresentationCabinet;
            if (!string.IsNullOrEmpty(praticienToAdd.PrixConsultation))
            praticien.PrixConsultation = praticienToAdd.PrixConsultation;
            if (!string.IsNullOrEmpty(praticienToAdd.Fax))
            praticien.Fax = praticienToAdd.Fax;
            if (!string.IsNullOrEmpty(praticienToAdd.Publication))
            praticien.Publication = praticienToAdd.Publication;
            if (!string.IsNullOrEmpty(praticienToAdd.Specialite))
            praticien.Specialite = praticienToAdd.Specialite;

            _praticienRepository.Update(praticien);

            Logger.LogInfo("Patch New Praticien : End --- Status = OK, Message= {1}");
            return
                new Return<PraticienResultDataModel>().OK()
                    .WithResult(new PraticienResultDataModel
                    {
                        Cin = praticien.Cin,
                        Adresse = praticien.Adresse,
                        Email = praticien.Email,
                        NomPrenom = praticien.NomPrenom,
                        Telephone = praticien.Telephone,
                        Delegation = praticien.Delegation,
                        Fax = praticien.Fax,
                        Gouvernerat = praticien.Gouvernerat,
                        Specialite = praticien.Specialite
                    });
        }

        public ResultOfType<PraticienResultDataModel> PostNewPraticien(PraticienDataModel praticienToAdd)
        {
            if (praticienToAdd == null)
                return new Return<PraticienResultDataModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                    null, "Les données sont vides.").WithDefaultResult();
            
            Logger.LogInfo(string.Format("Post New Praticien : Start --- CIN = {0}, Email = {1} ", praticienToAdd.Cin, praticienToAdd.Email));

            var validationResult = ValidateNewPraticienProperties(praticienToAdd);
            if (validationResult != null)
            {
                Logger.LogInfo(string.Format("Post New Praticien : End --- Status = {0}, Message= {1}",
                                             validationResult.Status, validationResult.Errors[0].Message));
                return validationResult;
            }

            validationResult = ValidatePasword(praticienToAdd.Password, praticienToAdd.NomPrenom);
            if (validationResult != null)
            {
                Logger.LogInfo(string.Format("Post New Praticien : End --- Status = {0}, Message= {1}",
                                             validationResult.Status, validationResult.Errors[0].Message));
                return validationResult;
            }

            //Activer tous les praticiens actuellement avant d'être active après le paiement

            if (praticienToAdd.Conventionne == null)
            {
                praticienToAdd.Conventionne = "false";
            }
            if (string.IsNullOrWhiteSpace(praticienToAdd.PrixConsultation))
            {
                praticienToAdd.PrixConsultation = "Non définit";
            }
            var praticien = new Praticien
            {
                Adresse = praticienToAdd.Adresse,
                Password = CrossCuttingLayer.Encryption.RijndaelEncryption.Encrypt(praticienToAdd.Password),
                Cin = praticienToAdd.Cin,
                Email = praticienToAdd.Email,
                NomPrenom = praticienToAdd.NomPrenom,
                Telephone = praticienToAdd.Telephone,
                Delegation = praticienToAdd.Delegation,
                Gouvernerat = praticienToAdd.Gouvernerat,
                Conventionne = bool.Parse(praticienToAdd.Conventionne),
                Cursus = praticienToAdd.Cursus,
                Diplomes = praticienToAdd.Diplomes,
            };

          
            praticien.Formations = praticienToAdd.Formations;
            praticien.InformationsPratique = praticienToAdd.InformationsPratique;
            praticien.LanguesParles = praticienToAdd.LanguesParles;
            praticien.MoyensPaiement = praticienToAdd.MoyensPaiement;
            praticien.ParcoursHospitalier = praticienToAdd.ParcoursHospitalier;
            praticien.PresentationCabinet = praticienToAdd.PresentationCabinet;
            praticien.PrixConsultation = praticienToAdd.PrixConsultation;
            praticien.Fax = praticienToAdd.Fax;
            praticien.Publication = praticienToAdd.Publication;
            praticien.Specialite = praticienToAdd.Specialite;
            

            var userAccount = new UserAccount
            {
                EstActive = false,
                Email = praticienToAdd.Email,
                LastModificationDate = praticien.LastModificationDate,
                Password = praticien.Password,
                Type = "Praticien"
            };


        


            try
            {
                //EmailTest emailTest = new EmailTest();
                //bool isCorrect = emailTest.chkEmailExist(praticien.Email);
                //if (isCorrect)
                //{
                    
                    //ajout d'un compte
                    _praticienRepository.Add(praticien);
                    _userAccountRepository.Add(userAccount);



                    Logger.LogInfo(string.Format("Post New Praticien : End --- Status = OK, Email = {0} - Cin = {1} - Adresse = {2} - NomPrenom = {3} - Telephone = {4} - Delegation = {5} - Fax = {6} - Gouvernerat = {7} ", praticien.Email, praticien.Cin, praticien.Adresse, praticien.NomPrenom, praticien.Telephone, praticien.Delegation, praticien.Fax, praticien.Gouvernerat));
                    return
                        new Return<PraticienResultDataModel>().OK()
                            .WithResult(new PraticienResultDataModel
                            {
                                Cin = praticien.Cin,
                                Adresse = praticien.Adresse,
                                Email = praticien.Email,
                                NomPrenom = praticien.NomPrenom,
                                Telephone = praticien.Telephone,
                                Delegation = praticien.Delegation,
                                Fax = praticien.Fax,
                                Gouvernerat = praticien.Gouvernerat
                            });
                //}
                //else
                //{
                //    //tracer par log ces informations
                //    Logger.LogInfo(string.Format("Post New Praticien : End --- Status = KO - Erreur de saisie d'email qui n'existe pas, Email = {0} - Cin = {1} - Adresse = {2} - NomPrenom = {3} - Telephone = {4} - Delegation = {5} - Fax = {6} - Gouvernerat = {7} ", praticien.Email, praticien.Cin, praticien.Adresse, praticien.NomPrenom, praticien.Telephone, praticien.Delegation, praticien.Fax, praticien.Gouvernerat));
                //    return
                //    new Return<PraticienResultDataModel>()
                //    .Error()
                //    .AsValidationFailure(null,
                //        "Votre adresse Email n'existe pas, veuillez vérifier la syntaxe !",
                //    "Email")
                //    .WithDefaultResult();

                //}


            }
            catch (Exception ex)
            {
                var exception = ex.Message;
                return
                    new Return<PraticienResultDataModel>()
                    .Error()
                    .AsValidationFailure(null,
                        "Votre adresse Email n'existe pas, veuillez vérifier la syntaxe !",
                    "Email")
                    .WithDefaultResult();
            }

        
          

        }

        /// <summary>
        /// Validation Praticien
        /// </summary>
        /// <param name="praticienDataModel">Praticien.</param>
        /// <returns>Null if the id is valid, a specific error message otherwise.</returns>
        private ResultOfType<PraticienResultDataModel> ValidateNewPraticienProperties(PraticienDataModel praticienDataModel)
        {
            #region validation CIN

            if (string.IsNullOrEmpty(praticienDataModel.Cin))
                return new Return<PraticienResultDataModel>()
                   .Error().AsValidationFailure(null, "Veuillez introduire votre CIN.", "CIN")
                   .WithDefaultResult();

            if (CinExists(praticienDataModel.Cin))
                return new Return<PraticienResultDataModel>()
                    .Error().AsValidationFailure(null, "Le CIN introduit est utilisé déjà par un autre utilisateur.", "CIN")
                    .WithDefaultResult();

            if (praticienDataModel.Cin.Length != 8)
                return new Return<PraticienResultDataModel>()
                         .Error().AsValidationFailure(null, "Le CIN introduit est incorrecte.", "CIN")
                         .WithDefaultResult();

            var myRegex = new Regex(@"[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]");
            var isNumber = myRegex.IsMatch(praticienDataModel.Cin);
            if (isNumber == false)
                return new Return<PraticienResultDataModel>()
                         .Error().AsValidationFailure(null, "Le CIN introduit est incorrecte, elle est composé de 8 chiffres.", "CIN")
                         .WithDefaultResult();

            #endregion

            #region Email

            if (string.IsNullOrEmpty(praticienDataModel.Email))
                return new Return<PraticienResultDataModel>()
                  .Error().AsValidationFailure(null, "Veuillez introduire votre Email.", "Email")
                  .WithDefaultResult();
            if (EmailExists(praticienDataModel.Email))
                return new Return<PraticienResultDataModel>()
                  .Error().AsValidationFailure(null, "L'Email introduit est utilisé déjà par un autre utilisateur.", "Email")
                  .WithDefaultResult();
            if (ValidateEmail(praticienDataModel.Email))
                return new Return<PraticienResultDataModel>()
                .Error().AsValidationFailure(null, "Le format de l'email introduit est incorrecte : exemple : xxx@domain.xxx.", "Email")
                .WithDefaultResult();

            #endregion

            #region Telephone

            if (string.IsNullOrEmpty(praticienDataModel.Telephone))
                return new Return<PraticienResultDataModel>()
                  .Error().AsValidationFailure(null, "Veuillez introduire votre téléphone.", "Téléphone")
                  .WithDefaultResult();

            if (praticienDataModel.Telephone.Length > 30)
                return new Return<PraticienResultDataModel>()
                    .Error().AsValidationFailure(null, "Fax number length must be between 0 and 30 characters", "Téléphone")
                    .WithDefaultResult();

            if (!Regex.IsMatch(praticienDataModel.Telephone, @"^[0-9+() ][0-9+() -]*"))
                return new Return<PraticienResultDataModel>()
                    .Error().AsValidationFailure(null, "Le numéro de téléphone introduit est incorrecte.", "Téléphone")
                    .WithDefaultResult();
            #endregion

            #region Adresse

            if (string.IsNullOrEmpty(praticienDataModel.Adresse))
                return new Return<PraticienResultDataModel>()
                  .Error().AsValidationFailure(null, "Veuillez introduire votre téléphone.", "Téléphone")
                  .WithDefaultResult();

            #endregion

            //#region heure début matin
            //if (string.IsNullOrEmpty(praticienDataModel.HeureDebutMatin))
            //{
            //    return new Return<PraticienResultDataModel>()
            //     .Error().AsValidationFailure(null, "Veuillez introduire votre heure début matin.", "HeureDebutMatin")
            //     .WithDefaultResult();
            //}

            //#endregion
            //#region Heure début Midi

            //if (string.IsNullOrEmpty(praticienDataModel.HeureDebutMidi))
            //{
            //    return new Return<PraticienResultDataModel>()
            //   .Error().AsValidationFailure(null, "Veuillez introduire votre heure début midi.", "HeureDebutMidi")
            //   .WithDefaultResult();
            //}

            //#endregion

            //#region HeureFinMatin

            //if (string.IsNullOrEmpty(praticienDataModel.HeureFinMatin))
            //{
            //    return new Return<PraticienResultDataModel>()
            //  .Error().AsValidationFailure(null, "Veuillez introduire votre heure fin matin.", "HeureFinMatin")
            //  .WithDefaultResult();
            //}

            //#endregion

            //#region HeureFinMidi

            //if (string.IsNullOrEmpty(praticienDataModel.HeureFinMidi))
            //{
            //    return new Return<PraticienResultDataModel>()
            // .Error().AsValidationFailure(null, "Veuillez introduire votre heure fin midi.", "HeureFinMidi")
            // .WithDefaultResult(); 
            //}

            //#endregion




            //#region MinuteDebutMatin

            //if (string.IsNullOrEmpty(praticienDataModel.MinuteDebutMatin))
            //{
            //    return new Return<PraticienResultDataModel>()
            //.Error().AsValidationFailure(null, "Veuillez introduire votre minute début matin.", "MinuteDebutMatin")
            //.WithDefaultResult(); 
            //}

            //#endregion

            //#region

            //if (string.IsNullOrEmpty(praticienDataModel.MinuteDebutMidi))
            //{
            //    return new Return<PraticienResultDataModel>()
            //        .Error()
            //        .AsValidationFailure(null, "Veuillez introduire votre minute début midi.", "MinuteDebutMidi")
            //        .WithDefaultResult();
            //}

            //#endregion

            //#region MinuteFinMatin

            //if (string.IsNullOrEmpty(praticienDataModel.MinuteFinMatin))
            //{
            //    return new Return<PraticienResultDataModel>()
            //      .Error()
            //      .AsValidationFailure(null, "Veuillez introduire votre minute fin matin.", "MinuteFinMatin")
            //      .WithDefaultResult();
            //}

            //#endregion

            //#region

            //if (string.IsNullOrEmpty(praticienDataModel.MinuteDebutMatin))
            //{
            //    return new Return<PraticienResultDataModel>()
            //    .Error()
            //    .AsValidationFailure(null, "Veuillez introduire votre minute début matin.", "MinuteDebutMatin")
            //    .WithDefaultResult();
            //}

            //#endregion


            //#region

            //if (string.IsNullOrEmpty(praticienDataModel.MinuteFinMidi))
            //{
            //    return new Return<PraticienResultDataModel>()
            //  .Error()
            //  .AsValidationFailure(null, "Veuillez introduire votre minute fin midi.", "MinuteFinMidi")
            //  .WithDefaultResult();
            //}

            //#endregion

            //if (int.Parse(praticienDataModel.HeureDebutMatin) < int.Parse(praticienDataModel.HeureFinMatin))
            //{
            //    return new Return<PraticienResultDataModel>()
            // .Error()
            // .AsValidationFailure(null, "L'heure de début matin doit être supérieur à l'heure de fin matin.", "MinuteFinMidi")
            // .WithDefaultResult();
            //}

            //if (int.Parse(praticienDataModel.HeureDebutMatin) < int.Parse(praticienDataModel.HeureFinMatin))
            //{
            //    return new Return<PraticienResultDataModel>()
            // .Error()
            // .AsValidationFailure(null, "L'heure de début matin doit être supérieur à l'heure de fin matin.", "MinuteFinMidi")
            // .WithDefaultResult();
            //}

            return null;
        }


        private ResultOfType<PraticienResultDataModel> ValidateUpdatePraticienProperties(PraticienDataModel praticienDataModel)
        {
            #region validation CIN

            if (string.IsNullOrEmpty(praticienDataModel.Cin))
                return new Return<PraticienResultDataModel>()
                   .Error().AsValidationFailure(null, "Veuillez introduire votre CIN.", "CIN")
                   .WithDefaultResult();

            if (praticienDataModel.Cin.Length != 8)
                return new Return<PraticienResultDataModel>()
                         .Error().AsValidationFailure(null, "Le CIN introduit est incorrecte.", "CIN")
                         .WithDefaultResult();

            var myRegex = new Regex(@"[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]");
            var isNumber = myRegex.IsMatch(praticienDataModel.Cin);
            if (isNumber == false)
                return new Return<PraticienResultDataModel>()
                         .Error().AsValidationFailure(null, "Le CIN introduit est incorrecte, elle est composé de 8 chiffres.", "CIN")
                         .WithDefaultResult();

            #endregion

            #region Email

            if (string.IsNullOrEmpty(praticienDataModel.Email))
                return new Return<PraticienResultDataModel>()
                  .Error().AsValidationFailure(null, "Veuillez introduire votre Email.", "Email")
                  .WithDefaultResult();

           // On ne va pas donner accès pour changer l'email ni le cin

            #endregion

            #region Telephone

            if (string.IsNullOrEmpty(praticienDataModel.Telephone))
                return new Return<PraticienResultDataModel>()
                  .Error().AsValidationFailure(null, "Veuillez introduire votre téléphone.", "Téléphone")
                  .WithDefaultResult();

            if (praticienDataModel.Telephone.Length > 30)
                return new Return<PraticienResultDataModel>()
                    .Error().AsValidationFailure(null, "Fax number length must be between 0 and 30 characters", "Téléphone")
                    .WithDefaultResult();

            if (!Regex.IsMatch(praticienDataModel.Telephone, @"^[0-9+() ][0-9+() -]*"))
                return new Return<PraticienResultDataModel>()
                    .Error().AsValidationFailure(null, "Le numéro de téléphone introduit est incorrecte.", "Téléphone")
                    .WithDefaultResult();
            #endregion

            #region Adresse

            if (string.IsNullOrEmpty(praticienDataModel.Adresse))
                return new Return<PraticienResultDataModel>()
                  .Error().AsValidationFailure(null, "Veuillez introduire votre téléphone.", "Téléphone")
                  .WithDefaultResult();

            #endregion

            return null;
        }

        /// <summary>
        /// vérifier si le cin existe ou pas
        /// </summary>
        /// <param name="cin"></param>
        /// <returns></returns>
        private bool CinExists(string cin)
        {
            return _praticienRepository.GetAll().Any(a => a.Cin == cin);
        }

        private bool EmailExists(string email)
        {
            return _userAccountRepository.GetAll().Any(c => c.Email == email);
        }

        private bool ValidateEmail(string emailAddress)
        {
            return Regex.IsMatch(emailAddress,
                              @"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$");
        }

        private ResultOfType<PraticienResultDataModel> ValidatePasword(string password, string nomprenom)
        {
            if (string.IsNullOrWhiteSpace(password))
                return
                    new Return<PraticienResultDataModel>()
                    .Error()
                    .AsValidationFailure(null,
                        "Veuillez introduire un mot de passe.",
                    "Mot de passe")
                    .WithDefaultResult();

            if (password.Length > 20 || password.Length < 5)
                return
                    new Return<PraticienResultDataModel>()
                    .Error()
                    .AsValidationFailure(null,
                        "Le mot de passe est composé de  5 et 20 caractères.",
                    "Mot de passe")
                    .WithDefaultResult();

            var split = nomprenom.Split(Convert.ToChar(" "));

            if (split.Length == 2 && split[0] != null)
            {
                if (password.Contains(split[0]))
                    return
                        new Return<PraticienResultDataModel>()
                        .Error()
                        .AsValidationFailure(null,
                            "Le mot de passe ne doit pas contenir votre nom ou votre prenom",
                        "Mot de passe")
                        .WithDefaultResult();
            }
            if (split.Length == 2 && split[1] != null)
            {
                if (password.Contains(split[1]))
                    return
                        new Return<PraticienResultDataModel>()
                            .Error()
                            .AsValidationFailure(null,
                                "Le mot de passe ne doit pas contenir votre nom ou votre prenom",
                                "Mot de passe")
                            .WithDefaultResult();
            }
            //int pwStrength = 0;

            //if (Regex.IsMatch(password, @"[\!@#\$%\^&*?_~]"))
            //    pwStrength++;

            //if (Regex.IsMatch(password, @"[a-z]"))
            //    pwStrength++;

            //if (Regex.IsMatch(password, @"[A-Z]"))
            //    pwStrength++;

            //if (Regex.IsMatch(password, @"[0-9]"))
            //    pwStrength++;

            //if (pwStrength >= 3)
                return null;

            //return
            //    new Return<PraticienResultDataModel>()
            //    .Error()
            //    .AsValidationFailure(null,
            //        "Pour améliorer la résistance du mot de passe, vous devez utiliser des chiffres, " +
            //        "caractères en majuscule et miniscule, " +
            //        "et des caractères spéciaux comme : !,@,#,$,%,^,&,*,?,_,~",
            //    "Mot de passe")
            //    .WithDefaultResult();
        }


        public ResultOfType<PraticienResultDataModel> GetPraticienByEmail(string email)
        {
            try
            {
               
                Logger.LogInfo(string.Format("GetPraticienByEmail : Start --- Email = {0} ", email ));

                var list = _praticienRepository.GetAll();
                if (list != null)
                {
                    var praticiens = list as Praticien[] ?? list.ToArray();
                    var praticien = praticiens.FirstOrDefault(p => p.Email.Equals(email));
                    
                
                    if (praticien != null)
                    {
                        var dataModel = PraticienWrapper.ConvertPraticienEntityToDataModel(praticien);
                        return new Return<PraticienResultDataModel>().OK().WithResult(dataModel);
                    }
                    else
                    {
                        return new Return<PraticienResultDataModel>().Error().As(EStatusDetail.NotFound).AddingGenericError(
                null, "Pas de praticien enregistré actuellement !!").WithDefaultResult();
                    }

                    
                }
                else
                {
                    return new Return<PraticienResultDataModel>().Error().As(EStatusDetail.NotFound).AddingGenericError(
                   null, "Pas de praticien enregistré actuellement !!").WithDefaultResult();
                }

            }
            catch (Exception ex)
            {
                return new Return<PraticienResultDataModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                   null, "Veuillez contacter AlloTabib :" + ex.Message).WithDefaultResult();
            }
        }

        public ResultOfType<PraticienResultDataModel> GetPraticienByNomPrenom(string nomPrenom)
        {
            try
            {

                Logger.LogInfo(string.Format("GetPraticienByEmail : Start --- Email = {0} ", nomPrenom));

                var list = _praticienRepository.GetAll();
                if (list != null)
                {
                    var praticiens = list as Praticien[] ?? list.ToArray();
                    var praticien = praticiens.FirstOrDefault(p => p.NomPrenom.Trim().ToLower().Equals(nomPrenom.Trim().ToLower()));


                    if (praticien != null)
                    {
                        var dataModel = PraticienWrapper.ConvertPraticienEntityToDataModel(praticien);
                        return new Return<PraticienResultDataModel>().OK().WithResult(dataModel);
                    }
                    else
                    {
                        return new Return<PraticienResultDataModel>().Error().As(EStatusDetail.NotFound).AddingGenericError(
                null, "Pas de praticien enregistré actuellement !!").WithDefaultResult();
                    }


                }
                else
                {
                    return new Return<PraticienResultDataModel>().Error().As(EStatusDetail.NotFound).AddingGenericError(
                   null, "Pas de praticien enregistré actuellement !!").WithDefaultResult();
                }

            }
            catch (Exception ex)
            {
                return new Return<PraticienResultDataModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                   null, "Veuillez contacter AlloTabib :" + ex.Message).WithDefaultResult();
            }
        }

        public ResultOfType<PraticienResultModel> GetPraticiensOpenSearch(string q, int take = 0, int skip = 0)
        {
            try
            {
               
                Logger.LogInfo("GetPraticiensOpenSearch : Start.");

                if (string.IsNullOrWhiteSpace(q))
                    return new Return<PraticienResultModel>()
                        .Error()
                        .AsValidationFailure(null, "Vous n'avez rien introduit pour faire la recherche.", "q")
                        .WithDefaultResult();
                q = Helpers.Common.ValidateQueryString(q);
                var result = _searchEngine.Search(q);


                var data = new PraticienResultModel();
                var totalCount = result.Count();
                var totalPages = (take != 0) ? (int)Math.Ceiling((double)totalCount / take) : 0;

                var paginationHeader = new PaginationHeader
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages
                };

                if (take != 0 || skip != 0)
                {
                    result = result.OrderBy(r => r.Cin).Skip(skip).Take(take).ToList();
                }

                result.ForEach(praticien =>
                {
                    string logLat = "";

                    var praticienData = new PraticienResultDataModel
                    {
                        Cin = praticien.Cin ?? "",
                        Adresse = praticien.Adresse ?? "",
                        Conventionne = praticien.Conventionne ?? "",
                        Cursus = praticien.Cursus ?? "",
                        Delegation = praticien.Delegation ?? "",
                        Diplomes = praticien.Diplomes ?? "",
                        Email = praticien.Email ?? "",
                        EstActive = praticien.EstActive ?? "",
                        Fax = praticien.Fax ?? "",
                        Formations = praticien.Formations ?? "",
                        Gouvernerat = praticien.Gouvernerat ?? "",
                        InformationsPratique = praticien.InformationsPratique ?? "",
                        LanguesParles = praticien.LanguesParles ?? "",
                        MoyensPaiement = praticien.MoyensPaiement ?? "",
                        NomPrenom = praticien.NomPrenom ?? "",
                        ParcoursHospitalier = praticien.ParcoursHospitalier ?? "",
                        Password = praticien.Password ?? "",
                        PresentationCabinet = praticien.PresentationCabinet ?? "",
                        PrixConsultation = praticien.PrixConsultation ?? "",
                        Publication = praticien.Publication ?? "",
                        ReseauxSociaux = praticien.ReseauxSociaux ?? "",
                        Telephone = praticien.Telephone ?? "",
                        Specialite = praticien.Specialite,
                        

                    };

                    data.Items.Add(praticienData);
                    data.PaginationHeader = paginationHeader;
                });
                Logger.LogInfo(string.Format("GetPraticiensOpenSearch : End ."));

                return new Return<PraticienResultModel>().OK().WithResult(data);
            }
            catch (Exception ex)
            {
                Logger.LogInfo(string.Format("GetPraticiensOpenSearch : End with Error --- : {0}", ex.Message));
                throw;
            }
        }

        public ResultOfType<PraticienResultModel> SearchForPraticien(string gouvernerat, string specialite,
                    string nomPraticien, int take = 0, int skip = 0)
        {
            try
            {
                
                Logger.LogInfo("Search For Praticien With Take And Skip Parameters : Start." );
                var praticiens = (take == 0 && skip == 0)
                                 ? _praticienRepository
                                      .GetAll()
                                      .OrderBy(a => a.CreationDate)
                                      .ToList()
                                 : _praticienRepository
                                      .GetAll()
                                      .OrderBy(a => a.CreationDate)
                                      .Skip(skip)
                                      .Take(take)
                                      .ToList();

                if (string.IsNullOrEmpty(gouvernerat) && string.IsNullOrEmpty(specialite) &&
                    string.IsNullOrEmpty(nomPraticien))
                {
                    //Get aall
                    return GetPraticiens(take, skip);
                }
                else
                {
                    if (!string.IsNullOrEmpty(gouvernerat) && !gouvernerat.Equals("undefined"))
                    {
                        praticiens = praticiens.Where(p => p.Gouvernerat.Trim().ToLower().Equals(gouvernerat.Trim().ToLower())).ToList();
                    }
                    if (!string.IsNullOrEmpty(specialite) && !specialite.Equals("undefined"))
                    {
                        praticiens = praticiens.Where(p => p.Specialite.Trim().ToLower().Equals(specialite.Trim().ToLower())).ToList();
                    }
                    if (!string.IsNullOrEmpty(nomPraticien) && !nomPraticien.Equals("undefined"))
                    {
                        praticiens = praticiens.Where(p => p.NomPrenom.Trim().ToLower().Contains(nomPraticien.Trim().ToLower())).ToList();
                    }

                    var totalCount = praticiens.Count;
                    var totalPages = (take != 0) ? (int)Math.Ceiling((double)totalCount / take) : 0;

                    var paginationHeader = new PaginationHeader
                    {
                        TotalCount = totalCount,
                        TotalPages = totalPages
                    };
                    var data = new PraticienResultModel();

                    praticiens.ForEach(praticien =>
                    {
                        var dataModel = PraticienWrapper.ConvertPraticienEntityToDataModel(praticien);
                        data.Items.Add(dataModel);
                    });

                    data.PaginationHeader = paginationHeader;

                    Logger.LogInfo("Get Praticiens With Take And Skip Parameters : End --- Status : OK");
                    return new Return<PraticienResultModel>().OK().WithResult(data);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ResultOfType<IList<SpecialiteGouverneratModel>> GetListSpecialiteGouvernerat()
        {
            try
            {
               
                Logger.LogInfo("GetListSpecialiteGouvernerat : Start.");
                var praticiensSpecialite = _praticienRepository
                                     .GetAll()
                                     .Select(m => new {m.Specialite, m.Gouvernerat})
                                     .Distinct()
                                     .ToList();
                                    

              
                var dataList = new List<SpecialiteGouverneratModel>();
                praticiensSpecialite.ForEach(praticien =>
                {
                    var data = new SpecialiteGouverneratModel
                    {
                        Specialite = praticien.Specialite,
                        Gouvernerat = praticien.Gouvernerat
                    };
                    dataList.Add(data);
                });
                Logger.LogInfo("GetListSpecialiteGouvernerat : End.");
                return new Return<IList<SpecialiteGouverneratModel>>().OK().WithResult(dataList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ResultOfType<RendezVousResultModel> GetListOfRendezVousTousEnCours(string praticienEmail)
        {
            try
            {
             
                Logger.LogInfo("Get List Of Rendez Vous Tous En Cours With Take And Skip Parameters : Start." );
                if(string.IsNullOrEmpty(praticienEmail))
                    return new Return<RendezVousResultModel>().Error().As(EStatusDetail.NotFound).AddingGenericError(
              null, "L'email introduit est vide!!").WithDefaultResult();

                var rendezvous = _rendezVousRepository.GetAll().Where(p => p.Praticien.Email.Equals(praticienEmail));

                var data = new RendezVousResultModel();
                IFormatProvider culture = new System.Globalization.CultureInfo("fr-FR", true);

                rendezvous.ForEach(rdv =>
                {
                    DateTime dt2 = DateTime.Parse(rdv.Creneaux.CurrentDate, culture,
                            System.Globalization.DateTimeStyles.AssumeLocal);
                    int result = DateTime.Compare(dt2, DateTime.Now);
                    if (result > 0)
                    {
                        var dataModel = RendezVousWrapper.ConvertPatientEntityToDataModel(rdv);
                        data.Items.Add(dataModel);
                    }                 
                });


                Logger.LogInfo("Get List Of Rendez Vous Tous En Cours : End --- Status : OK");
                return new Return<RendezVousResultModel>().OK().WithResult(data);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
