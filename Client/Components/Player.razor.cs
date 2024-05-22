using Blazored.Video.Support;
using Blazored.Video;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace DOOH.Client.Components
{
    public partial class Player
    {
        [Parameter]
        public string Src { get; set; }

        [Inject]
        private DialogService DialogService { get; set; }


        Dictionary<VideoEvents, VideoStateOptions> options = new Dictionary<VideoEvents, VideoStateOptions>();
        VideoState videoState;
        string PausePlayText = "Pause";
        BlazoredVideo video;
        string id;

        [Inject]
        protected SecurityService Security { get; set; }


        protected override void OnInitialized()
        {
            var pausedOption = new VideoStateOptions() { Paused = true };
            options[VideoEvents.CanPlay] = pausedOption;
            options[VideoEvents.Ended] = pausedOption;
            options[VideoEvents.Pause] = pausedOption;
            options[VideoEvents.Play] = pausedOption;
        }
        void OnEvent(ref string PausePlayText, VideoState videoState)
        {
            id = videoState.Id;
            PausePlayText = videoState.Paused switch
            {
                true => "Play",
                _ => "Pause"
            };
            StateHasChanged();
        }

        async Task PlayOrPause(BlazoredVideo video)
        {
            if (await video.GetPausedAsync())
            {
                await video.StartPlayback();
            }
            else
            {
                await video.PausePlayback();
                DialogService.Close(id);
            }
        }
    }
}