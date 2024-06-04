using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace DOOH.Client.Pages
{
    public partial class AddCampaignAdboard
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }
        [Inject]
        public DOOHDBService DOOHDBService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            campaignAdboard = new DOOH.Server.Models.DOOHDB.CampaignAdboard();
        }
        protected bool errorVisible;
        protected DOOH.Server.Models.DOOHDB.CampaignAdboard campaignAdboard;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Campaign> campaignsForCampaignId;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Adboard> adboardsForAdboardId;


        protected int campaignsForCampaignIdCount;
        protected DOOH.Server.Models.DOOHDB.Campaign campaignsForCampaignIdValue;
        protected async Task campaignsForCampaignIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetCampaigns(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(CampaignName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                campaignsForCampaignId = result.Value.AsODataEnumerable();
                campaignsForCampaignIdCount = result.Count;

                if (!object.Equals(campaignAdboard.CampaignId, null))
                {
                    var valueResult = await DOOHDBService.GetCampaigns(filter: $"CampaignId eq {campaignAdboard.CampaignId}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        campaignsForCampaignIdValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Campaign" });
            }
        }

        protected int adboardsForAdboardIdCount;
        protected DOOH.Server.Models.DOOHDB.Adboard adboardsForAdboardIdValue;
        protected async Task adboardsForAdboardIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetAdboards(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(DisplaySerial, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                adboardsForAdboardId = result.Value.AsODataEnumerable();
                adboardsForAdboardIdCount = result.Count;

                if (!object.Equals(campaignAdboard.AdboardId, null))
                {
                    var valueResult = await DOOHDBService.GetAdboards(filter: $"AdboardId eq {campaignAdboard.AdboardId}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        adboardsForAdboardIdValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Adboard" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                var result = await DOOHDBService.CreateCampaignAdboard(campaignAdboard);
                DialogService.Close(campaignAdboard);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }



        [Inject]
        protected SecurityService Security { get; set; }
    }
}