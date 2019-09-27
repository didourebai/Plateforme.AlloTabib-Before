using System;
using System.Globalization;
using System.Linq;
using Plateforme.AlloTabib.CrossCuttingLayer.Logging;
using Plateforme.AlloTabib.DomainLayer.Entities;
using Plateforme.AlloTabib.DomainLayer.Helpers;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;
using PlateformeAlloTabib.Standards.Helpers;
using System.Collections.Generic;
using Plateforme.AlloTabib.DomainLayer.Base.Interfaces;

namespace Plateforme.AlloTabib.DomainLayer.DomainServices.CreneauDomainServices
{
    public class CreneauDomainServices : ICreneauDomainServices
    {

        private readonly IRepository<Creneaux> _creneauxRepository;
        private readonly IRepository<Praticien> _praticienRepository;
        private readonly IRepository<ConfigurationPraticien> _configurationPraticienRepository;
      
        public CreneauDomainServices(IRepository<ConfigurationPraticien> configurationPraticienRepository, IRepository<Creneaux> creneauxRepository, IRepository<Praticien> praticienRepository)
        {
            if (creneauxRepository == null)
                throw new ArgumentNullException("creneauxRepository");
            _creneauxRepository = creneauxRepository;
            if (praticienRepository == null)
                throw new ArgumentNullException("praticienRepository");
            _praticienRepository = praticienRepository;

            if (configurationPraticienRepository == null)
                throw new ArgumentNullException("configurationPraticienRepository");
            _configurationPraticienRepository = configurationPraticienRepository;
          
        }

        private string AddZero(string h)
        {
            if (h.Length == 1)
            {
                h = string.Concat("0", h);

            }
            return h;
        }

        private string DeleteZero(string hh)
        {
            if (hh.Length == 2)
            {
                var h = hh.Substring(0, 1);
                if (h.Equals('0'.ToString(CultureInfo.InvariantCulture)))
                    return hh.Substring(1, 1);
                else
                {
                    return hh;
                }
            }

            return hh;
        }

        public ResultOfType<Null> PostCreneauxJour(string from, string to, string cinPraticien,string jour)
        {
            try
            {
                if (string.IsNullOrEmpty(from))

                    return new Return<Null>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                        null, "heure de début null.").WithDefaultResult();

                if (string.IsNullOrEmpty(to))
                    return new Return<Null>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                        null, "heure de fin null.").WithDefaultResult();

                if (string.IsNullOrEmpty(cinPraticien))
                    return new Return<Null>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                        null, "Cin praticien vide.").WithDefaultResult();

