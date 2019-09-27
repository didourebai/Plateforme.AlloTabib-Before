using System;
using System.Collections.Generic;
using System.Linq;
using Plateforme.AlloTabib.CrossCuttingLayer.Logging;
using Plateforme.AlloTabib.DomainLayer.Entities;
using Plateforme.AlloTabib.DomainLayer.Helpers;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;
using PlateformeAlloTabib.Standards.Helpers;
using System.Globalization;
using Plateforme.AlloTabib.DomainLayer.Base.Interfaces;

namespace Plateforme.AlloTabib.DomainLayer.DomainServices.CalendrierDomainServices
{
    /// <summary>
    /// table rendez vous : statut sont : R( rejeté), NC (non confirmé ou en cours de confirmation) ou C : confirmé
    /// table creneaux : pour R => le statut est D (disponible) et pour C/NC =>le statut est ND
    /// Et on test aussi sur le jour férié qui est un jour non disponible et s'il contient des rendez vous il seront rejetés
    /// </summary>
    public class CalendrierDomainServices : ICalendrierDomainServices
    {
        private readonly IRepository<ConfigurationPraticien> _configurationPraticienRepository;
        private readonly IRepository<RendezVous> _rendezvousRepository;
        private readonly IRepository<Creneaux> _creneauRepository;
        private readonly IRepository<JourFerie> _jourFerieRepository;
        private readonly IRepository<Patient> _patientRepository;
        private readonly IRepository<Praticien> _praticienRepository;
        private CultureInfo _cultureFr = new CultureInfo("fr-FR");

        public CalendrierDomainServices(IRepository<ConfigurationPraticien> configurationPraticienRepository,
            IRepository<RendezVous> rendezvousRepository, IRepository<Creneaux> creneauRepository,
            IRepository<JourFerie> jourFerieRepository, IRepository<Patient> patientRepository,
            IRepository<Praticien> praticienRepository)
        {

            if (configurationPraticienRepository == null)
                throw new ArgumentNullException("configurationPraticienRepository");
            _configurationPraticienRepository = configurationPraticienRepository;
            if (rendezvousRepository == null)
                throw new ArgumentNullException("rendezvousRepository");
            _rendezvousRepository = rendezvousRepository;
            if (creneauRepository == null)
                throw new ArgumentNullException("creneauRepository");
            _creneauRepository = creneauRepository;
            if (jourFerieRepository == null)
                throw new ArgumentNullException("jourFerieRepository");
            _jourFerieRepository = jourFerieRepository;
            _patientRepository = patientRepository;
            _praticienRepository = praticienRepository;
        }

        public ResultOfType<CalendrierResultModel> GetCalendrierParPraticienAndDate(
            ConfigurationPraticien configurationPraticien, DateTime? dateSelected)
        {

            Logger.LogInfo("GetCalendrierParPraticienAndDate : Start ");
            if (configurationPraticien == null)
                return new Return<CalendrierResultModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                    null, "La configuration n'a pas été effectué par le praticien.").WithDefaultResult();
            if (dateSelected == null)
                dateSelected = DateTime.UtcNow;

            //Récupérer la liste des rendez vous par date et par praticien

            var listRendezVous =
                _rendezvousRepository.GetAll()
                    .Where(
                        x =>

                            x.Praticien.Cin == configurationPraticien.Praticien.Cin);

            IList<CalendrierDataModel> calendrierPraticien = new List<CalendrierDataModel>();
            //Matin
            var startTimeMatin = string.Concat(AddZero(configurationPraticien.HeureDebutMatin), ":",
                AddZero(configurationPraticien.MinuteDebutMatin));
            var endTimeMatin = string.Concat(AddZero(configurationPraticien.HeureFinMatin), ":",
                AddZero(configurationPraticien.MinuteFinMatin));
            //Midi
            var startTimeMidi = string.Concat(AddZero(configurationPraticien.HeureDebutMidi), ":",
                AddZero(configurationPraticien.MinuteDebutMidi));
            var endTimeMidi = string.Concat(AddZero(configurationPraticien.HeureFinMidi), ":",
                AddZero(configurationPraticien.MinuteFinMidi));

