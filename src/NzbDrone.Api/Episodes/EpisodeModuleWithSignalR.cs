using System.Collections.Generic;
using NzbDrone.Api.EpisodeFiles;
using NzbDrone.Api.Series;
using NzbDrone.Common.Disk;
using NzbDrone.Common.Extensions;
using NzbDrone.Core.Configuration;
using NzbDrone.Core.Datastore.Events;
using NzbDrone.Core.DecisionEngine;
using NzbDrone.Core.DecisionEngine.Specifications;
using NzbDrone.Core.Download;
using NzbDrone.Core.MediaFiles.Events;
using NzbDrone.Core.Messaging.Events;
using NzbDrone.Core.Tv;
using NzbDrone.SignalR;
using Sonarr.Http;

namespace NzbDrone.Api.Episodes
{
    public abstract class EpisodeModuleWithSignalR : SonarrRestModuleWithSignalR<EpisodeResource, Episode>,
        IHandle<EpisodeGrabbedEvent>,
        IHandle<EpisodeImportedEvent>
    {
        protected readonly IEpisodeService _episodeService;
        protected readonly ISeriesService _seriesService;
        protected readonly IUpgradableSpecification _upgradableSpecification;
        protected readonly IConfigService _configService;
        protected readonly IDiskProvider _diskProvider;

        protected EpisodeModuleWithSignalR(IEpisodeService episodeService,
                                           ISeriesService seriesService,
                                           IUpgradableSpecification upgradableSpecification,
                                           IConfigService configService,
                                           IDiskProvider diskProvider,
                                           IBroadcastSignalRMessage signalRBroadcaster)
            : base(signalRBroadcaster)
        {
            _episodeService = episodeService;
            _seriesService = seriesService;
            _upgradableSpecification = upgradableSpecification;
            _configService = configService;
            _diskProvider = diskProvider;

            GetResourceById = GetEpisode;
        }

        protected EpisodeModuleWithSignalR(IEpisodeService episodeService,
                                           ISeriesService seriesService,
                                           IUpgradableSpecification upgradableSpecification,
                                           IConfigService configService,
                                           IDiskProvider diskProvider,
                                           IBroadcastSignalRMessage signalRBroadcaster,
                                           string resource)
            : base(signalRBroadcaster, resource)
        {
            _episodeService = episodeService;
            _seriesService = seriesService;
            _upgradableSpecification = upgradableSpecification;
            _configService = configService;
            _diskProvider = diskProvider;

            GetResourceById = GetEpisode;
        }

        protected EpisodeResource GetEpisode(int id)
        {
            var episode = _episodeService.GetEpisode(id);
            var resource = MapToResource(episode, true, true);
            return resource;
        }

        protected EpisodeResource MapToResource(Episode episode, bool includeSeries, bool includeEpisodeFile)
        {
            var resource = episode.ToResource();

            if (includeSeries || includeEpisodeFile)
            {
                var series = episode.Series ?? _seriesService.GetSeries(episode.SeriesId);

                if (includeSeries)
                {
                    resource.Series = series.ToResource();
                }
                if (includeEpisodeFile && episode.EpisodeFileId != 0)
                {
                    resource.EpisodeFile = episode.EpisodeFile.Value.ToResource(series, _upgradableSpecification);

                    if (_configService.CopyUsingSymlinks)
                    {
                        resource.EpisodeFile.Path = _diskProvider.GetRealPath(resource.EpisodeFile.Path);
                    }
                }
            }

            return resource;
        }

        protected List<EpisodeResource> MapToResource(List<Episode> episodes, bool includeSeries, bool includeEpisodeFile)
        {
            var result = episodes.ToResource();

            if (includeSeries || includeEpisodeFile)
            {
                var seriesDict = new Dictionary<int, Core.Tv.Series>();
                for (var i = 0; i < episodes.Count; i++)
                {
                    var episode = episodes[i];
                    var resource = result[i];

                    var series = episode.Series ?? seriesDict.GetValueOrDefault(episodes[i].SeriesId) ?? _seriesService.GetSeries(episodes[i].SeriesId);
                    seriesDict[series.Id] = series;
                    
                    if (includeSeries)
                    {
                        resource.Series = series.ToResource();
                    }
                    if (includeEpisodeFile && episodes[i].EpisodeFileId != 0)
                    {
                        resource.EpisodeFile = episodes[i].EpisodeFile.Value.ToResource(series, _upgradableSpecification);

                        if (_configService.CopyUsingSymlinks)
                        {
                            resource.EpisodeFile.Path = _diskProvider.GetRealPath(resource.EpisodeFile.Path);
                        }
                    }
                }
            }

            return result;
        }

        public void Handle(EpisodeGrabbedEvent message)
        {
            foreach (var episode in message.Episode.Episodes)
            {
                var resource = episode.ToResource();
                resource.Grabbed = true;

                BroadcastResourceChange(ModelAction.Updated, resource);
            }
        }

        public void Handle(EpisodeImportedEvent message)
        {
            if (!message.NewDownload)
            {
                return;
            }

            foreach (var episode in message.EpisodeInfo.Episodes)
            {
                BroadcastResourceChange(ModelAction.Updated, episode.Id);
            }
        }
    }
}
