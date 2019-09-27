using System;
using System.Collections.Generic;
using System.Configuration;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Plateforme.AlloTabib.DomainLayer.Mapping;
using Plateforme.AlloTabib.InfrastructureLayer.BaseContext.Enumerations;

namespace Plateforme.AlloTabib.InfrastructureLayer.SessionFactories
{
    public class SessionFactoryBuilder
    {

        public static ISessionFactory BuildSessionFactory(string dbmsTypeAsString, string connectionStringName, List<Type> entityMappingTypes, bool withLog = true, bool create = false, bool update = false)
        {
            var dbmsType = (EAvailableDBMS)Enum.Parse(typeof(EAvailableDBMS), dbmsTypeAsString);

            switch (dbmsType)
            {
                case EAvailableDBMS.Oracle9:
                    {
                        return BuildSessionFactoryForOracle9(connectionStringName, entityMappingTypes, withLog, create, update);
                    }
                case EAvailableDBMS.Oracle10:
                    {
                        return BuildSessionFactoryForOracle10(connectionStringName, entityMappingTypes, withLog, create, update);
                    }
                case EAvailableDBMS.SqlServer7:
                    {
                        return BuildSessionFactoryForSqlServer7(connectionStringName, entityMappingTypes, withLog, create, update);
                    }
                case EAvailableDBMS.SqlServer2000:
                    {
                        return BuildSessionFactoryForSqlServer2000(connectionStringName, entityMappingTypes, withLog, create, update);
                    }
                case EAvailableDBMS.SqlServer2005:
                    {
                        return BuildSessionFactoryForSqlServer2005(connectionStringName, entityMappingTypes, withLog, create, update);
                    }
                case EAvailableDBMS.SqlServer2008:
                    {
                        return BuildSessionFactoryForSqlServer2008(connectionStringName, entityMappingTypes, withLog, create, update);
                    }
                case EAvailableDBMS.SqlServer2012:
                    {
                        return BuildSessionFactoryForSqlServer2012(connectionStringName, entityMappingTypes, withLog, create, update);
                    }
                case EAvailableDBMS.MySql:
                    {
                        return BuildSessionFactoryForMySql(connectionStringName, entityMappingTypes, withLog, create, update);
                    }
                case EAvailableDBMS.PostgreSQL:
                    {
                        return BuildSessionFactoryForPostgreSQL(connectionStringName, entityMappingTypes, withLog, create, update);
                    }
                case EAvailableDBMS.PostgreSQL81:
                    {
                        return BuildSessionFactoryForPostgreSQL81(connectionStringName, entityMappingTypes, withLog, create, update);
                    }
                case EAvailableDBMS.PostgreSQL82:
                    {
                        return BuildSessionFactoryForPostgreSQL82(connectionStringName, entityMappingTypes, withLog, create, update);
                    }
                case EAvailableDBMS.SQLite:
                    {
                        return BuildSessionFactoryForSqLite(connectionStringName, entityMappingTypes, withLog, create, update);
                    }
                default:
                    {
                        throw new NotSupportedException(String.Format("The DBMS Type {0} is not supported.", dbmsTypeAsString));
                    }
            }
            throw new NotImplementedException();
        }

        #region Private Methods

