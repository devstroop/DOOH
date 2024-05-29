using Blazored.Video.Support;
using Blazored.Video;
using Microsoft.AspNetCore.Components;
using Radzen;
using Microsoft.JSInterop;

namespace DOOH.Client.Components
{
    public partial class Player
    {
        [Parameter]
        public string Src { get; set; }

        [Inject]
        private DialogService DialogService { get; set; }

        [Inject]
        private IJSRuntime JSRuntime { get; set; }


        Dictionary<VideoEvents, VideoStateOptions> options = new Dictionary<VideoEvents, VideoStateOptions>();
        VideoState videoState { get; set; }
        string PausePlayText = "Pause";
        BlazoredVideo video { get; set; }
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
            this.videoState = videoState;
            StateHasChanged();
        }

        //async Task PlayOrPause(BlazoredVideo video)
        //{
        //    if (await video.GetPausedAsync())
        //    {
        //        await video.StartPlayback();
        //    }
        //    else
        //    {
        //        await video.PausePlayback();
        //        DialogService.Close(id);
        //    }
        //}


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("videoHandler.initialize");
            }
        }
    }
}