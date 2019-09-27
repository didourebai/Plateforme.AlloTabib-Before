using System;

namespace Plateforme.AlloTabib.DomainLayer.Models.Base
{
    public class SessionFactoryIdentifierAttribute : Attribute
    {
        public string SessionFactoryIdentifire;

        public SessionFactoryIdentifierAttribute()
        { }

        public SessionFactoryIdentifierAttribute(string sessionFactoryKey)
        {
            SessionFactoryIdentifire = sessionFactoryKey;
        }
    }
}
