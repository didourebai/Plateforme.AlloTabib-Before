using System;
using System.Collections.Generic;
using System.Linq;
using Plateforme.AlloTabib.CrossCuttingLayer.Logging;
using Plateforme.AlloTabib.DomainLayer.Base.Interfaces;
using Plateforme.AlloTabib.DomainLayer.Entities;
using Plateforme.AlloTabib.DomainLayer.Helpers;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;
using PlateformeAlloTabib.Standards.Helpers;

namespace Plateforme.AlloTabib.DomainLayer.DomainServices.ConfigurationDomainServices
{
    public class ConfigurationDomainServices : IConfigurationDomainServices
    {
        #region Properties 

        private readonly IRepository<ConfigurationPraticien> _configurationPraticienRepository;
        private  readonly  IRepository<Praticien>  _praticienRepository;
        private readonly IRepository<JourFerie> _jourFerieRepository;
       #endregion

        #region Constructeur

        public ConfigurationDomainServices(IRepository<JourFerie> jourFerieRepository,IRepository<ConfigurationPraticien> configurationPraticienRepository, IRepository<Praticien> praticienRepository)
        {
            if (configurationPraticienRepository == null)
                throw new ArgumentNullException("configurationPraticienRepository");
            _configurationPraticienRepository = configurationPraticienRepository;
            if (praticienRepository == null)
                throw new ArgumentNullException("praticienRepository");
            _praticienRepository = praticienRepository;
            if (jourFerieRepository == null)
                throw new ArgumentNullException("jourFerieRepository");
            _jourFerieRepository = jourFerieRepository;
         
        }
        
        #endregion

        /// <summary>
        /// Ajouter une nouvelle configuration
        /// </summary>
        /// <param name="configurationToAdd"></param>
        /// <returns></returns>
        public ResultOfType<ConfigurationResultDataModel> PostNewConfiguration(ConfigurationDataModel configurationToAdd)
        {
            try
            {
               
                Logger.LogInfo("PostNewConfiguration : Start " );
                if (configurationToAdd == null)
                {
                    return
                        new Return<ConfigurationResultDataModel>().Error()
                            .As(EStatusDetail.BadRequest)
                            .AddingGenericError(
                                null, "Les données sont vides.").WithDefaultResult();
                }
                var validationResult = ValidateNewConfigProperties(configurationToAdd);
                if (validationResult != null)
                {
                    return validationResult;
                }

                var praticien = _praticienRepository.Get(configurationToAdd.PraticienCin);
                Logger.LogInfo("PostNewConfiguration : End." );

                var configurationPraticien = new ConfigurationPraticien
                {
                    Praticien = praticien,
                  
                    HeureDebutMatin = configurationToAdd.HeureDebutMatin,
                    HeureDebutMidi = configurationToAdd.HeureDebutMidi,
                    
                    MinuteDebutMatin = configurationToAdd.MinuteDebutMatin,
                    MinuteDebutMidi = configurationToAdd.MinuteDebutMidi,
                    HeureFinMatin = configurationToAdd.HeureFinMatin,
                    HeureFinMidi = configurationToAdd.HeureFinMidi,
                    DecalageMinute = int.Parse(configurationToAdd.DecalageMinute),
                    MinuteFinMatin = configurationToAdd.MinuteFinMatin,
                    MinuteFinMidi = configurationToAdd.MinuteFinMidi
                };

                _configurationPraticienRepository.Add(configurationPraticien);
                return new Return<ConfigurationResultDataModel>().OK().WithResult(new ConfigurationResultDataModel
                {
                  
                    HeureDebutMatin = configurationPraticien.HeureDebutMatin,
                    HeureDebutMidi = configurationPraticien.HeureDebutMidi,
                    
                    MinuteDebutMatin = configurationPraticien.MinuteDebutMatin,
                    MinuteDebutMidi = configurationPraticien.MinuteDebutMidi,
                    PraticienCin = configurationPraticien.Praticien != null ? configurationPraticien.Praticien.Cin : "",
                    DecalageMinute = configurationToAdd.DecalageMinute,
                    HeureFinMatin = configurationToAdd.HeureFinMatin,
                    HeureFinMidi = configurationToAdd.HeureFinMidi,
                    MinuteFinMatin = configurationToAdd.MinuteFinMatin,
                    MinuteFinMidi = configurationToAdd.MinuteFinMidi
                });

            }
            catch (Exception ex)
            {
                return new Return<ConfigurationResultDataModel>()
                    .Error().AsGenericError().AddingException(ex)
                    .WithResult(null);

            }

        }

        //Ajouter les dimanches lors de la première configuration, pareil pour samedi.



