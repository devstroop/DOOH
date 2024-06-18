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
        public DOOH.Server.Models.DOOHDB.Campaign Campaign { get; set; }
        private int CampaignId { get; set; } = 0;
        private string CampaignName { get; set; } = "Unnamed Campaign";
        private int BudgetType { get; set; } = 1;
        private decimal Budget { get; set; } = 0;
        private bool IsDraft { get; set; } = true;
        private bool Continuous { get; set; } = true;
        private DateTime StartDate { get; set; } = DateTime.Today;
        private DateTime? EndDate { get; set; }


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


        protected override Task OnParametersSetAsync()
        {
            if (Campaign != null)
            {
                CampaignId = Campaign.CampaignId;
                CampaignName = Campaign.CampaignName;
                StartDate = Campaign.StartDate;
                EndDate = Campaign.EndDate;
                Continuous = Campaign.EndDate == null;
                Budget = Campaign.Budget;
                BudgetType = Campaign.BudgetType;
                IsDraft = Campaign.IsDraft;
            }
            return base.OnParametersSetAsync();
        }

        protected bool errorVisible;

        protected int selectedTabIndex { get; set; } = 0;

        protected async Task SaveCampaign(MouseEventArgs args)
        {
            try
            {
                Campaign.CampaignId = CampaignId;
                Campaign.CampaignName = CampaignName;
                Campaign.StartDate = StartDate;
                Campaign.EndDate = Continuous ? null : EndDate;
                Campaign.Budget = Budget;
                Campaign.BudgetType = BudgetType;
                Campaign.IsDraft = IsDraft;
                if (CampaignId == 0)
                {
                    var result = await DOOHDBService.CreateCampaign(Campaign);
                    if(result != null)
                    {
                        Campaign.CampaignId = result.CampaignId;
                        DialogService.Close(Campaign);
                    }
                }
                else
                {
                    var result = await DOOHDBService.UpdateCampaign(Campaign.CampaignId, Campaign);
                    if (result != null)
                    {
                        DialogService.Close(Campaign);
                    }
                }
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