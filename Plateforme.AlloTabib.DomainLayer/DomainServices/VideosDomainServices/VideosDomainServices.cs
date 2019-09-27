using Plateforme.AlloTabib.CrossCuttingLayer.Logging;
using Plateforme.AlloTabib.DomainLayer.Entities;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;
using PlateformeAlloTabib.Standards.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Plateforme.AlloTabib.DomainLayer.Base.Interfaces;

namespace Plateforme.AlloTabib.DomainLayer.DomainServices.VideosDomainServices
{
    public class VideosDomainServices : IVideosDomainServices
    {
        #region Private attributes

        private readonly IRepository<Video> _videoRepository;

        #endregion

        public VideosDomainServices(IRepository<Video> videoRepository)
        {
            if(videoRepository == null)
                throw new ArgumentNullException("videoRepository");
            _videoRepository = videoRepository;
        }

        /// <summary>
        /// GetAllVideos
        /// </summary>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        public ResultOfType<VideosResultModel> GetAllVideos(int take = 0, int skip = 0)
        {
            try
            {
                Logger.LogInfo("Get the list of videos With Take And Skip Parameters : Start.");

                var absolutTotalCount = _videoRepository.GetCount();

                var totalCount = _videoRepository.GetCount();
                var totalPages = (take != 0) ? (int)Math.Ceiling((double)totalCount / take) : 0;

                var paginationHeader = new PaginationHeader
                {
                    AbsolutTotalCount = absolutTotalCount,
                    TotalCount = totalCount,
                    TotalPages = totalPages
                };

                var videos = (take == 0 && skip == 0)
                                   ? _videoRepository
                                        .GetAll()
                                        .ToList()
                                        .OrderBy(a => a.UrlYoutube)
                                        .ToList()
                                   : _videoRepository
                                        .GetAll()
                                        .OrderBy(a => a.CreationDate)
                                        .Skip(skip)
                                        .Take(take)
                                        .ToList();


                var data = new VideosResultModel();

                videos.ForEach(video =>
                {
                    var dataModel = new VideoDataModel
                    {
                        Auteur = video.Auteur,
                        Catégorie = video.Categorie,
                        DateCreation = video.CreationDate.ToShortDateString(),
                        DateModification = video.LastModificationDate.ToShortDateString(),
                        Titre = video.Titre,
                        UrlYoutube = video.UrlYoutube
                    };
                    data.Items.Add(dataModel);
                });

                data.PaginationHeader = paginationHeader;

                Logger.LogInfo("Get Video With Take And Skip Parameters : End --- Status : OK");
                return new Return<VideosResultModel>().OK().WithResult(data);
            }
            catch(Exception ex)
            {
                Logger.LogError("Get videos Exception", ex);
                throw;
            }
        }

        public ResultOfType<VideoDataModel> PostNewVideo(VideoDataModel videoToAdd)
        {
            try
            {
                Logger.LogInfo("PostNewVideo : Start.");

                if (videoToAdd == null)
                    return new Return<VideoDataModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                        null, "Les données sont vides.").WithDefaultResult();
                Logger.LogInfo(string.Format("Post New Video : Start --- Url = {0}, Auteur = {1}",
                                               videoToAdd.UrlYoutube, videoToAdd.Auteur));

                if (string.IsNullOrEmpty(videoToAdd.Auteur))
                    return new Return<VideoDataModel>()
                       .Error().AsValidationFailure(null, "Veuillez déterminer l'auteur.", "Auteur")
                       .WithDefaultResult();


                if (string.IsNullOrEmpty(videoToAdd.Catégorie))
                    return new Return<VideoDataModel>()
                       .Error().AsValidationFailure(null, "Veuillez déterminer votre catégorie.", "Catégorie")
                       .WithDefaultResult();


                if (string.IsNullOrEmpty(videoToAdd.Titre))
                    return new Return<VideoDataModel>()
                       .Error().AsValidationFailure(null, "Veuillez déterminer le tite.", "Titre")
                       .WithDefaultResult();

                if (string.IsNullOrEmpty(videoToAdd.UrlYoutube))
                    return new Return<VideoDataModel>()
                       .Error().AsValidationFailure(null, "Veuillez déterminer l'url de la vidéo.", "UrlYoutube")
                       .WithDefaultResult();

                var video = new Video
                {
                    UrlYoutube = videoToAdd.UrlYoutube,
                    Titre = videoToAdd.Titre,
                    Categorie = videoToAdd.Catégorie,
                    Auteur = videoToAdd.Auteur
                };


                _videoRepository.Add(video);
                Logger.LogInfo("Post New Vidéo : End --- Status = OK, Message= {1}");

                return new Return<VideoDataModel>().OK().WithResult(videoToAdd);
            }
            catch(Exception ex)
            {
                Logger.LogError("Post video Exception", ex);
                throw;
            }
        }

        public ResultOfType<VideoDataModel> PatchVideo(VideoDataModel video)
        {
            throw new NotImplementedException();
        }

        public ResultOfType<VideoDataModel> DeleteVideo(VideoDataModel video)
        {
            Logger.LogInfo("DeleteVideo : Start.");

            if( video == null )
                return new Return<VideoDataModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                    null, "Veuillez introduire un cin ou patient possédant un cin vide !!!!").WithDefaultResult();
            Logger.LogInfo(string.Format("Delete patient : Start ---"));

            try
            {
                //get video
                var videoToDelete = _videoRepository.GetAll().FirstOrDefault(v => v.UrlYoutube.Equals(video.UrlYoutube) && v.Titre.Equals(video.Titre));
                _videoRepository.Delete(videoToDelete.Id);
                return new Return<VideoDataModel>().OK().WithResult(video);
            }
            catch (Exception ex)
            {
                Logger.LogInfo(string.Format("Delete patient : end error --- Exception = {0}", ex.Message));
                return new Return<VideoDataModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                    null, "Erreur suite à une exception avec notre serveur.").WithDefaultResult();

            }

        }
    }
}