            var calendrierStatus = new CalendrierDataModel();

            //Si on n'a pas de rendez vous dans ce jour alors on affiche le calendrier vide
            var rendezVousList = listRendezVous as IList<RendezVous> ?? listRendezVous.ToList();
            if (rendezVousList.ToList().Count == 0)
            {


                //C'est disponible
                const string statut = "Pas de rendez vous";
                calendrierPraticien.ToList()
                    .AddRange(CalendrierFerie(configurationPraticien, startTimeMatin, endTimeMatin, calendrierStatus,
                        statut));
                calendrierPraticien.ToList()
                    .AddRange(CalendrierFerie(configurationPraticien, startTimeMidi, endTimeMidi, calendrierStatus,
                        statut));

            }
            else
            {
                calendrierPraticien = CalendrierReservationStatus(configurationPraticien, startTimeMatin, endTimeMatin,
                    calendrierStatus, rendezVousList);
                calendrierPraticien.ToList()
                    .AddRange(CalendrierReservationStatus(configurationPraticien, startTimeMidi, endTimeMidi,
                        calendrierStatus, rendezVousList));
            }
            var data = new CalendrierResultModel {CalendrierPraticien = calendrierPraticien};
            Logger.LogInfo("GetCalendrierParPraticienAndDate : End.");
            return new Return<CalendrierResultModel>().OK().WithResult(data);
        }

        private IList<CalendrierDataModel> CalendrierReservationStatus(ConfigurationPraticien configurationPraticien,
            string startTimeMatin,
            string endTimeMatin, CalendrierDataModel calendrierStatus, IList<RendezVous> rendezVousList)
        {


            IList<CalendrierDataModel> calendrierList = new List<CalendrierDataModel>();
            //Ce jour est non visible pour les utilisateurs et vide pour le praticien
            while (startTimeMatin.Equals(endTimeMatin))
            {
                //Remplir le calendrier
                calendrierStatus.HeureDebutCalendrier = startTimeMatin;
                calendrierStatus.PatientCin = null;
                calendrierStatus.PraticienCin = configurationPraticien.Praticien.Cin;
                string statut = "Pas de rendez vous";
                //TODO
                //foreach (var rendezvous in rendezVousList)
                //{
                //    var consultationTime = string.Concat(AddZero(rendezvous.HeureConsultation), ":",
                //        AddZero(rendezvous.MinuteConsultation));
                //    if (!consultationTime.Equals(startTimeMatin)) continue;
                //    statut = "Réservé";
                //    calendrierStatus.Statut = statut;
                //    calendrierStatus.PatientCin = rendezvous.Patient.Cin;
                //}

                calendrierStatus.Statut = statut;
                calendrierList.Add(calendrierStatus);

                int minute = int.Parse(configurationPraticien.MinuteDebutMatin);
                int heure = int.Parse(configurationPraticien.HeureDebutMatin);
                minute = minute + configurationPraticien.DecalageMinute;
                if (minute >= 60)
                {
                    minute = minute - 60;
                    heure = heure + 1;
                }

                startTimeMatin = string.Concat(AddZero(heure.ToString()), ":", AddZero(minute.ToString()));
            }
            return calendrierList;
        }

