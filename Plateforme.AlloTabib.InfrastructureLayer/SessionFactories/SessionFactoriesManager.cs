using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using Plateforme.AlloTabib.DomainLayer.Base.Classes;
using Plateforme.AlloTabib.DomainLayer.Base.Interfaces;

namespace Plateforme.AlloTabib.InfrastructureLayer.SessionFactories
{
    public class SessionFactoriesManager : ISessionFactoriesManager
    {

        #region Private attributes

        private IDictionary<string, ISessionFactory> _sessionFactoryDictionary;

        #endregion


        #region Constructors

        public SessionFactoriesManager()
        {
            _sessionFactoryDictionary = new Dictionary<string, ISessionFactory>();

        }
        #endregion

        public IDictionary<string, NHibernate.ISessionFactory> GetSessionFactories()
        {
            return _sessionFactoryDictionary;
        }

        public void AddSessionFactory(string sessionFactoryIdentifire, NHibernate.ISessionFactory sessionFactory)
        {
            _sessionFactoryDictionary.Add(sessionFactoryIdentifire, sessionFactory);
        }

        public NHibernate.ISessionFactory AddSessionFactory(string sessionFactoryIdentifire, string dbmsTypeAsString, string connectionStringName, List<Type> listOfEntityMapTypes, bool withLog = true)
        {
            var sessionFactory = SessionFactoryBuilder.BuildSessionFactory(dbmsTypeAsString, connectionStringName, listOfEntityMapTypes, withLog);
            _sessionFactoryDictionary.Add(sessionFactoryIdentifire, sessionFactory);
            return sessionFactory;
        }

        public void AddSessionFactoryForNamespaceOf<T>(NHibernate.ISessionFactory sessionFactory) where T : DomainLayer.Base.Classes.BasicEntity
        {
            var entityNamespace = typeof(T).Namespace;
            _sessionFactoryDictionary.Add(entityNamespace, sessionFactory);
        }

        public NHibernate.ISessionFactory AddSessionFactoryForNamespaceOf<E, M>(string dbmsTypeAsString, string connectionStringName, bool withLog = true, bool create = false, bool update = false)
            where E : DomainLayer.Base.Classes.BasicEntity
            where M : class
        {
            var entityNamespace = typeof(E).Namespace;
            var listOfEntityMap = typeof(M).Assembly.GetTypes().Where(t => String.Equals(t.Namespace, typeof(M).Namespace)).ToList();
            var sessionFactory = SessionFactoryBuilder.BuildSessionFactory(dbmsTypeAsString, connectionStringName, listOfEntityMap, withLog, create, update);
            _sessionFactoryDictionary.Add(entityNamespace, sessionFactory);
            return sessionFactory;
        }

        public NHibernate.ISessionFactory GetEntitySessionFactory<T>() where T : DomainLayer.Base.Classes.BasicEntity
        {
            var sessionfactoryIdentifire = typeof(T).Namespace;
            var sessionFactory = _sessionFactoryDictionary[sessionfactoryIdentifire];
            return sessionFactory;
        }

        #region Public Methods

        public string GetEntitySessionFactoryIdentifierAttribute<T>() where T : BasicEntity
        {
            var attribute = (SessionFactoryIdentifierAttribute)typeof(T).GetCustomAttributes(typeof(SessionFactoryIdentifierAttribute), false).FirstOrDefault();
            return attribute == null ? null : attribute.SessionFactoryIdentifire;
        }

        #endregion
    }
}
