using System;

namespace Plateforme.AlloTabib.DomainLayer.Base.Classes
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
