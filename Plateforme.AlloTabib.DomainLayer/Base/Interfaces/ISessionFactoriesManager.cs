using System;
using System.Collections.Generic;
using NHibernate;
using Plateforme.AlloTabib.DomainLayer.Base.Classes;

namespace Plateforme.AlloTabib.DomainLayer.Base.Interfaces
{
    public interface ISessionFactoriesManager
    {
        /// <summary>
        /// Gets the dictionary of the list of session factories.
        /// </summary>
        /// <returns></returns>
        IDictionary<string, ISessionFactory> GetSessionFactories();

        /// <summary>
        /// Adds a session factory to the list of managed session factories.
        /// </summary>
        /// <param name="sessionFactoryIdentifire">The unique identifier of the new session factory.</param>
        /// <param name="sessionFactory">The new session factory.</param>
        void AddSessionFactory(string sessionFactoryIdentifire, ISessionFactory sessionFactory);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionFactoryIdentifire"></param>
        /// <param name="dbmsTypeAsString"></param>
        /// <param name="connectionStringName"></param>
        /// <param name="listOfEntityMapTypes"></param>
        /// <param name="withLog"></param>
        /// <returns></returns>
        ISessionFactory AddSessionFactory(string sessionFactoryIdentifire, string dbmsTypeAsString, string connectionStringName, List<Type> listOfEntityMapTypes, bool withLog = true);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sessionFactory"></param>
        void AddSessionFactoryForNamespaceOf<T>(ISessionFactory sessionFactory) where T : BasicEntity;

        /// <summary>
        /// Adds a session factory for a specific Entities namespace and for a specific Entities Mapping namespace.
        /// </summary>
        /// <typeparam name="E">An entity class from the specific Entities namespace.</typeparam>
        /// <typeparam name="M">An entity mapping class from the specific Entities Mapping namespace.</typeparam>
        /// <param name="dbmsTypeAsString">The type of the Data Base Management System.</param>
        /// <param name="connectionStringName">The name of the connection string to use.</param>
        /// <param name="withLog">Configures the use of log.</param>
        /// <param name="create">Creates the DB Schema if true.</param>
        /// <param name="update">Updates the DB Schema if true.</param>
        /// <returns></returns>
        ISessionFactory AddSessionFactoryForNamespaceOf<E, M>(string dbmsTypeAsString, string connectionStringName, bool withLog = true, bool create = false, bool update = false)
            where E : BasicEntity
            where M : class;

        /// <summary>
        /// Gets the appropriate session factory to use for a specific entity.
        /// </summary>
        /// <typeparam name="T">The type of the Entity.</typeparam>
        /// <returns>The appropriate session factory.</returns>
        ISessionFactory GetEntitySessionFactory<T>() where T : BasicEntity;
    }
}
