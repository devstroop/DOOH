using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System.Text.Json.Nodes;

namespace DOOH.Client.Pages.Admin.Campaigns.Editor
{
    public partial class CampaignAdvertisementsEditor
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected DOOH.Client.DOOHDBService DOOHDBService { get; set; }

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Advertisement> advertisements;

        protected int advertisementsCount;


        protected async Task advertisementsLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetAdvertisements(new Query { Top = args.Top, Skip = args.Skip, Filter = args.Filter, OrderBy = args.OrderBy });

                advertisements = result.Value.AsODataEnumerable();
                advertisementsCount = result.Count;
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load" });
            }
        }

        protected async Task OnAdMediaClick(MouseEventArgs args, DOOH.Server.Models.DOOHDB.Advertisement advertisement)
        {
            try
            {
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load" });
            }
        }
    }
}