        private ResultOfType<ConfigurationResultDataModel> ValidateNewConfigProperties(ConfigurationDataModel configurationToAdd)
        {
            //if(string.IsNullOrEmpty(configurationToAdd.HeureDebutMatin))
            //    return new Return<ConfigurationResultDataModel>()
            //      .Error().AsValidationFailure(null, "Veuillez introduire votre Heure début du matin.", "HeureDebutMatin")
            //      .WithDefaultResult();
            //if(string.IsNullOrEmpty(configurationToAdd.MinuteDebutMatin))
            //    return new Return<ConfigurationResultDataModel>()
            //     .Error().AsValidationFailure(null, "Veuillez introduire votre minute début du matin.", "MinuteDebutMatin")
            //     .WithDefaultResult();
            //if (string.IsNullOrEmpty(configurationToAdd.HeureDebutMidi))
            //    return new Return<ConfigurationResultDataModel>()
            //     .Error().AsValidationFailure(null, "Veuillez introduire votre Heure début du midi.", "HeureDebutMidi")
            //     .WithDefaultResult();

            //if (string.IsNullOrEmpty(configurationToAdd.MinuteDebutMidi))
            //    return new Return<ConfigurationResultDataModel>()
            //     .Error().AsValidationFailure(null, "Veuillez introduire votre minute début du midi.", "MinuteDebutMidi")
            //     .WithDefaultResult();
            if (string.IsNullOrEmpty(configurationToAdd.PraticienCin))
                return new Return<ConfigurationResultDataModel>()
                    .Error()
                    .AsValidationFailure(null, "Veuillez introduire votre Praticien ID.", "PraticienCin")
                    .WithDefaultResult();
            if(string.IsNullOrEmpty(configurationToAdd.DecalageMinute))
                return new Return<ConfigurationResultDataModel>()
                .Error().AsValidationFailure(null, "Veuillez introduire votre décalage en minute entre deux consultations.", "DecalageMinute")
                .WithDefaultResult();

            return null;
        }

        public ResultOfType<ConfigurationResultDataModel> GetConfigurationByPraticien(string praticien)
        {
         
            Logger.LogInfo("GetConfigurationByPraticien : Start." );

            if(praticien == null)
                return new Return<ConfigurationResultDataModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                  null, "Les données sont vides.").WithDefaultResult();

            try
            {
                var config = _configurationPraticienRepository.GetAll()
                    .FirstOrDefault(c => c.Praticien.Cin == praticien);
                if(config == null)
                    return new Return<ConfigurationResultDataModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                   null, "Aucune configuration a été mis pour ce docteur").WithDefaultResult();
                var configurationResultDataModel = new ConfigurationResultDataModel
                {
                  
                    HeureDebutMatin = config.HeureDebutMatin,
                    HeureDebutMidi = config.HeureDebutMidi,
                    
                    MinuteDebutMatin = config.MinuteDebutMatin,
                    MinuteDebutMidi = config.MinuteDebutMidi,
                    PraticienCin = config.Praticien != null ? config.Praticien.Cin : "",
                    DecalageMinute = config.DecalageMinute.ToString(),
                    HeureFinMatin = config.HeureFinMatin,
                    HeureFinMidi = config.HeureFinMidi,
                    MinuteFinMatin = config.MinuteFinMatin,
                    MinuteFinMidi = config.MinuteFinMidi
                };
                Logger.LogInfo("GetConfigurationByPraticien : End ");

