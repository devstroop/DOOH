using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace DOOH.Client.Pages.Admin.Campaigns
{
    public partial class EditCampaign
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

        [Parameter]
        public int CampaignId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            campaign = await DOOHDBService.GetCampaignByCampaignId(campaignId:CampaignId);
        }
        protected bool errorVisible;
        protected DOOH.Server.Models.DOOHDB.Campaign campaign;

        protected async Task FormSubmit()
        {
            try
            {
                var result = await DOOHDBService.UpdateCampaign(campaignId:CampaignId, campaign);
                if (result != null)
                {
                    DialogService.Close(campaign);
                }
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