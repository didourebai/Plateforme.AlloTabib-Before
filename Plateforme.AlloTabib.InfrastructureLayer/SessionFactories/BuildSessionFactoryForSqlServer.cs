using System;
using FluentNHibernate.Cfg;
using NHibernate;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Plateforme.AlloTabib.DomainLayer.Mapping;

namespace Plateforme.AlloTabib.InfrastructureLayer.SessionFactories
{
    public class BuildSessionFactoryForSqlServer : IBuildSessionFactory
    {
        #region Public Methods
        /// <summary>
        /// Create new session factory for postgres provider.
        /// </summary>
        /// <returns></returns>
        public ISessionFactory BuildSessionFactory()
        {

            try
            {
                return Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2012
                        .ConnectionString(c => c.FromConnectionStringWithKey("DefaultConnection")))
                    .Mappings(m => m.FluentMappings
                        .AddFromAssemblyOf<PatientMap>()
                    ).CurrentSessionContext("call")
                    .ExposeConfiguration(BuildSchema)
                    .BuildSessionFactory();



            }
            catch (Exception ex)
            {
                var exception = ex;
                throw;
            }
        }

        #endregion
       


        #region Private Methods
        /// <summary>
        /// Build the schema of the database.
        /// </summary>
        /// <param name="config">Configuration.</param>
        private static void BuildSchema(Configuration config)
        {
            new SchemaUpdate(config).Execute(false, true);
            //new SchemaExport(config);//.Create(false, true);
        }
        #endregion

       
    }
}
