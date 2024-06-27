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
        // private bool Continuous { get; set; } = true;
        private DateTime StartDate { get; set; } = DateTime.Today;
        private DateTime EndDate { get; set; } = DateTime.Today.AddDays(30);
        private int StatusId { get; set; }
        
        private IList<int> _selectedAdboardIds = new List<int>();
        private IEnumerable<Advertisement> _advertisements = new List<Advertisement>();

        private DOOH.Server.Models.DOOHDB.Campaign _campaign;

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
                    _campaign = await DoohdbService.GetCampaignByCampaignId(campaignId: CampaignIdInt, expand: "CampaignAdboards($expand=Adboard),Advertisements($expand=Upload)");
                    if (_campaign != null)
                    {
                        CampaignId = _campaign.CampaignId.ToString();
                        CampaignName = _campaign.CampaignName;
                        StartDate = _campaign.StartDate;
                        EndDate = _campaign.EndDate;
                        Budget = _campaign.Budget;
                        BudgetType = (BudgetType)_campaign.BudgetType;
                        IsDraft = _campaign.IsDraft;
                        StatusId = _campaign.StatusId ?? 0;
                        _selectedAdboardIds = _campaign.CampaignAdboards.Select(ca => ca.AdboardId).ToList();
                        _advertisements = _campaign.Advertisements;
                    }
                }
            }
            finally
            {
                IsSaving = false;
                StateHasChanged();
            }
        }

        private int _selectedTabIndex = 0;


        private async Task SaveCampaignAdboards()
        {
            var existing = _campaign.CampaignAdboards.Select(ca => ca.AdboardId).ToList();
            var toAdd = _selectedAdboardIds.Except(existing).ToList();
            var toRemove = existing.Except(_selectedAdboardIds).ToList();
            
            foreach (var adboardId in toAdd)
            {
                var campaignAdboard = new DOOH.Server.Models.DOOHDB.CampaignAdboard();
                campaignAdboard.CampaignId = _campaign.CampaignId;
                campaignAdboard.AdboardId = adboardId;
                await DoohdbService.CreateCampaignAdboard(campaignAdboard);
            }
            
            foreach (var adboardId in toRemove)
            {
                var campaignAdboard = _campaign.CampaignAdboards.FirstOrDefault(ca => ca.AdboardId == adboardId);
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
                // var confirmariotnResult = await DialogService.Confirm("Are you sure you want to save?");
                // if (confirmariotnResult != true)
                // {
                //     return;
                // }
                
                // OpenDialog Review
                var reviewDialog = await DialogService.OpenAsync<Review>("Review", new Dictionary<string, object>() {{"CampaignId", CampaignIdInt}}, new DialogOptions()
                {
                    Width = "100%",
                    Height = "100%",
                });
                if (reviewDialog != true)
                {
                    return;
                }
                
                IsSaving = true;
                StateHasChanged();

                _campaign = _campaign ?? new DOOH.Server.Models.DOOHDB.Campaign();
                _campaign.CampaignId = CampaignIdInt;
                _campaign.CampaignName = CampaignName;
                _campaign.StartDate = StartDate;
                _campaign.EndDate = EndDate;
                _campaign.Budget = Budget;
                _campaign.BudgetType = (int)BudgetType;
                _campaign.IsDraft = IsDraft;
                _campaign.StatusId = StatusId;
                
                if (_campaign.CampaignId == 0)
                {
                    var result = await DoohdbService.CreateCampaign(_campaign);
                    if(result != null)
                    {
                        _campaign.CampaignId = result.CampaignId;
                        await SaveCampaignAdboards();
                        NavigationManager.NavigateTo("admin/campaigns");
                    }
                }
                else
                {
                    var result = await DoohdbService.UpdateCampaign(_campaign.CampaignId, _campaign);
                    if (result != null)
                    {
                        await SaveCampaignAdboards();
                        NavigationManager.NavigateTo("admin/campaigns");
                    }
                }
            }
            finally
            {
                IsSaving = false;
                StateHasChanged();
            }
        }

        private async Task Cancel(MouseEventArgs args)
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


        private async Task statusesLoadData(LoadDataArgs args)
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


        private async Task OnAddAdboard(int adboardId)
        {
            if (!_selectedAdboardIds.Contains(adboardId))
            {
                _selectedAdboardIds.Add(adboardId);
            }
        }

        private async Task OnRemoveAdboard(int adboardId)
        {
            if (_selectedAdboardIds.Contains(adboardId))
            {
                _selectedAdboardIds.Remove(adboardId);
            }
        }

        private async Task OnClearAdboards()
        {
            _selectedAdboardIds.Clear();
        }
        
        // OnRefresh
        private void OnRefresh(){
            StateHasChanged();
        }
        
        // OnDeleteAdvertisement
        private async Task OnDeleteAdvertisement(int advertisementId)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this advertisement?") == true)
                {
                    var result = await DoohdbService.DeleteAdvertisement(advertisementId);
                    if (result != null)
                    {
                        NotificationService.Notify(new NotificationMessage
                        {
                            Severity = NotificationSeverity.Success, Summary = "Success",
                            Detail = "Advertisement deleted successfully"
                        });
                    }
                }
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage
                    { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to delete" });
            }
            finally
            {
                await LoadCampaign();
            }
        }
        
        private async Task OnAddAdvertisement(Advertisement advertisement)
        {
            await LoadCampaign();
        }
    }
}