        private static ISessionFactory BuildSessionFactoryForOracle9(string connectionStringName, List<Type> entityMappingTypes, bool withLog = true, bool create = false, bool update = false)
        {
            if (withLog)
                log4net.Config.XmlConfigurator.Configure();
            return Fluently.Configure()
                .Database(OracleClientConfiguration.Oracle9
                .ConnectionString(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
                .Mappings(m => entityMappingTypes.ForEach(e => { m.FluentMappings.Add(e); }))
                .CurrentSessionContext("call")
                .ExposeConfiguration(cfg => BuildSchema(cfg, create, update))
                .BuildSessionFactory();
        }

        private static ISessionFactory BuildSessionFactoryForOracle10(string connectionStringName, List<Type> entityMappingTypes, bool withLog = true, bool create = false, bool update = false)
        {
            if (withLog)
                log4net.Config.XmlConfigurator.Configure();

            return Fluently.Configure()
                .Database(OracleClientConfiguration.Oracle10
                .ConnectionString(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
                .Mappings(m => entityMappingTypes.ForEach(e => { m.FluentMappings.Add(e); }))
                .CurrentSessionContext("call")
                .ExposeConfiguration(cfg => BuildSchema(cfg, create, update))
                .BuildSessionFactory();
        }

        private static ISessionFactory BuildSessionFactoryForMySql(string connectionStringName, List<Type> entityMappingTypes, bool withLog = true, bool create = false, bool update = false)
        {
            if (withLog)
                log4net.Config.XmlConfigurator.Configure();

            return Fluently.Configure()
                .Database(MySQLConfiguration.Standard
                .ConnectionString(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
                .Mappings(m => entityMappingTypes.ForEach(e => { m.FluentMappings.Add(e); }))
                .CurrentSessionContext("call")
                .ExposeConfiguration(cfg => BuildSchema(cfg, create, update))
                .BuildSessionFactory();
        }

        private static ISessionFactory BuildSessionFactoryForPostgreSQL(string connectionStringName, List<Type> entityMappingTypes, bool withLog = true, bool create = false, bool update = false)
        {
            return Fluently.Configure()
                .Database(PostgreSQLConfiguration.Standard
                .ConnectionString(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
                .Mappings(m => entityMappingTypes.ForEach(e => { m.FluentMappings.Add(e); }))
                .CurrentSessionContext("call")
                .ExposeConfiguration(cfg => BuildSchema(cfg, create, update))
                .BuildSessionFactory();
        }

        private static ISessionFactory BuildSessionFactoryForPostgreSQL81(string connectionStringName, List<Type> entityMappingTypes, bool withLog = true, bool create = false, bool update = false)
        {
            return Fluently.Configure()
                .Database(PostgreSQLConfiguration.PostgreSQL81
                .ConnectionString(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
                .Mappings(m => entityMappingTypes.ForEach(e => { m.FluentMappings.Add(e); }))
                .CurrentSessionContext("call")
                .ExposeConfiguration(cfg => BuildSchema(cfg, create, update))
                .BuildSessionFactory();
        }

        private static ISessionFactory BuildSessionFactoryForPostgreSQL82(string connectionStringName, List<Type> entityMappingTypes, bool withLog = true, bool create = false, bool update = false)
        {
            return Fluently.Configure()
                .Database(PostgreSQLConfiguration.PostgreSQL82
                .ConnectionString(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
                .Mappings(m => entityMappingTypes.ForEach(e => { m.FluentMappings.Add(e); }))
                .CurrentSessionContext("call")
                .ExposeConfiguration(cfg => BuildSchema(cfg, create, update))
                .BuildSessionFactory();
        }

        private static ISessionFactory BuildSessionFactoryForSqLite(string connectionStringName, List<Type> entityMappingTypes, bool withLog = true, bool create = false, bool update = false)
        {
            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard
               .ConnectionString(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
                .Mappings(m => entityMappingTypes.ForEach(e => { m.FluentMappings.Add(e); }))
                .CurrentSessionContext("call")
                .ExposeConfiguration(cfg => BuildSchema(cfg, create, update))
                .BuildSessionFactory();
        }

        private static ISessionFactory BuildSessionFactoryForSqlServer7(string connectionStringName, List<Type> entityMappingTypes, bool withLog = true, bool create = false, bool update = false)
        {
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql7.ConnectionString(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
                .Mappings(m => entityMappingTypes.ForEach(e => { m.FluentMappings.Add(e); }))
                .CurrentSessionContext("call")
                .ExposeConfiguration(cfg => BuildSchema(cfg, create, update))
                .BuildSessionFactory();
        }

        private static ISessionFactory BuildSessionFactoryForSqlServer2000(string connectionStringName, List<Type> entityMappingTypes, bool withLog = true, bool create = false, bool update = false)
        {
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2000.ConnectionString(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
                .Mappings(m => entityMappingTypes.ForEach(e => { m.FluentMappings.Add(e); }))
                .CurrentSessionContext("call")
                .ExposeConfiguration(cfg => BuildSchema(cfg, create, update))
                .BuildSessionFactory();
        }

        private static ISessionFactory BuildSessionFactoryForSqlServer2005(string connectionStringName, List<Type> entityMappingTypes, bool withLog = true, bool create = false, bool update = false)
        {
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2005.ConnectionString(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
                .Mappings(m => entityMappingTypes.ForEach(e => { m.FluentMappings.Add(e); }))
                .CurrentSessionContext("call")
                .ExposeConfiguration(cfg => BuildSchema(cfg, create, update))
                .BuildSessionFactory();
        }

        private static ISessionFactory BuildSessionFactoryForSqlServer2008(string connectionStringName, List<Type> entityMappingTypes, bool withLog = true, bool create = false, bool update = false)
        {
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
                .Mappings(m => entityMappingTypes.ForEach(e => { m.FluentMappings.Add(e); }))
                .CurrentSessionContext("call")
                .ExposeConfiguration(cfg => BuildSchema(cfg, create, update))
                .BuildSessionFactory();
        }

        private static ISessionFactory BuildSessionFactoryForSqlServer2012(string connectionStringName, List<Type> entityMappingTypes, bool withLog = true, bool create = false, bool update = false)
        {
            return Fluently.Configure()

                 //.Database(MsSqlConfiguration.MsSql2012
                 //       .ConnectionString(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
                 //   .Mappings(m => m.FluentMappings
                 //       .AddFromAssemblyOf<PatientMap>()
                 //   ).CurrentSessionContext("call")
                 //   .ExposeConfiguration(BuildSchema)
                 //   .BuildSessionFactory();


                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString))
                .Mappings(m => entityMappingTypes.ForEach(e => { m.FluentMappings.Add(e); }))
                .CurrentSessionContext("call")
                .ExposeConfiguration(cfg => BuildSchema(cfg, create, update))
                .BuildSessionFactory();
        }

        /// <summary>
        /// Build the schema of the database.
        /// </summary>
        /// <param name="config">Configuration.</param>
        private static void BuildSchema(NHibernate.Cfg.Configuration config, bool create = false, bool update = false)
        {
            if (create)
            {
                new SchemaExport(config).Create(false, true);
            }
            else
            {
                new SchemaUpdate(config).Execute(false, update);
            }
        }

        #endregion
    }
}
