using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plateforme.AlloTabib.DomainLayer.Mapping;

namespace Plateforme.AlloTabib.InfrastructureLayer.SessionFactories
{
    public class BuildSessionFactoryForSqLite : IBuildSessionFactory
    {
        #region Public Methods
        /// <summary>
        /// Create new session factory for sql lite.
        /// </summary>
        /// <returns></returns>
        public ISessionFactory BuildSessionFactory(bool withLog = true)
        {
            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard

               .ConnectionString(@"Data Source=D:\mydb.db;Version=3;")
                .ShowSql()
                )
                .Mappings(m => m.FluentMappings
                .AddFromAssemblyOf<ProductMap>()
                ).CurrentSessionContext("web")
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
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
