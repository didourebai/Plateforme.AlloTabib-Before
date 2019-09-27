using Microsoft.Practices.Unity;
using System.Web.Http;
using Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.AlloTabibUserServices;
using Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.ContactAppServices;
using Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.PatientAppServices;
using Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.PraticienAppServices;
using Plateforme.AlloTabib.CrossCuttingLayer.Adapter;
using Plateforme.AlloTabib.DomainLayer;
using Plateforme.AlloTabib.DomainLayer.DomainServices.AlloTabibUserDomainServices;
using Plateforme.AlloTabib.DomainLayer.DomainServices.ContactDomainServices;
using Plateforme.AlloTabib.DomainLayer.DomainServices.PatientDomainServices;
using Plateforme.AlloTabib.DomainLayer.DomainServices.PraticienDomainServices;
using Plateforme.AlloTabib.DomainLayer.Entities;
using Plateforme.AlloTabib.InfrastructureLayer.BaseContext;
using Plateforme.AlloTabib.ServiceLayer.Controllers.AlloTabibApi;
using Unity.WebApi;
using Plateforme.AlloTabib.InfrastructureLayer.SessionFactories;
using NHibernate;
using NHibernate.Context;
using Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.CalendrierAppServices;
using Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.ConfigurationAppServices;
using Plateforme.AlloTabib.DomainLayer.DomainServices.CalendrierDomainServices;
using Plateforme.AlloTabib.DomainLayer.DomainServices.ConfigurationDomainServices;
using Plateforme.AlloTabib.DomainLayer.DomainServices.CreneauDomainServices;
using Plateforme.AlloTabib.DomainLayer.DomainServices.GoogleMapsDomainServices;
using Plateforme.AlloTabib.DomainLayer.DomainServices.RendezVousDomainServices;
using Plateforme.AlloTabib.DomainLayer.Models;
using Plateforme.AlloTabib.DomainLayer.SearchEngine;
using Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.RendezVousAppServices;
using Plateforme.AlloTabib.DomainLayer.DomainServices.JourFerieDomainServices;
using Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.JourFerieAppServices;
using Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.CreneauAppServices;
using Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.TwitterAppServices;
using Plateforme.AlloTabib.DomainLayer.DomainServices.VideosDomainServices;
using Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.VideoAppServices;
using Plateforme.AlloTabib.DomainLayer.Base.Interfaces;
using Plateforme.AlloTabib.DomainLayer.Mapping;
using Plateforme.AlloTabib.InfrastructureLayer.BaseContext.Enumerations;

namespace Plateforme.AlloTabib.ServiceLayer
{
    public static class UnityConfig
    {
        private static IUnityContainer _currentContainer;

        public static IUnityContainer CurrentContainer
        {
            get { return _currentContainer ?? (_currentContainer = new UnityContainer()); }
        }

        public static void RegisterComponents()
        {

            ConfigureContainer();
            ConfigureFactories();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(CurrentContainer);
          
        }

        private static void ConfigureFactories()
        {
            //var typeAdapterFactory = CurrentContainer.Resolve<ITypeAdapterFactory>();
            //TypeAdapterFactory.SetCurrent(typeAdapterFactory);

            //Register a singleton instance for IBuildSessionFactory and ISessionFactory.
            //ISessionFactoriesManager sessionFactoriesManager = new SessionFactoriesManager();
            ////sessionFactoriesManager.AddSessionFactoryForNamespaceOf<Report, AreaMap>(EAvailableDBMS.Oracle9.ToString(), "axrg", true, false, true);
            //sessionFactoriesManager.AddSessionFactoryForNamespaceOf<Patient, PatientMap>(EAvailableDBMS.SqlServer2012.ToString(), "DefaultConnection");
            //CurrentContainer.RegisterInstance(sessionFactoriesManager);

            //var sessionBuilder = new BuildSessionFactoryForSqlServer();
            //CurrentContainer.RegisterInstance<IBuildSessionFactory>(sessionBuilder);
            //CurrentContainer.RegisterInstance(sessionBuilder.BuildSessionFactory());
        }

