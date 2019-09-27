using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Plateforme.AlloTabib.DomainLayer.Mapping;

namespace Plateforme.AlloTabib.InfrastructureLayer.SessionFactories
{
    public class BuildSessionFactoryForPostgres : IBuildSessionFactory
    {
        #region Public Methods

        /// <summary>
        /// Create new session factory for oracle provider.
        /// </summary>
        /// <returns></returns>
        public ISessionFactory BuildSessionFactory()
        {
            return Fluently.Configure()
                .Database(PostgreSQLConfiguration.PostgreSQL82
                .ConnectionString(c => c
                .Host("localhost")
                .Port(5432)
                .Database("allotabib")
                .Username("postgres")
                .Password("test")))
                .Mappings(m => m.FluentMappings
                .AddFromAssemblyOf<PatientMap>()
                ).CurrentSessionContext("call")
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Build the schema of the database.
        /// </summary>
        /// <param name="config">Configuration.</param>
        private static void BuildSchema(Configuration config)
        {
            new SchemaUpdate(config).Execute(false, true);
        }

        #endregion Private Methods
    }
}