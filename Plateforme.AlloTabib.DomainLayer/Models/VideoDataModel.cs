using System;

namespace Plateforme.AlloTabib.DomainLayer.Models
{
    public class VideoDataModel
    {
        public virtual string UrlYoutube { get; set; }
        public virtual string Titre { get; set; }
        public virtual string Auteur { get; set; }
        public virtual string Catégorie { get; set; }
        public virtual string DateCreation { get; set; }
        public virtual string DateModification { get; set; }
    }
}