                if (string.IsNullOrEmpty(jour))
                    return new Return<Null>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                        null, "jour.").WithDefaultResult();

                Praticien praticien = _praticienRepository.GetAll().FirstOrDefault(p => p.Cin.Equals(cinPraticien));

                if (praticien == null)
                    return new Return<Null>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                        null, "praticien est null.").WithDefaultResult();

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

                var lstSundays = new List<string>();
                int intYear = DateTime.Now.Year;
                int intMonth = DateTime.Now.Month;
                for (int i = intMonth; i < 13; i++)
                {
                    var list = PrintHolidays(intYear, i, praticien, dayOfWeek).ToList();
                    lstSundays.AddRange(list);
                }

                foreach (var lstSunday in lstSundays)
                {
                    PostCreneaux(from, to, cinPraticien, lstSunday);

                }

                return
                    new Return<Null>().OK().WithDefaultResult();
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public IList<string> PrintHolidays(int intYear, int intMonth, Praticien praticien, DayOfWeek dayOfWeek)
        {
            var lstSundays = new List<string>();

            int intDaysThisMonth = DateTime.DaysInMonth(intYear, intMonth);
           
            var conditionDateTime = new DateTime(intYear, intMonth, intDaysThisMonth);

            for (var dt1 = new DateTime(intYear, intMonth, 1); dt1 <= conditionDateTime; dt1 = dt1.AddDays(1))
            {
                if (dt1.DayOfWeek == dayOfWeek)
                {
                    string day = dt1.ToShortDateString();

                    lstSundays.Add(day);
                }
            }

            return lstSundays;
        }


        public ResultOfType<Null> PostCreneaux(string from, string to, string cinPraticien, string dateSelected)
        {
            try
            {
                if (string.IsNullOrEmpty(from))

                    return new Return<Null>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                        null, "heure de début null.").WithDefaultResult();

                if (string.IsNullOrEmpty(to))
                    return new Return<Null>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                        null, "heure de fin null.").WithDefaultResult();

                if (string.IsNullOrEmpty(cinPraticien))
                    return new Return<Null>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                        null, "Cin praticien vide.").WithDefaultResult();


                if (string.IsNullOrEmpty(dateSelected))
                    return new Return<Null>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                        null, "Date courante non définit.").WithDefaultResult();

                //Récupérer la configuration pour faire un décalage
                //var startHeure, endHeure;
                ConfigurationPraticien configurationPraticien =
                    _configurationPraticienRepository.GetAll().FirstOrDefault(c => c.Praticien.Cin == cinPraticien);

                if (configurationPraticien == null)
                    return new Return<Null>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                        null, "configurationPraticien est null.").WithDefaultResult();
                Praticien praticien = _praticienRepository.GetAll().FirstOrDefault(p => p.Cin.Equals(cinPraticien));

                if (praticien == null)
                    return new Return<Null>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                        null, "praticien est null.").WithDefaultResult();

                int heureFrom = 0;
                int minuteFrom = 0;

                int heureTo = 0;
                int minuteTo = 0;

                var splitFrom = from.Split(Convert.ToChar(":"));
                if (splitFrom.Count() == 2)
                {
                    heureFrom = int.Parse(DeleteZero(splitFrom[0]));
                    minuteFrom = int.Parse(DeleteZero(splitFrom[1]));
                }

                var splitTo = to.Split(Convert.ToChar(":"));
                if (splitTo.Count() == 2)
                {
                    heureTo = int.Parse(DeleteZero(splitTo[0]));
                    minuteTo = int.Parse(DeleteZero(splitTo[1]));
                }

                bool isTheEnd = false;
                string startTime = from;
                IList<CreneauDataModel> creneauDataModelList = new List<CreneauDataModel>();
                while (!(isTheEnd))
                {
                    isTheEnd = (heureFrom == heureTo && minuteFrom >= minuteTo);
                    string dateDebut = startTime;

                    minuteFrom = minuteFrom + configurationPraticien.DecalageMinute;
                    if (minuteFrom >= 60)
                    {
                        minuteFrom = minuteFrom - 60;
                        heureFrom = heureFrom + 1;
                    }

                    startTime = string.Concat(AddZero(heureFrom.ToString(CultureInfo.InvariantCulture)), ":",
                        AddZero(minuteFrom.ToString(CultureInfo.InvariantCulture)));

                  
                    var creneau = new CreneauDataModel
                    {
                        
                        PraticienEmail = praticien.Email,
                        CurrentDate = dateSelected,
                        HeureDebut = dateDebut,
                        HeureFin = startTime,
                        Status = "ND"
                        
                    };
                    
                   creneauDataModelList.Add(creneau);
                }

                PostNewCreneau(creneauDataModelList);

                return
                    new Return<Null>().OK().WithDefaultResult();
            }

            catch (Exception ex)
            {
                throw;
            }

        }
        public ResultOfType<IList<CreneauResultDataModel>> PostNewCreneau(IList<CreneauDataModel> creneaux)
        {

            if (creneaux == null)
                return new Return<IList<CreneauResultDataModel>>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                    null, "Les données sont vides.").WithDefaultResult();
            Logger.LogInfo(string.Format("Post New Creneau : Start --- "));



            if (creneaux.Count() > 0)
            {

                IList<CreneauResultDataModel> data = new List<CreneauResultDataModel>();

                foreach (CreneauDataModel creneauToAdd in creneaux)
                {
                    var validationResult = ValidateNewCreneauProperties(creneauToAdd);

                    if (validationResult != null)
                    {
                        Logger.LogInfo(string.Format("Post New Creneau : End --- Status = {0}, Message= {1}",
                                                     validationResult.Status, validationResult.Errors[0].Message));
                        return validationResult;
                    }

                    Praticien praticien = _praticienRepository.GetAll().FirstOrDefault(p => p.Email.Equals(creneauToAdd.PraticienEmail));
                    if (praticien == null)
                        return new Return<IList<CreneauResultDataModel>>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                           null, "aucun praticien est lié à ce créneau !!!.").WithDefaultResult();

                    CreneauResultDataModel cre = new CreneauResultDataModel();
                    data.Add(cre);

                    var creneau = new Creneaux
                    {
                        HeureDebut = creneauToAdd.HeureDebut,
                        HeureFin = creneauToAdd.HeureFin,
                        Praticien = praticien,
                        Status = creneauToAdd.Status,
                        CurrentDate = creneauToAdd.CurrentDate,
                        Commentaire = creneauToAdd.Commentaire
                    };

                    _creneauxRepository.Add(creneau);
                }
                return
           new Return<IList<CreneauResultDataModel>>().OK()
               .WithResult(data);
            }

            var cren = new List<CreneauResultDataModel>();
            return
           new Return<IList<CreneauResultDataModel>>().OK()
               .WithResult(cren);

        }

        //public ResultOfType<bool> CreneauHasRendezVous(string heureDebut, string heureFin, string currentDate, string emailPraticien)
        //{
        //    Logger.LogInfo("Get Creneaux With Take And Skip Parameters : Start.");
        //    //Récupérer par praticien , heure début et date donné
        //    var creneau =
        //        _creneauxRepository.GetAll()
        //            .FirstOrDefault(p => p.Praticien.Cin.Equals(praticien) && p.HeureDebut.Equals(heureDebut) && p.CurrentDate.Equals(currentDate));
        //}

        public ResultOfType<CreneauResultModel> GetCreneauxByPraticien(string email, int take = 0, int skip = 0)
        {
            try
            {
                Logger.LogInfo("Get Creneaux With Take And Skip Parameters : Start.");

                var totalCount = _praticienRepository.GetCount();
                var totalPages = (take != 0) ? (int)Math.Ceiling((double)totalCount / take) : 0;

                var paginationHeader = new PaginationHeader
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages
                };

                var creneaux = (take == 0 && skip == 0)
                    ? _creneauxRepository
                        .GetAll().Where(p => p.Praticien.Email == email)
                        .ToList()
                    : _creneauxRepository
                        .GetAll().Where(p => p.Praticien.Email == email)
                        .Skip(skip)
                        .Take(take)
                        .ToList();
                var data = new CreneauResultModel();
                creneaux.ForEach(creneau =>
                {
                    var dataModel = CreneauWrapper.ConvertCreneauEntityToDataModel(creneau);
                    data.Items.Add(dataModel);
                });

                data.PaginationHeader = paginationHeader;

                Logger.LogInfo("Get Creneaux by praticien With Take And Skip Parameters : End --- Status : OK");
                return new Return<CreneauResultModel>().OK().WithResult(data);
            }
            catch (Exception exception)
            {
                Logger.LogError("Get Praticiens Exception", exception);
                throw;
            }
        }

        public ResultOfType<CreneauResultDataModel> GetCreneauByHeureDebutAndDate(string heureDebut, string praticien, string currentDate)
        {
            try
            {
                Logger.LogInfo("Get Creneaux With Take And Skip Parameters : Start.");
                //Récupérer par praticien , heure début et date donné
                var creneau =
                    _creneauxRepository.GetAll()
                        .FirstOrDefault(p => p.Praticien.Email.Equals(praticien) && p.HeureDebut.Equals(heureDebut) && p.CurrentDate.Equals(currentDate));

                //Pas de créneau
                if (creneau == null)
                {
                    return new Return<CreneauResultDataModel>()
                          .Error()
                          .AsNotFound()
                          .WithDefaultResult();
                }

                var dataModel = CreneauWrapper.ConvertCreneauEntityToDataModel(creneau);

                Logger.LogInfo("Get Creneaux by praticien and date Parameters : End --- Status : OK");
                return new Return<CreneauResultDataModel>().OK().WithResult(dataModel);
            }
            catch (Exception exception)
            {
                Logger.LogError("Get Praticiens Exception", exception);
                throw;
            }
        }

        private ResultOfType<IList<CreneauResultDataModel>> ValidateNewCreneauProperties(CreneauDataModel creneauToAdd)
        {
            if (string.IsNullOrEmpty(creneauToAdd.HeureDebut))
                return new Return<IList<CreneauResultDataModel>>()
                  .Error().AsValidationFailure(null, "Veuillez introduire votre HeureDebut.", "HeureDebut")
                  .WithDefaultResult();

            if (string.IsNullOrEmpty(creneauToAdd.HeureFin))
                return new Return<IList<CreneauResultDataModel>>()
                  .Error().AsValidationFailure(null, "Veuillez introduire votre HeureFin.", "HeureFin")
                  .WithDefaultResult();

            if (string.IsNullOrEmpty(creneauToAdd.PraticienEmail))
                return new Return<IList<CreneauResultDataModel>>()
                 .Error().AsValidationFailure(null, "Veuillez introduire votre Praticien Cin.", "PraticienCin")
                 .WithDefaultResult();

            if (string.IsNullOrEmpty(creneauToAdd.Status))
                return new Return<IList<CreneauResultDataModel>>()
                .Error().AsValidationFailure(null, "Veuillez introduire votre Status.", "Status")
                .WithDefaultResult();
            if (string.IsNullOrEmpty(creneauToAdd.CurrentDate))
                return new Return<IList<CreneauResultDataModel>>()
                .Error().AsValidationFailure(null, "Veuillez introduire votre currentDate.", "CurrentDate")
                .WithDefaultResult();

            return null;
        }

        private ResultOfType<CreneauResultDataModel> ValidateUpdateCreneauProperties(CreneauDataModel creneauToAdd)
        {
            if (string.IsNullOrEmpty(creneauToAdd.HeureDebut))
                return new Return<CreneauResultDataModel>()
                  .Error().AsValidationFailure(null, "Veuillez introduire votre HeureDebut.", "HeureDebut")
                  .WithDefaultResult();

            if (string.IsNullOrEmpty(creneauToAdd.HeureFin))
                return new Return<CreneauResultDataModel>()
                  .Error().AsValidationFailure(null, "Veuillez introduire votre HeureFin.", "HeureFin")
                  .WithDefaultResult();

            if (string.IsNullOrEmpty(creneauToAdd.PraticienEmail))
                return new Return<CreneauResultDataModel>()
                 .Error().AsValidationFailure(null, "Veuillez introduire votre Praticien Cin.", "PraticienCin")
                 .WithDefaultResult();

            if (string.IsNullOrEmpty(creneauToAdd.Status))
                return new Return<CreneauResultDataModel>()
                .Error().AsValidationFailure(null, "Veuillez introduire votre Status.", "Status")
                .WithDefaultResult();
            if (string.IsNullOrEmpty(creneauToAdd.CurrentDate))
                return new Return<CreneauResultDataModel>()
                .Error().AsValidationFailure(null, "Veuillez introduire votre currentDate.", "CurrentDate")
                .WithDefaultResult();

            return null;
        }

        public ResultOfType<Null> DeleteCreneau(string praticien, string dateCurrent, string heureDebut)
        {
            try
            {
                if (string.IsNullOrEmpty(praticien))
                    if (string.IsNullOrEmpty(heureDebut))
                        return new Return<Null>()
                          .Error().AsValidationFailure(null, "Veuillez introduire votre HeureDebut.", "HeureDebut")
                          .WithDefaultResult();

                if (string.IsNullOrEmpty(praticien))
                    return new Return<Null>()
                     .Error().AsValidationFailure(null, "Veuillez introduire votre Praticien Cin.", "Praticien")
                     .WithDefaultResult();

                if (string.IsNullOrEmpty(dateCurrent))
                    return new Return<Null>()
                    .Error().AsValidationFailure(null, "Veuillez introduire votre currentDate.", "DateCurrent")
                    .WithDefaultResult();

                var creneauResult =
                    _creneauxRepository.GetAll()
                        .FirstOrDefault(
                            c =>
                                c.CurrentDate.Equals(dateCurrent) && c.HeureDebut.Equals(heureDebut) &&
                                c.Praticien.Cin.Equals(praticien));
                if (creneauResult != null)
                {
                    _creneauxRepository.Delete(creneauResult);
                    return new Return<Null>().OK().WithDefaultResult();
                }

                return new Return<Null>().Error().AsNotFound().WithDefaultResult();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ResultOfType<CreneauResultDataModel> UpdateCreneauPraticienByDate(string praticien, string dateCurrent, CreneauDataModel creneau)
        {
            try
            {
                if (string.IsNullOrEmpty(praticien))
                    return new Return<CreneauResultDataModel>()
                     .Error().AsValidationFailure(null, "Veuillez introduire votre Praticien Cin.", "Praticien")
                     .WithDefaultResult();

                if (string.IsNullOrEmpty(dateCurrent))
                    return new Return<CreneauResultDataModel>()
                    .Error().AsValidationFailure(null, "Veuillez introduire votre currentDate.", "DateCurrent")
                    .WithDefaultResult();
                if (creneau != null)
                {
                    var validationResult = ValidateUpdateCreneauProperties(creneau);
                    if (validationResult == null)
                    {
                        var creneauResult =
                            _creneauxRepository.GetAll()
                                .FirstOrDefault(
                                    c =>
                                        c.CurrentDate.Equals(dateCurrent) && c.Praticien.Cin.Equals(praticien));

                        if (creneauResult != null)
                        {
                            creneauResult.CurrentDate = creneau.CurrentDate;
                            creneauResult.HeureDebut = creneau.HeureDebut;
                            creneauResult.Status = creneau.Status;
                            //get praticien
                            var thePraticien = _praticienRepository.GetAll().FirstOrDefault(p => p.Cin == praticien);
                            if (thePraticien != null)
                                creneauResult.Praticien = thePraticien;
                            creneauResult.HeureFin = creneau.HeureFin;

                            _creneauxRepository.Update(creneauResult);
                            var dataModel = CreneauWrapper.ConvertCreneauEntityToDataModel(creneauResult);
                            return new Return<CreneauResultDataModel>().OK().WithResult(dataModel);
                        }
                        else
                        {
                            return
                                new Return<CreneauResultDataModel>().OK()
                                    .WithResult(new CreneauResultDataModel
                                    {
                                        CurrentDate = creneau.CurrentDate,
                                        PraticienCin = creneau.PraticienEmail,
                                        HeureFin = creneau.HeureFin,
                                        Status = creneau.Status,
                                        HeureDebut = creneau.HeureDebut,
                                        Commentaire = creneau.Commentaire
                                    });
                        }

                    }

                    return validationResult;
                }
                else
                {
                   
                    return
                               new Return<CreneauResultDataModel>().OK()
                                   .WithResult(new CreneauResultDataModel
                                   {
                                       CurrentDate = creneau.CurrentDate,
                                       PraticienCin = creneau.PraticienEmail,
                                       HeureFin = creneau.HeureFin,
                                       Status = creneau.Status,
                                       HeureDebut = creneau.HeureDebut,
                                       Commentaire = creneau.Commentaire
                                   });
                }


            }
            catch (Exception ex)
            {

                throw;
            }
        }


    }
}