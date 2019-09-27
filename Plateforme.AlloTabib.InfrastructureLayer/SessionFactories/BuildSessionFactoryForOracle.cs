using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Driver;
using NHibernate.SqlTypes;
using NHibernate.Tool.hbm2ddl;
using Plateforme.AlloTabib.DomainLayer.Mapping;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace Plateforme.AlloTabib.InfrastructureLayer.SessionFactories
{
    public class BuildSessionFactoryForOracle : IBuildSessionFactory
    {
        #region IBuildSessionFactory Methods

        /// <summary>
        /// Create new session factory for oracle provider.
        /// </summary>
        /// <returns></returns>
        public ISessionFactory BuildSessionFactory()
        {
            log4net.Config.XmlConfigurator.Configure();

            return Fluently.Configure()
                .Database(
                    OracleClientConfiguration.Oracle9.Driver<CustomOracleDriver>().ConnectionString(
                        ConfigurationManager.ConnectionStrings["axrg"].ConnectionString + " Omit Oracle Connection Name = true;"))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<PatientMap>())
                .CurrentSessionContext("call")
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }

        #endregion IBuildSessionFactory Methods

        #region Private Methods

        /// <summary>
        /// Build the schema of the database.
        /// </summary>
        /// <param name="config">Configuration.</param>
        private static void BuildSchema(Configuration config)
        {
            new SchemaUpdate(config).Execute(false, true);
        }

        /// <summary>
        /// Enable the insertion of Clob data.
        /// </summary>
        private class CustomOracleDriver : OracleClientDriver
        {
            protected override void InitializeParameter(System.Data.IDbDataParameter dbParam, string name, SqlType sqlType)
            {
                base.InitializeParameter(dbParam, name, sqlType);

                // System.Data.OracleClient.dll driver generates an ORA-01461 exception because
                // the driver mistakenly infers the column type of the string being saved, and
                // tries forcing the server to update a LONG value into a CLOB/NCLOB column type.
                // The reason for the incorrect behavior is even more obscure and only happens
                // when all the following conditions are met.
                //   1.) IDbDataParameter.Value = (string whose length: 4000 > length > 2000 )
                //   2.) IDbDataParameter.DbType = DbType.String
                //   3.) DB Column is of type NCLOB/CLOB

                // The above is the default behavior for NHibernate.OracleClientDriver
                // So we use the built-in StringClobSqlType to tell the driver to use the NClob Oracle type
                // This will work for both NCLOB/CLOBs without issues.
                // Mapping file must be updated to use StringClob as the property type
                // See: http://thebasilet.blogspot.be/2009/07/nhibernate-oracle-clobs.html
                if ((sqlType is StringClobSqlType))
                {
                    //((OracleParameter)dbParam).OracleType = OracleType.Clob; TODO
                }
            }
        }

        #endregion Private Methods
    }
}