        private IList<CalendrierDataModel> CalendrierFerie(ConfigurationPraticien configurationPraticien,
            string startTimeMatin, string endTimeMatin, CalendrierDataModel calendrierStatus, string statut)
        {
            IList<CalendrierDataModel> calendrierList = new List<CalendrierDataModel>();
            //Ce jour est non visible pour les utilisateurs et vide pour le praticien
            while (startTimeMatin.Equals(endTimeMatin))
            {
                //Remplir le calendrier
                calendrierStatus.HeureDebutCalendrier = startTimeMatin;
                calendrierStatus.PatientCin = null;
                calendrierStatus.PraticienCin = configurationPraticien.Praticien.Cin;
                calendrierStatus.Statut = statut;
                calendrierList.Add(calendrierStatus);

                int minute = int.Parse(configurationPraticien.MinuteDebutMatin);
                int heure = int.Parse(configurationPraticien.HeureDebutMatin);
                minute = minute + configurationPraticien.DecalageMinute;
                if (minute >= 60)
                {
                    minute = minute - 60;
                    heure = heure + 1;
                }

                startTimeMatin = string.Concat(AddZero(heure.ToString()), ":", AddZero(minute.ToString()));
            }
            return calendrierList;
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
                if (h.Equals('0'.ToString()))
                    return hh.Substring(1, 1);
                else
                {
                    return hh;
                }
            }

