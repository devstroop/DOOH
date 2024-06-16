using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using System.Collections;
using Radzen.Blazor.Rendering;
using System.Text.Json.Nodes;

namespace DOOH.Client.Pages.Admin.Campaigns.Editor
{
    public partial class CampaignEditor
    {
        [Parameter]
        public int CampaignId { get; set; } = 0;


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


        protected bool CampaignNameEditable { get; set; } = false;


        protected async override Task OnParametersSetAsync()
        {
            if(campaign == null)
            {
                await LoadCampaign();
            }
        }

        private async Task LoadCampaign()
        {
            if (CampaignId > 0)
            {
                var confirm = await DialogService.Confirm("Are you sure you want to edit this campaign?", "Confirm", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
                if (confirm != true)
                {
                    DialogService.Close(null);
                }
                var result = DOOHDBService.GetCampaignByCampaignId(campaignId: CampaignId);
                campaign = result.Result;
            }
            else
            {
                var confirm = await DialogService.Confirm("Are you sure you want to create a new campaign?", "Confirm", new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
                if (confirm != true)
                {
                    DialogService.Close(null);
                }
                campaign = new DOOH.Server.Models.DOOHDB.Campaign();
                campaign.CampaignName = "Unnamed Campaign";
                campaign.IsDraft = true;
                campaign.Budget = 0;

                campaign = await DOOHDBService.CreateCampaign(campaign);
            }
        }

        protected bool errorVisible;
        protected DOOH.Server.Models.DOOHDB.Campaign campaign;

        protected int selectedTabIndex { get; set; } = 0;

        protected async Task SaveCampaign(MouseEventArgs args)
        {
            try
            {
                //campaign.CampaignSchedules = campaignSchedules;
                var result = await DOOHDBService.CreateCampaign(campaign);
                DialogService.Close(campaign);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task Cancel(MouseEventArgs args)
        {
            DialogService.Close(null);
        }


        [Inject]
        protected SecurityService Security { get; set; }


    }
}