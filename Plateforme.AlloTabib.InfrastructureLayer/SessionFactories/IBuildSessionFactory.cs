using NHibernate;

namespace Plateforme.AlloTabib.InfrastructureLayer.SessionFactories
{
    public interface IBuildSessionFactory
    {
        /// <summary>
        /// Create new session factory.
        /// </summary>
        /// <returns></returns>
        ISessionFactory BuildSessionFactory();
    }
}