            return hh;
        }

        /// <summary>
        /// C'est une méthode permettant d'afficher les disponibilités d'un medecin coté patient
        /// </summary>
        /// <param name="praticien"></param>
        /// <param name="dateSelected"></param>
        /// <returns></returns>
        public ResultOfType<CalendrierPatientDataModel> GetCalendrierParPraticienForPatient(string praticien,
            string dateSelected)
        {

            try
            {
                Logger.LogInfo("GetCalendrierParPraticienForPatient : Start. ");
                //nous avant la configuration du médecin qui construit la premiere liste 
                //puis on tranche de la liste les créneaux non disponible
                if (string.IsNullOrEmpty(praticien))
                    return
                        new Return<CalendrierPatientDataModel>().Error()
                            .As(EStatusDetail.BadRequest)
                            .AddingGenericError(
                                null, "L'email du praticien est null.").WithDefaultResult();
                if (string.IsNullOrEmpty(dateSelected))
                    return
                        new Return<CalendrierPatientDataModel>().Error()
                            .As(EStatusDetail.BadRequest)
                            .AddingGenericError(
                                null, "La date courante est null.").WithDefaultResult();

                ConfigurationPraticien configurationPraticien =
                    _configurationPraticienRepository.GetAll().FirstOrDefault(c => c.Praticien.Email == praticien);


                if (configurationPraticien == null)
                    return
                        new Return<CalendrierPatientDataModel>().Error()
                            .As(EStatusDetail.BadRequest)
                            .AddingGenericError(
                                null,
                                "Aucune configuration effectué pour le compte en compte.Veuillez contacter l'administrateur de AlloTabib pour établir une première configuration.")
                            .WithDefaultResult();

                IList<Creneaux> creneauxConfig =
                    _creneauRepository.GetAll()
                        .Where(cr => cr.Praticien.Email.Equals(praticien) && cr.CurrentDate.Equals(dateSelected))
                        .ToList();

                IList<string> heures = new List<string>();
                int decalage = configurationPraticien.DecalageMinute;

                
                var jourFerie =
                    _jourFerieRepository.GetAll()
                        .FirstOrDefault(x => x.JourFerieNom.Contains(dateSelected) && x.Praticien.Email.Equals(praticien));
                DateTime dateCast;
                DateTime.TryParseExact(dateSelected, "dd/MM/yyyy",
                    _cultureFr, DateTimeStyles.None,
                          out dateCast);
                
                string day = DateTimeFormatInfo.CurrentInfo.GetDayName(dateCast.DayOfWeek);
                CalendrierPatientDataModel data = new CalendrierPatientDataModel();
                if (jourFerie != null || day.Equals(DayOfWeek.Sunday.ToString()))
                {
                    //Do the cast for the date
                    data = new CalendrierPatientDataModel
                    {
                        HeureCalendrier = null,
                        DateCourante = dateSelected.Substring(0, 5),
                        Jour = day
                    };
                    //On doit pas afficher ce jour la dans le cas d'un patient
                    return new Return<CalendrierPatientDataModel>().OK().WithResult(data);
                }
                else
                {
                    //si il n'a aucun créneau dans cette date on affiche toute les disponibilité tous le calendrier au patient
                    //if (creneauxConfig == null)
                    //{
                    if (configurationPraticien.MinuteDebutMatin != null &&
                        configurationPraticien.HeureDebutMatin != null &&
                        configurationPraticien.MinuteFinMatin != null && configurationPraticien.HeureFinMatin != null)
                    {
                        int minute = int.Parse(DeleteZero(configurationPraticien.MinuteDebutMatin));
                        int heure = int.Parse(DeleteZero(configurationPraticien.HeureDebutMatin));

                        int minuteFinMatin = int.Parse(DeleteZero(configurationPraticien.MinuteFinMatin));
                        int heureFinMatin = int.Parse(DeleteZero(configurationPraticien.HeureFinMatin));



                        string startTimeMatin = string.Concat(AddZero(heure.ToString()), ":", AddZero(minute.ToString()));

                        AjouterDisponibilitePraticien(heure, heureFinMatin, minute, minuteFinMatin, heures,
                            startTimeMatin,
                            decalage);
                        heures.RemoveAt(heures.Count - 1);
                    }


                    if (configurationPraticien.MinuteDebutMidi != null && configurationPraticien.HeureDebutMidi != null)
                    {
                        int minuteAp = int.Parse(DeleteZero(configurationPraticien.MinuteDebutMidi));
                        int heureAp = int.Parse(DeleteZero(configurationPraticien.HeureDebutMidi));

                        int minuteFinAp = int.Parse(DeleteZero(configurationPraticien.MinuteFinMidi));
                        int heureFinAp = int.Parse(DeleteZero(configurationPraticien.HeureFinMidi));


                        string startTimeMidi = string.Concat(AddZero(heureAp.ToString()), ":",
                            AddZero(minuteAp.ToString()));

                        AjouterDisponibilitePraticien(heureAp, heureFinAp, minuteAp, minuteFinAp, heures, startTimeMidi,
                            decalage);
                        heures.RemoveAt(heures.Count - 1);
                    }


                    //On doit traiter le cas des rendez vous 
                    if (creneauxConfig != null && creneauxConfig.ToList().Count > 0)
                    {
                        //ici on doit enlever des heurs les rendez vous prix 
                        foreach (Creneaux cre in creneauxConfig)
                        {
                            //vérifier si ce creneaux est lié à un rdv
                            var rdv = _rendezvousRepository.GetAll().FirstOrDefault(r => r.Creneaux.Id.Equals(cre.Id));
                            if (rdv != null)
                            {
                                if (rdv.Statut.Equals("R")) //Si le rendez vous a été rejeté alors il sera libéré
                                {
                                    if (heures.IndexOf(cre.HeureDebut) != -1)
                                    {
                                        if (cre.Status.Equals("ND".ToString()))
                                            heures.Remove(cre.HeureDebut);
                                    }
                                }
                            }

                            if (heures.IndexOf(cre.HeureDebut) != -1)
                            {
                                if (cre.Status.Equals("ND".ToString()))
                                    heures.Remove(cre.HeureDebut);
                            }
                        }
                    }

                    DateTime.TryParseExact(dateSelected, "dd/MM/yyyy",
                        _cultureFr, DateTimeStyles.None,
                              out dateCast);
                    day = DateTimeFormatInfo.CurrentInfo.GetDayName(dateCast.DayOfWeek);

                    //Do the cast for the date
                    data = new CalendrierPatientDataModel
                    {
                        HeureCalendrier = heures,
                        DateCourante = dateSelected,
                        Jour = day
                    };
                }
                Logger.LogInfo("GetCalendrierParPraticienForPatient : End .");
                return new Return<CalendrierPatientDataModel>().OK().WithResult(data);
            }
            catch (Exception ex)
            {
                return
                         new Return<CalendrierPatientDataModel>().Error()
                             .As(EStatusDetail.BadRequest)
                             .AddingGenericError(
                                 null, "Erreur de récupération du jours.").WithDefaultResult();
            }
        }

        private void AjouterDisponibilitePraticien(int heure, int heureFinMatin, int minute, int minuteFinMatin,
            IList<string> heures,
            string startTimeMatin, int decalage)
        {
            bool isTheEnd = false;
            while (!(isTheEnd))
            {
                isTheEnd = (heure == heureFinMatin && minute >= minuteFinMatin);
                heures.Add(startTimeMatin);

                minute = minute + decalage;
                if (minute >= 60)
                {
                    minute = minute - 60;
                    heure = heure + 1;
                }

                startTimeMatin = string.Concat(AddZero(heure.ToString()), ":", AddZero(minute.ToString()));
            }
        }

        public ResultOfType<IList<CalendrierPatientDataModel>> GetCalendrierParPraticienForPatientParSemaine(
            string praticien,
            string dateSelected)
        {
            try
            {

                Logger.LogInfo("GetCalendrierParPraticienForPatientParSemaine : Start. ");

                IList<CalendrierPatientDataModel> calendriersPatientList = new List<CalendrierPatientDataModel>();
                int i = 0;
                while (i < 7)
                {
                   
                    DateTime today;
                    DateTime.TryParseExact(dateSelected, "dd/MM/yyyy",
                        _cultureFr, DateTimeStyles.None,
                              out today);
                    DateTime yesterday = today.AddDays(-1);

                    if (yesterday.CompareTo(DateTime.Now) > 0)
                    {
                        today = yesterday;
                        i++;
                        dateSelected = today.ToShortDateString();

                        var data = GetCalendrierParPraticienForPatient(praticien, dateSelected);

                        if (data != null && data.Data != null)
                        {
                            var calendrierPatientDataModel = new CalendrierPatientDataModel
                            {
                                DateCourante = data.Data.DateCourante,
                                HeureCalendrier = data.Data.HeureCalendrier,
                                Jour = data.Data.Jour
                            };

                            calendriersPatientList.Add(calendrierPatientDataModel);
                        }

                    }
                    else
                    {
                        return
                            new Return<IList<CalendrierPatientDataModel>>().Error()
                                .As(EStatusDetail.NotFound)
                                .AddingGenericError(
                                    null,
                                    "Vous ne pouvez pas revenir vers la semaine précédente car la date est inférieur.")
                                .WithDefaultResult();

                    }
                }

                var calendrierPatientDataModels = calendriersPatientList.Reverse().ToList();
                Logger.LogInfo("GetCalendrierParPraticienForPatientParSemaine : End. ");
                return new Return<IList<CalendrierPatientDataModel>>().OK().WithResult(calendrierPatientDataModels);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ResultOfType<IList<CalendrierPatientDataModel>> GetCalendrierParPraticienForPatientParSem(
            string praticien,
            string dateSelected)
        {
            try
            {

                Logger.LogInfo("GetCalendrierParPraticienForPatientParSem : Start. ");
                IList<CalendrierPatientDataModel> calendriersPatientList = new List<CalendrierPatientDataModel>();
                for (int i = 0; i < 7; i++)
                {
                    DateTime today;
                    DateTime.TryParseExact(dateSelected, "dd/MM/yyyy",
                        _cultureFr, DateTimeStyles.None,
                              out today);
                    DateTime tomorrow = today.AddDays(1);
                    today = tomorrow;
                    dateSelected = today.ToShortDateString();

                    var data = GetCalendrierParPraticienForPatient(praticien, dateSelected);

                    if (data != null && data.Data != null)
                    {
                        var calendrierPatientDataModel = new CalendrierPatientDataModel
                        {
                            DateCourante = data.Data.DateCourante,
                            HeureCalendrier = data.Data.HeureCalendrier,
                            Jour = data.Data.Jour
                        };

                        calendriersPatientList.Add(calendrierPatientDataModel);
                    }



                }
                Logger.LogInfo("GetCalendrierParPraticienForPatientParSem : End .");
                return new Return<IList<CalendrierPatientDataModel>>().OK().WithResult(calendriersPatientList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="praticien"></param>
        /// <param name="dateSelected"></param>
        /// <returns></returns>
        public ResultOfType<CalendrierResultModel> GetCalendrierParPraticienForPraticien(string praticien,
            string dateSelected)
        {
            Logger.LogInfo("GetCalendrierParPraticienForPraticien : Start.");

            //Afficher tout le calendrier
            if (string.IsNullOrEmpty(praticien))
                return new Return<CalendrierResultModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                    null, "Le cin du praticien est null.").WithDefaultResult();
            if (string.IsNullOrEmpty(dateSelected))
                return new Return<CalendrierResultModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                    null, "La date courante est null.").WithDefaultResult();

            //var startHeure, endHeure;
            ConfigurationPraticien configurationPraticien =
                _configurationPraticienRepository.GetAll().FirstOrDefault(c => c.Praticien.Email == praticien);


            if (configurationPraticien == null)
                return new Return<CalendrierResultModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                    null,
                    "Aucune configuration effectué pour le compte en compte.Veuillez contacter l'administrateur de AlloTabib pour établir une première configuration.")
                    .WithDefaultResult();

            IList<Creneaux> creneauxConfig =
                _creneauRepository.GetAll()
                    .Where(cr => cr.Praticien.Email.Equals(praticien) && cr.CurrentDate.Equals(dateSelected)).ToList();
            //Créer la liste d'objets contenant cette éléments qui seront affichés au médecin
            IList<CalendrierDataModel> listCalendrierDataModels = new List<CalendrierDataModel>();


            int decalage = configurationPraticien.DecalageMinute;
            var jourFerie =
                _jourFerieRepository.GetAll()
                    .FirstOrDefault(x => x.JourFerieNom.Equals(dateSelected) && x.Praticien.Email.Equals(praticien));

            if (configurationPraticien.MinuteDebutMatin != null && configurationPraticien.HeureDebutMatin != null &&
                configurationPraticien.MinuteFinMatin != null && configurationPraticien.HeureFinMatin != null)
            {
                int minute = int.Parse(DeleteZero(configurationPraticien.MinuteDebutMatin));
                int heure = int.Parse(DeleteZero(configurationPraticien.HeureDebutMatin));

                int minuteFinMatin = int.Parse(DeleteZero(configurationPraticien.MinuteFinMatin));
                int heureFinMatin = int.Parse(DeleteZero(configurationPraticien.HeureFinMatin));



                string startTimeMatin = string.Concat(AddZero(heure.ToString()), ":", AddZero(minute.ToString()));
                string endTimeMatin = string.Concat(AddZero(heureFinMatin.ToString()), ":",
                    AddZero(minuteFinMatin.ToString()));

                AjouterCalendrierPraticien(heure, heureFinMatin, minute, minuteFinMatin, listCalendrierDataModels,
                    startTimeMatin, endTimeMatin,
                    decalage);
            }
            //: je garde au mécedin ce crénaux en cas ou il a besoin d'ajouter un rdv urgent dans sa pause
            listCalendrierDataModels.RemoveAt(listCalendrierDataModels.Count - 1);

            if (configurationPraticien.MinuteDebutMidi != null && configurationPraticien.HeureDebutMidi != null)
            {
                int minuteAp = int.Parse(DeleteZero(configurationPraticien.MinuteDebutMidi));
                int heureAp = int.Parse(DeleteZero(configurationPraticien.HeureDebutMidi));

                int minuteFinAp = int.Parse(DeleteZero(configurationPraticien.MinuteFinMidi));
                int heureFinAp = int.Parse(DeleteZero(configurationPraticien.HeureFinMidi));

                string startTimeMidi = string.Concat(AddZero(heureAp.ToString()), ":", AddZero(minuteAp.ToString()));
                string endTimeMidi = string.Concat(AddZero(heureFinAp.ToString()), ":", AddZero(minuteFinAp.ToString()));

                AjouterCalendrierPraticien(heureAp, heureFinAp, minuteAp, minuteFinAp, listCalendrierDataModels,
                    startTimeMidi, endTimeMidi,
                    decalage);

            }
            //Supprimer le dernier élément: je garde au mécedin ce crénaux en cas ou il a besoin d'ajouter un rdv urgent dans sa pause
            //listCalendrierDataModels.RemoveAt(listCalendrierDataModels.Count - 1);

            var finalList = new List<CalendrierDataModel>();
            if (creneauxConfig != null && creneauxConfig.ToList().Count > 0)
            {
                foreach (CalendrierDataModel cal in listCalendrierDataModels)
                {
                    if (jourFerie != null)
                    {
                        cal.Statut = "ND";
                        finalList.Add(cal);
                    }
                    else
                    {
                        var creneau =
                            creneauxConfig.FirstOrDefault(
                                cr => cr.HeureDebut == cal.HeureDebutCalendrier && cr.CurrentDate.Equals(dateSelected));
                        if (creneau != null)
                        {

                            //On doit traiter le cas des rendez vous 
                            var rdv = _rendezvousRepository.GetAll().FirstOrDefault(c => c.Creneaux.Id == creneau.Id);

                            string status = creneau.Status;
                            string praticienCin = string.Empty;
                            string patientCin = string.Empty;
                            string heurefin = creneau.HeureFin;
                            string nomPrenomPatient = string.Empty;
                            string telephonePatient = string.Empty;





                            if (rdv != null)
                            {
                                //get patient or praticien of it's a praticien
                                var pat =
                                    _patientRepository.GetAll().FirstOrDefault(p => p.Email.Equals(rdv.Patient.Email));
                                if (pat == null)
                                {
                                    var prat =
                                        _praticienRepository.GetAll()
                                            .FirstOrDefault(pr => pr.Email.Equals(rdv.Patient.Email));

                                    if (prat != null)
                                    {
                                        nomPrenomPatient = prat.NomPrenom;
                                        telephonePatient = prat.Telephone;
                                    }
                                }
                                else
                                {
                                    nomPrenomPatient = pat.NomPrenom;
                                    telephonePatient = pat.Telephone;
                                }
                                // status = rdv.Statut;
                                praticienCin = rdv.Praticien.Cin;
                                patientCin = rdv.Patient.Email;
                            }

                            var elem =
                                listCalendrierDataModels.FirstOrDefault(
                                    w => w.HeureDebutCalendrier == creneau.HeureDebut);
                            elem.HeureDebutCalendrier = creneau.HeureDebut;
                            elem.HeureFinCalendrier = heurefin;
                            elem.PatientCin = patientCin;
                            elem.PraticienCin = praticienCin;
                            elem.Statut = status;
                            elem.NomPrenomPatient = nomPrenomPatient;
                            elem.TelephonePatient = telephonePatient;
                            finalList.Add(elem);


                        }
                        else
                        {
                            if (jourFerie != null)
                            {
                                cal.Statut = "ND";
                                finalList.Add(cal);
                            }
                            else

                                finalList.Add(cal);
                        }

                    }
                }

            }
            else
            {
                finalList.AddRange(listCalendrierDataModels);
                if (jourFerie != null)
                {
                    finalList.ForEach(cal =>
                    {
                        cal.Statut = "ND";
                    });
                }
            }

            ////On doit traiter le cas des rendez vous 
            //if (creneauxConfig != null && creneauxConfig.ToList().Count > 0)
            //{
            //    //ici on doit enlever des heurs les rendez vous prix 
            //    foreach (Creneaux cre in creneauxConfig)
            //    {
            //        CalendrierDataModel cal = listCalendrierDataModels.FirstOrDefault(x => x.HeureDebutCalendrier == cre.HeureDebut);
            //        if (cal != null)
            //        {
            //            if (cre.Status.Equals("ND".ToString()))
            //            {
            //                var rdv = _rendezvousRepository.GetAll().FirstOrDefault(c => c.Creneaux.Id == cre.Id);
            //                string status = "ND";
            //                string praticienCin = string.Empty;
            //                string patientCin = string.Empty;
            //                string heurefin = cre.HeureFin;

            //                if (rdv != null)
            //                {
            //                    status = rdv.Statut;
            //                    praticienCin = rdv.Praticien.Cin;
            //                    patientCin = rdv.Patient.Cin;
            //                }

            //                var elem = listCalendrierDataModels.FirstOrDefault(w => w.HeureDebutCalendrier == cre.HeureDebut);
            //                elem.HeureDebutCalendrier = cre.HeureDebut;
            //                elem.HeureFinCalendrier = heurefin;
            //                elem.PatientCin = patientCin;
            //                elem.PraticienCin = praticienCin;
            //                elem.Statut = status;

            //                finalList.Add(elem);

            //            }

            //        }
            //        else
            //            finalList.Add(cal);
            //    }
            //}
            //else
            //{
            //    finalList = listCalendrierDataModels;
            //}

            CalendrierResultModel data = new CalendrierResultModel
            {
                CalendrierPraticien = finalList
            };

            Logger.LogInfo("GetCalendrierParPraticienForPraticien : End. ");
            return new Return<CalendrierResultModel>().OK().WithResult(data);
        }

        private void AjouterCalendrierPraticien(int heure, int heureFinMatin, int minute, int minuteFinMatin,
            IList<CalendrierDataModel> listCalendrierDataModels, string startTime, string endTime, int decalage)
        {
            bool isTheEnd = false;
            while (!(isTheEnd))
            {
                isTheEnd = (heure == heureFinMatin && minute == minuteFinMatin);
                var calendrierDataModel = new CalendrierDataModel();
                calendrierDataModel.HeureDebutCalendrier = startTime;
                calendrierDataModel.Statut = "D";


                minute = minute + decalage;
                if (minute >= 60)
                {
                    minute = minute - 60;
                    heure = heure + 1;
                }

                startTime = string.Concat(AddZero(heure.ToString()), ":", AddZero(minute.ToString()));

                calendrierDataModel.HeureFinCalendrier = startTime;
                listCalendrierDataModels.Add(calendrierDataModel);

                endTime = string.Concat(AddZero(heureFinMatin.ToString(CultureInfo.CurrentCulture)), ":", AddZero(minuteFinMatin.ToString()));
            }
        }


        public ResultOfType<CalendarPraticienByDay> GetPremiereDisponibilite(string praticienEmail, string dateSelected)
        {
            if (string.IsNullOrEmpty(praticienEmail))
                return new Return<CalendarPraticienByDay>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                    null, "L'Email du praticien est null.").WithDefaultResult();

            try
            {
                Logger.LogInfo("GetCalendrierParPraticienForPatientParSemaine : Start. ");

                var calendarPraticienByDay = new CalendarPraticienByDay();
                int i = 0;

                DateTime today;
                DateTime.TryParseExact(dateSelected, "dd/MM/yyyy",
                    _cultureFr, DateTimeStyles.None,
                          out today);
                bool isOk = false;
                while (isOk == false)
                {
                    DateTime tomorrow = today.AddDays(1);

                    today = tomorrow;
                    dateSelected = today.ToShortDateString();

                    var data = GetCalendrierParPraticienForPatient(praticienEmail, dateSelected);

                    if (data != null && data.Data != null && !string.IsNullOrEmpty(data.Data.DateCourante) && !string.IsNullOrEmpty(data.Data.Jour) && data.Data.HeureCalendrier !=null)
                    {
                        isOk = true;
                        calendarPraticienByDay.DatePremiere = data.Data.DateCourante;
                        calendarPraticienByDay.HeurePremiere = data.Data.HeureCalendrier.FirstOrDefault();
                        calendarPraticienByDay.JourPremier = data.Data.Jour;
                    }                  
                }
             
                Logger.LogInfo("GetCalendrierParPraticienForPatientParSemaine : End. ");
                return new Return<CalendarPraticienByDay>().OK().WithResult(calendarPraticienByDay);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}