using Plateforme.AlloTabib.DomainLayer.DomainServices.VideosDomainServices;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.VideoAppServices
{
    public class VideoApplicationServices : IVideoApplicationServices
    {
        #region Private Properties

        private readonly IVideosDomainServices _videoDomainServices;

        #endregion

        public VideoApplicationServices(IVideosDomainServices videoDomainServices)
        {
            if (videoDomainServices == null)
                throw new ArgumentNullException("videoDomainServices");
            _videoDomainServices = videoDomainServices;

        }

        public ResultOfType<VideosResultModel> GetAllVideos(int take = 0, int skip = 0)
        {
           return  _videoDomainServices.GetAllVideos(take, skip);
        }

        public ResultOfType<VideoDataModel> PostNewVideo(VideoDataModel video)
        {
            return _videoDomainServices.PostNewVideo(video);
        }

        public ResultOfType<VideoDataModel> PatchVideo(VideoDataModel video)
        {
            return _videoDomainServices.PatchVideo(video);
        }

        public ResultOfType<VideoDataModel> DeleteVideo(VideoDataModel video)
        {
            return _videoDomainServices.DeleteVideo(video);
        }
    }
}
