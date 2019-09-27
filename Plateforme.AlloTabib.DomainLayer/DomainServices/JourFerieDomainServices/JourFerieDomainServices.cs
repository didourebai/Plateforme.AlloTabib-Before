using Plateforme.AlloTabib.CrossCuttingLayer.Logging;
using Plateforme.AlloTabib.DomainLayer.Entities;
using Plateforme.AlloTabib.DomainLayer.Helpers;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;
using PlateformeAlloTabib.Standards.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plateforme.AlloTabib.DomainLayer.Base.Interfaces;

namespace Plateforme.AlloTabib.DomainLayer.DomainServices.JourFerieDomainServices
{
    public class JourFerieDomainServices : IJourFerieDomainServices
    {
        #region Private Properties

        private readonly IRepository<JourFerie> _jourFerieRepository;
        private readonly IRepository<Praticien> _praticienRepository;
        private readonly IRepository<RendezVous> _rendezVousRepository;
       

        #endregion

        public JourFerieDomainServices(IRepository<JourFerie> jourFerieRepository, IRepository<Praticien> praticienRepository, IRepository<RendezVous> rendezVousRepository)
        {
            if (jourFerieRepository == null)
                throw new ArgumentNullException("jourFerieRepository");
            _jourFerieRepository = jourFerieRepository;

            if (praticienRepository == null)
                throw new ArgumentNullException("praticienRepository");
            _praticienRepository = praticienRepository;
            if (rendezVousRepository == null)
                throw new ArgumentNullException("rendezVousRepository");
            _rendezVousRepository = rendezVousRepository;
           
        }

        public ResultOfType<JourFerieResultDataModel> PostNewJourFerie(JourFerieDataModel jourFerieDto)
        {
            try
            {
                if (jourFerieDto == null)
                    return new Return<JourFerieResultDataModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                        null, "Les données sont vides.").WithDefaultResult();

                Logger.LogInfo(string.Format("Post New jour férié : Start --- jour = {0}, praticien mail = {1}",
                                               jourFerieDto.JourFerieNom, jourFerieDto.PraticienEmail));

                if(string.IsNullOrEmpty(jourFerieDto.JourFerieNom))
                    return new Return<JourFerieResultDataModel>()
                     .Error().AsValidationFailure(null, "Veuillez introduire le jour férié.", "JourFerieNom")
                     .WithDefaultResult();

                if (string.IsNullOrEmpty(jourFerieDto.PraticienEmail))
                    return new Return<JourFerieResultDataModel>()
                     .Error().AsValidationFailure(null, "Veuillez introduire l'email du praticien.", "PraticienEmail")
                     .WithDefaultResult();
                var praticien = _praticienRepository.GetAll().FirstOrDefault(pr=>pr.Email.Equals(jourFerieDto.PraticienEmail));

                if(praticien == null)
                    return new Return<JourFerieResultDataModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                      null, "Praticien n'existe pas.").WithDefaultResult();

                JourFerie jourferie = new JourFerie
                {
                    JourFerieNom = jourFerieDto.JourFerieNom,
                    Praticien = praticien
                };

                _jourFerieRepository.Add(jourferie);
                //Mettre tous les rendez vous en statut = R (rejeté)
                var _rendezVous = _rendezVousRepository.GetAll().Where(r => r.Creneaux.CurrentDate.Equals(jourFerieDto.JourFerieNom) && r.Praticien.Email.Equals(jourFerieDto.PraticienEmail)).ToList();
                if(_rendezVous !=null && _rendezVous.Count()>0)
                {
                    _rendezVous.ForEach(rdv => { rdv.Statut = "R";
                    _rendezVousRepository.Update(rdv);
                    
                    });
                }

                return new Return<JourFerieResultDataModel>().OK()
                        .WithResult(new JourFerieResultDataModel
                        {
                            JourFerieNom = jourferie.JourFerieNom,
                            PraticienEmail = jourferie.Praticien.Email
                        });
            }
            catch
            {
                throw;
            }
        }

        public ResultOfType<Null> DeleteJourFerie(string jourFeriename, string email)
        {
            if (string.IsNullOrEmpty(jourFeriename))
                return new Return<Null>()
                       .Error().AsValidationFailure(null, "Aucun jour férié a été passé.", "jourFeriename")
                       .WithDefaultResult();
            if (string.IsNullOrEmpty(email))
                return new Return<Null>()
                       .Error().AsValidationFailure(null, "Aucun email a été passé.", "email")
                       .WithDefaultResult();

            try
            {
                var _jourFeries = _jourFerieRepository.GetAll().FirstOrDefault(j => j.JourFerieNom.Equals(jourFeriename) && j.Praticien.Email.Equals(email));
                if(_jourFeries == null)
                    return new Return<Null>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                         null, "Aucun rendez vous existe.").WithDefaultResult();

                _jourFerieRepository.Delete(_jourFeries.Id);

                var _rendezVous = _rendezVousRepository.GetAll().Where(r => r.Creneaux.CurrentDate.Equals(jourFeriename) && r.Praticien.Email.Equals(email)).ToList();
                if (_rendezVous != null && _rendezVous.Count() > 0)
                {
                    _rendezVous.ForEach(rdv =>
                    {
                        rdv.Statut = "NC";
                        _rendezVousRepository.Update(rdv);

                    });
                }
                return new Return<Null>().OK().WithDefaultResult();
            }
            catch (Exception ex)
            {
                Logger.LogInfo(string.Format("Delete jour férié : end error --- Exception = {0}", ex.Message));

                throw;
            }
        }


        public ResultOfType<JourFerieResultDataModel> EstUnJourFerie(string jourFerieName, string email)
        {
            if (string.IsNullOrEmpty(jourFerieName))
                return new Return<JourFerieResultDataModel>()
                       .Error().AsValidationFailure(null, "Aucun jour férié a été passé.", "jourFeriename")
                       .WithDefaultResult();
            if (string.IsNullOrEmpty(email))
                return new Return<JourFerieResultDataModel>()
                       .Error().AsValidationFailure(null, "Aucun email a été passé.", "email")
                       .WithDefaultResult();
            try
            {
                var jourFerie = _jourFerieRepository.GetAll().FirstOrDefault(j => j.JourFerieNom.Equals(jourFerieName) && j.Praticien.Email.Equals(email));

                if (jourFerie == null)

                    return new Return<JourFerieResultDataModel>().OK()
                       .WithResult(new JourFerieResultDataModel());

                return new Return<JourFerieResultDataModel>().OK()
                       .WithResult(new JourFerieResultDataModel
                       {
                           JourFerieNom = jourFerieName,
                           PraticienEmail = email
                       });
            }
            catch (Exception ex)
            {
                Logger.LogInfo(string.Format("Delete jour férié : end error --- Exception = {0}", ex.Message));

                throw;
            }
        }
    }
}
