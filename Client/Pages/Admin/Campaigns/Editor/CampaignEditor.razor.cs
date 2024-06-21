﻿using System;
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
using DOOH.Server.Models.DOOHDB;
using DOOH.Server.Models.Enums;

namespace DOOH.Client.Pages.Admin.Campaigns.Editor
{
    public partial class CampaignEditor
    {
        [Parameter]
        public object CampaignId { get; set; }
        private int CampaignIdInt => Convert.ToInt32(CampaignId);
        private string CampaignName { get; set; } = "Unnamed Campaign";
        private BudgetType BudgetType { get; set; } = BudgetType.Total;
        private decimal Budget { get; set; } = 0;
        private bool IsDraft { get; set; } = true;
        private bool IsSaving { get; set; } = false;
        private bool Continuous { get; set; } = true;
        private DateTime StartDate { get; set; } = DateTime.Today;
        private DateTime EndDate { get; set; } = DateTime.Today.AddDays(30);
        private int StatusId { get; set; }
        
        private IList<int> selectedAdboardIds = new List<int>();

        private DOOH.Server.Models.DOOHDB.Campaign campaign;
        

        [Inject]
        protected IJSRuntime JsRuntime { get; set; }

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
        public DOOHDBService DoohdbService { get; set; }


        protected bool CampaignNameEditable { get; set; } = false;


        protected override async Task OnInitializedAsync()
        {
            await LoadCampaign();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadCampaign();
            }
        }

        private async Task LoadCampaign()
        {
            try
            {
                if (CampaignIdInt != 0)
                {
                    campaign = await DoohdbService.GetCampaignByCampaignId(campaignId: CampaignIdInt, expand: "CampaignAdboards($expand=Adboard)");
                    if (campaign != null)
                    {
                        CampaignId = campaign.CampaignId.ToString();
                        CampaignName = campaign.CampaignName;
                        StartDate = campaign.StartDate;
                        EndDate = campaign.EndDate ?? DateTime.Today.AddDays(30);
                        Continuous = campaign.EndDate == null;
                        Budget = campaign.Budget;
                        BudgetType = (BudgetType)campaign.BudgetType;
                        IsDraft = campaign.IsDraft;
                        StatusId = campaign.StatusId ?? 0;
                        selectedAdboardIds = campaign.CampaignAdboards.Select(ca => ca.AdboardId).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
            finally
            {
                IsSaving = false;
                StateHasChanged();
            }
        }

        protected bool errorVisible;

        private int selectedTabIndex { get; set; } = 0;


        private async Task SaveCampaignAdboards()
        {
            var existing = campaign.CampaignAdboards.Select(ca => ca.AdboardId).ToList();
            var toAdd = selectedAdboardIds.Except(existing).ToList();
            var toRemove = existing.Except(selectedAdboardIds).ToList();
            
            foreach (var adboardId in toAdd)
            {
                var campaignAdboard = new DOOH.Server.Models.DOOHDB.CampaignAdboard();
                campaignAdboard.CampaignId = campaign.CampaignId;
                campaignAdboard.AdboardId = adboardId;
                await DoohdbService.CreateCampaignAdboard(campaignAdboard);
            }
            
            foreach (var adboardId in toRemove)
            {
                var campaignAdboard = campaign.CampaignAdboards.FirstOrDefault(ca => ca.AdboardId == adboardId);
                if (campaignAdboard != null)
                {
                    await DoohdbService.DeleteCampaignAdboard(campaignAdboard.CampaignId, campaignAdboard.AdboardId);
                }
            }
        }

        private async Task SaveCampaign(MouseEventArgs args)
        {
            try
            {
                var confirmariotnResult = await DialogService.Confirm("Are you sure you want to save?");
                if (confirmariotnResult != true)
                {
                    return;
                }
                
                IsSaving = true;
                StateHasChanged();

                campaign = campaign ?? new DOOH.Server.Models.DOOHDB.Campaign();
                campaign.CampaignId = CampaignIdInt;
                campaign.CampaignName = CampaignName;
                campaign.StartDate = StartDate;
                campaign.EndDate = Continuous ? null : EndDate;
                campaign.Budget = Budget;
                campaign.BudgetType = (int)BudgetType;
                campaign.IsDraft = IsDraft;
                campaign.StatusId = StatusId;
                
                if (campaign.CampaignId == 0)
                {
                    var result = await DoohdbService.CreateCampaign(campaign);
                    if(result != null)
                    {
                        campaign.CampaignId = result.CampaignId;
                        await SaveCampaignAdboards();
                        NavigationManager.NavigateTo("admin/campaigns");
                    }
                }
                else
                {
                    var result = await DoohdbService.UpdateCampaign(campaign.CampaignId, campaign);
                    if (result != null)
                    {
                        await SaveCampaignAdboards();
                        NavigationManager.NavigateTo("admin/campaigns");
                    }
                }
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
            finally
            {
                IsSaving = false;
                StateHasChanged();
            }
        }

        protected async Task Cancel(MouseEventArgs args)
        {
            var result = await DialogService.Confirm("Are you sure you want to cancel?");
            if (result == true)
            {
                NavigationManager.NavigateTo("admin/campaigns");
            }
        }


        [Inject]
        protected SecurityService Security { get; set; }

        
        private IEnumerable<DOOH.Server.Models.DOOHDB.Status> statuses;
        private int statusesCount;


        protected async Task statusesLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DoohdbService.GetStatuses(filter: $"{args.Filter}", orderby: $"{args.OrderBy}",
                    top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null);
                statuses = result.Value.AsODataEnumerable();
                statusesCount = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage()
                    { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Statuses" });
            }
        }


        protected async Task OnAddAdboard(int adboardId)
        {
            if (!selectedAdboardIds.Contains(adboardId))
            {
                selectedAdboardIds.Add(adboardId);
            }
        }
        protected async Task OnRemoveAdboard(int adboardId)
        {
            if (selectedAdboardIds.Contains(adboardId))
            {
                selectedAdboardIds.Remove(adboardId);
            }
        }
        protected async Task OnClearAdboards()
        {
            selectedAdboardIds.Clear();
        }
        
        // OnRefresh
        private void OnRefresh(){
            StateHasChanged();
        }
    }
}