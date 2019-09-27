using FluentNHibernate.Mapping;
using Plateforme.AlloTabib.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateforme.AlloTabib.DomainLayer.Mapping
{
    public class VideoMap : ClassMap<Video>
    {
        public VideoMap()
        {
            Id(x => x.Id)
               .GeneratedBy
               .Guid().Column("Id");
            Map(x => x.Auteur).Column("Auteur").Not.Nullable();
            Map(x => x.DateCreation).Column("DateCreation").Not.Nullable();
            Map(x => x.DateModification).Column("DateModification").Not.Nullable();
            Map(x => x.Titre).Column("Titre").Not.Nullable();
            Map(x => x.UrlYoutube).Column("UrlYoutube").Not.Nullable();
            Map(x => x.Categorie).Column("Categorie").Not.Nullable();

        }
    }
}
