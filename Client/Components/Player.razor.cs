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
        
        [Parameter]
        public RenderFragment Overlay { get; set; }

        [Inject]
        private DialogService DialogService { get; set; }

        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        private Dictionary<VideoEvents, VideoStateOptions> videoEventOptions = new();
        private VideoState currentVideoState { get; set; }
        private string pausePlayButtonText = "Pause";
        private BlazoredVideo videoPlayer { get; set; }
        private string videoId;

        [Inject]
        protected SecurityService SecurityService { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            var pausedOption = new VideoStateOptions() { Paused = true };
            videoEventOptions[VideoEvents.CanPlay] = pausedOption;
            videoEventOptions[VideoEvents.Ended] = pausedOption;
            videoEventOptions[VideoEvents.Pause] = pausedOption;
            videoEventOptions[VideoEvents.Play] = pausedOption;
        }

        private void HandleVideoEvent(VideoState videoState)
        {
            videoId = videoState.Id;
            pausePlayButtonText = videoState.Paused ? "Play" : "Pause";
            currentVideoState = videoState;
            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                
            }
        }
    }
}