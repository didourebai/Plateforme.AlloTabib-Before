using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;
using System;

namespace Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.VideoAppServices
{
    public interface IVideoApplicationServices
    {
        ResultOfType<VideosResultModel> GetAllVideos(int take = 0, int skip = 0);
        ResultOfType<VideoDataModel> PostNewVideo(VideoDataModel video);
        ResultOfType<VideoDataModel> PatchVideo(VideoDataModel video);
        ResultOfType<VideoDataModel> DeleteVideo(VideoDataModel video);
    }
}
