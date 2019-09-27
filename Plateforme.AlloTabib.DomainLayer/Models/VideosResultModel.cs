using System;
using System.Collections.Generic;

namespace Plateforme.AlloTabib.DomainLayer.Models
{
    public class VideosResultModel
    {
        public List<VideoDataModel> Items { get; set; }
        public PaginationHeader PaginationHeader { get; set; }

        public VideosResultModel ()
        {
            PaginationHeader = new PaginationHeader();
            Items = new List<VideoDataModel>();
        }

    }
}