                return new Return<ConfigurationResultDataModel>().OK().WithResult(configurationResultDataModel);
            }
            catch (Exception ex)
            {
                return new Return<ConfigurationResultDataModel>()
                 .Error().AsGenericError().AddingException(ex)
                   .WithResult(null);
            }

        }

        public ResultOfType<IList<string>> AjouterDimancheFerie(string cinPraticien)
        {
           
            Logger.LogInfo("AjouterDimancheFerie : Start. " );
            try
            {
                //Praticien
                var praticien = _praticienRepository.GetAll().FirstOrDefault(p => p.Cin.Equals(cinPraticien));
                if(praticien == null)

                    return
                      new Return<IList<string>>().Error()
                          .As(EStatusDetail.BadRequest)
                          .AddingGenericError(
                              null, "Praticin CIN non existant").WithDefaultResult();
                var lstSundays = new List<string>();
                int anneeCourant = DateTime.Now.Year;

                for (int i = 1; i < 13; i++)
                {
                    lstSundays = PrintHolidays(anneeCourant, i, praticien, DayOfWeek.Sunday).ToList().Select(x => x.JourFerieNom).ToList();
                }
                Logger.LogInfo("AjouterDimancheFerie : End.");
                return new Return<IList<string>>().OK().WithResult(lstSundays);

            }
            catch (Exception ex)
            {
                return new Return<IList<string>>()
                   .Error().AsGenericError().AddingException(ex)
                   .WithResult(null);
            }
        }

        public ResultOfType<IList<string>> AjouterSamediFerie(string cinPraticien)
        {
          
            Logger.LogInfo("AjouterSamediFerie : Start.");
            try
            {
                //Praticien
                var praticien = _praticienRepository.GetAll().FirstOrDefault(p => p.Cin.Equals(cinPraticien));
                if (praticien == null)

                    return
                      new Return<IList<string>>().Error()
                          .As(EStatusDetail.BadRequest)
                          .AddingGenericError(
                              null, "Praticin CIN non existant").WithDefaultResult();
                var lstSundays = new List<string>();
                int anneeCourant = DateTime.Now.Year;

                for (int i = 1; i < 13; i++)
                {
                    lstSundays = PrintHolidays(anneeCourant, i, praticien, DayOfWeek.Saturday).ToList().Select(x => x.JourFerieNom).ToList();
                }
                Logger.LogInfo("AjouterDimancheFerie : End." );
                return new Return<IList<string>>().OK().WithResult(lstSundays);

            }
            catch (Exception ex)
            {
                return new Return<IList<string>>()
                   .Error().AsGenericError().AddingException(ex)
                   .WithResult(null);
            }
        }

        public ResultOfType<IList<string>> AjouterFerie(string cinPraticien,string jour)
        {
            try
            {
               
                Logger.LogInfo("AjouterFerie : Start. ");
                if(string.IsNullOrEmpty(cinPraticien))
                    return
                     new Return<IList<string>>().Error()
                         .As(EStatusDetail.BadRequest)
                         .AddingGenericError(
                             null, "Praticin CIN vide").WithDefaultResult();

                if (string.IsNullOrEmpty(jour))
                    return
                     new Return<IList<string>>().Error()
                         .As(EStatusDetail.BadRequest)
                         .AddingGenericError(
                             null, "Jour vide").WithDefaultResult();

                var dayOfWeek = new DayOfWeek();

                switch (jour)
                {
                    case "lundi":
                        dayOfWeek = DayOfWeek.Monday;
                        break;
                    case "mardi":
                        dayOfWeek = DayOfWeek.Tuesday;
                        break;
                    case "mercredi":
                        dayOfWeek = DayOfWeek.Wednesday;
                        break;
                    case "jeudi":
                        dayOfWeek = DayOfWeek.Thursday;
                        break;
                    case "vendredi":
                        dayOfWeek = DayOfWeek.Friday;
                        break;
                    case "samedi":
                        dayOfWeek = DayOfWeek.Saturday;
                        break;
                    case "dimanche":
                        dayOfWeek = DayOfWeek.Sunday;
                        break;
                }

                //Praticien
                var praticien = _praticienRepository.GetAll().FirstOrDefault(p => p.Cin.Equals(cinPraticien));
                if (praticien == null)

                    return
                      new Return<IList<string>>().Error()
                          .As(EStatusDetail.BadRequest)
                          .AddingGenericError(
                              null, "Praticin CIN non existant").WithDefaultResult();
                var lstSundays = new List<string>();
                int anneeCourant = DateTime.Now.Year;

                for (int i = 1; i < 13; i++)
                {
                    lstSundays = PrintHolidays(anneeCourant, i, praticien, dayOfWeek).ToList().Select(x => x.JourFerieNom).ToList();
                }

                return new Return<IList<string>>().OK().WithResult(lstSundays);

            }
            catch (Exception ex)
            {
                return new Return<IList<string>>()
                   .Error().AsGenericError().AddingException(ex)
                   .WithResult(null);
            }
        }

        public IList<JourFerie> PrintHolidays(int intYear, int intMonth, Praticien praticien, DayOfWeek dayOfWeek)
        {

            var lstSundays = new List<JourFerie>();

            int intDaysThisMonth = DateTime.DaysInMonth(intYear, intMonth);

            var conditionDateTime = new DateTime(intYear, intMonth, intDaysThisMonth);

            for (var dt1 = new DateTime(intYear, intMonth, 1); dt1 <= conditionDateTime; dt1 = dt1.AddDays(1))
            {
                if (dt1.DayOfWeek == dayOfWeek)
                {
                    string day = dt1.ToShortDateString();
                    var jourferie = new JourFerie
                    {
                        Praticien = praticien,
                        JourFerieNom = day
                    };

                    //vérifier si ce jour existe déjà dans les jours fériés
                    var ferie =
                        _jourFerieRepository.GetAll()
                            .FirstOrDefault(
                                f =>
                                    f.Praticien.Cin.Equals(praticien.Cin) &&
                                    f.JourFerieNom.Equals(jourferie.JourFerieNom));
                    if (ferie == null)
                    {
                        _jourFerieRepository.Add(jourferie);
                        lstSundays.Add(jourferie);
                    }
                }
            }
           
            return lstSundays;
        }

    }
}
