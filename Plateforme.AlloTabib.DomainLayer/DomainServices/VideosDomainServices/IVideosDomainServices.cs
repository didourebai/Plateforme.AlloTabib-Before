using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;

namespace Plateforme.AlloTabib.DomainLayer.DomainServices.VideosDomainServices
{
    public interface IVideosDomainServices
    {
        ResultOfType<VideosResultModel> GetAllVideos(int take = 0, int skip = 0);
        ResultOfType<VideoDataModel> PostNewVideo(VideoDataModel video);
        ResultOfType<VideoDataModel> PatchVideo(VideoDataModel video);
        ResultOfType<VideoDataModel> DeleteVideo(VideoDataModel video);

    }
}