        private static void ConfigureContainer()
        {
            // Controllers
            CurrentContainer.RegisterType<ContactApiController>();
            CurrentContainer.RegisterType<PatientApiController>();
            CurrentContainer.RegisterType<PraticienApiController>();
            CurrentContainer.RegisterType<UserAccountController>();
            CurrentContainer.RegisterType<ConfigurationController>();
            CurrentContainer.RegisterType<CalendrierPraticienController>();
            CurrentContainer.RegisterType<RendezVousController>();
            CurrentContainer.RegisterType<CreneauController>();
            CurrentContainer.RegisterType<JourFerieController>();

            //Repositories
            CurrentContainer.RegisterType<IRepository<Patient>, Repository<Patient>>();
            CurrentContainer.RegisterType<IRepository<UserAccount>, Repository<UserAccount>>();
            CurrentContainer.RegisterType<IRepository<Praticien>, Repository<Praticien>>();
            CurrentContainer.RegisterType<IRepository<GoogleMapsCoordinations>, Repository<GoogleMapsCoordinations>>();
            CurrentContainer.RegisterType<IRepository<ConfigurationPraticien>, Repository<ConfigurationPraticien>>();
            CurrentContainer.RegisterType<IRepository<RendezVous>, Repository<RendezVous>>();
            CurrentContainer.RegisterType<IRepository<Creneaux>, Repository<Creneaux>>(); 
            CurrentContainer.RegisterType<IRepository<JourFerie>, Repository<JourFerie>>();
            CurrentContainer.RegisterType<IRepository<HistoriqueRendezVous>, Repository<HistoriqueRendezVous>>();
            CurrentContainer.RegisterType<IRepository<Video>, Repository<Video>>();
            // Lucene search engine
            CurrentContainer.RegisterType<ILuceneSearchEngine<PraticienToIndexModel>, PraticienLuceneSearchEngine>();

            // Application Services
            CurrentContainer.RegisterType<IPatientApplicationServices, PatientApplicationServices>();
            CurrentContainer.RegisterType<IAlloTabibUserAppServices, AlloTabibUserAppServices>();
            CurrentContainer.RegisterType<IPraticienApplicationServices, PraticienApplicationServices>();
            CurrentContainer.RegisterType<IContactAppServices, ContactAppServices>();
            CurrentContainer.RegisterType<IConfigurationAppServices, ConfigurationAppServices>();
            CurrentContainer.RegisterType<ICalendrierAppServices, CalendrierAppServices>();
            CurrentContainer.RegisterType<IRendezVousAppServices, RendezVousAppServices>();
            CurrentContainer.RegisterType<IJourFerieAppServices, JourFerieAppServices>();
            CurrentContainer.RegisterType<ICreneauAppServices, CreneauAppServices>();
            CurrentContainer.RegisterType<IVideoApplicationServices, VideoApplicationServices>();
            
            CurrentContainer.RegisterType<IFacebookUserDomainServices, FacebookUserDomainServices>();
            CurrentContainer.RegisterType<ITwitterUserDomainServices, TwitterUserDomainServices>();
           
           
            // Domain API Services
            CurrentContainer.RegisterType<IPatientDomainServices, PatientDomainServices>();
            CurrentContainer.RegisterType<IVideosDomainServices, VideosDomainServices>();
            CurrentContainer.RegisterType<IAlloTabibUserDomainServices, AlloTabibUserDomainServices>();
            CurrentContainer.RegisterType<IPraticienDomainServices, PraticienDomainServices>();
            CurrentContainer.RegisterType<IContactDomainServices, ContactDomainServices>();
            CurrentContainer.RegisterType<ICreneauDomainServices, CreneauDomainServices>();
            CurrentContainer.RegisterType<IRendezVousDomainServices, RendezVousDomainServices>();
            CurrentContainer.RegisterType<IConfigurationDomainServices, ConfigurationDomainServices>();
            CurrentContainer.RegisterType<ICalendrierDomainServices, CalendrierDomainServices>();
            CurrentContainer.RegisterType<IFacebookUserAppServices, FacebookUserAppServices>();
            CurrentContainer.RegisterType<IJourFerieDomainServices, JourFerieDomainServices>();
            CurrentContainer.RegisterType<ITwitterAppServices, TwitterAppServices>();
            // Automapper
            CurrentContainer.RegisterType<ITypeAdapterFactory, AutomapperTypeAdapterFactory>(
                new ContainerControlledLifetimeManager());
        }

        #region  Initialization

        private static ISessionFactory SessionFactory { get; set; }

        public static void OnExecuting()
        {
            SessionFactory = CurrentContainer.Resolve<ISessionFactory>();

            var session = SessionFactory.OpenSession();
            CurrentSessionContext.Bind(session);
            session.BeginTransaction();
        }

        public static void OnExecuted()
        {
            var session = CurrentSessionContext.Unbind(SessionFactory);
            var transaction = session.Transaction;

            if (transaction != null && transaction.IsActive)
            {
                try
                {
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }

            session.Close();
        }
        #endregion
    }
}