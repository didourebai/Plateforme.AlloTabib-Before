using System;

namespace Plateforme.AlloTabib.DomainLayer.Base.Classes
{
    class CriteriaAttribute : Attribute
    {
        /// <summary>
        /// A reference to the Entity property name, needed for building Nhibernate Restrictions and Projections
        /// </summary>
        public string PropertyName;

        /// <summary>
        /// Set to true if you want to order the results of the repository using this property.
        /// </summary>
        public bool IsDefaultOrderProperty;

        public CriteriaAttribute()
        {

        }
    }
}
