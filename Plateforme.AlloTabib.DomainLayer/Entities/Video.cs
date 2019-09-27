using System;
using Plateforme.AlloTabib.DomainLayer.Base.Classes;

namespace Plateforme.AlloTabib.DomainLayer.Entities
{
    public class Video : BasicEntity
    {
        public virtual string Id { get; set; }
        public virtual string UrlYoutube { get; set; }
        public virtual string Titre { get; set; }
        public virtual string Auteur { get; set; }
        public virtual string Categorie { get; set; }


    }
}
