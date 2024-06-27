using DOOH.Client.Components;
using DOOH.Client.Extensions;
using DOOH.Client.Templates;
using DOOH.Server.Models.DOOHDB;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace DOOH.Client.Pages.Admin.Campaigns.Editor
{
    public partial class ReviewPublish
    {
        [Parameter]
        public DOOH.Server.Models.DOOHDB.Campaign Campaign { get; set; }
        [Parameter]
        public IEnumerable<DOOH.Server.Models.DOOHDB.Adboard> Adboards { get; set; }
        [Parameter]
        public IEnumerable<DOOH.Server.Models.DOOHDB.Advertisement> Advertisements { get; set; }
        [Parameter]
        public IEnumerable<DOOH.Server.Models.DOOHDB.CampaignSchedule> Schedules { get; set; }
        
        
        
        
        [Inject]
        private DOOHDBService DoohdbService { get; set; }
        
        [Inject]
        private DialogService DialogService { get; set; }
        
        [Inject]
        private NotificationService NotificationService { get; set; }
        
        
        
        private IEnumerable<string> Images => Campaign.Advertisements?.Select(x => x.Upload.GetThumbnail()) ?? new List<string>();
    
        private int _currentIndex = 0;
        private string CurrentImage => Images.ElementAtOrDefault(_currentIndex);
        private bool IsCurrentImageVisible => !string.IsNullOrEmpty(CurrentImage);
        private bool IsPreviousButtonVisible => _currentIndex > 0;
        private bool IsNextButtonVisible => Images.Count() - 1 > _currentIndex;


        private void NextImage()
        {
            _currentIndex = (_currentIndex + 1) % Images.Count();
        }

        private void PreviousImage()
        {
            _currentIndex = (_currentIndex - 1 + Images.Count()) % Images.Count();
        }
        
        

        private async Task OnHoverClick(MouseEventArgs args, string image)
        {
            var advertisement = Campaign.Advertisements.Where(x => CurrentImage.Contains(x.Upload.Thumbnail)).FirstOrDefault();
            if (advertisement != null)
            {
                var url = advertisement.Upload.GetUrl();
                await DialogService.OpenAsync<Player>($"#{advertisement.AdvertisementId}", new Dictionary<string, object>() { { "Src", url } }, new DialogOptions() { Width = "400px" });
            }
            // NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Info, Summary = "Image", Detail = url });
        }
        
        
        private async Task OnLocationClick(MouseEventArgs args)
        {
            var markers = new List<Tuple<string, GoogleMapPosition>>();
            GoogleMapPosition firstPosition = null;
            foreach (var adboard in Campaign.CampaignAdboards)
            {
                var position = new GoogleMapPosition() { Lat = adboard?.Adboard?.Latitude ?? 0, Lng = adboard?.Adboard?.Longitude ?? 0 };
                markers.Add(new Tuple<string, GoogleMapPosition>($"#{adboard.AdboardId}", position));
                if (firstPosition == null)
                {
                    firstPosition = position;
                }
            }

 

            var parameters = new Dictionary<string, object>();
            parameters.Add("Markers", markers);
            parameters.Add("Center", firstPosition);
            parameters.Add("Zoom", 12);
            var options = new DialogOptions() { Width = "800px", Height = "600px", Draggable = true, CloseDialogOnOverlayClick = true, CloseDialogOnEsc = true };
            await DialogService.OpenAsync<GoogleMapTemplate>("Map", parameters, options);
        }


        // Submit, Cancel
        private async Task Submit()
        {
            DialogService.Close(true);
        }
        
        private async Task Cancel()
        {
            DialogService.Close(false);
        }

    }
}
