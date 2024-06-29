using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using DOOH.Server.Models.DOOHDB;
using DOOH.Server.Models.Enums;

namespace DOOH.Client.Pages.Admin.Campaigns.Editor
{
    public partial class CampaignEditor
    {
        [Parameter]
        public object CampaignId { get; set; }
        private string CampaignName { get; set; } = "Unnamed Campaign";
        private BudgetType BudgetType { get; set; } = BudgetType.Total;
        private decimal Budget { get; set; } = 0;
        private bool IsDraft { get; set; } = true;
        private bool IsSaving { get; set; } = false;
        private DateTime StartDate { get; set; } = DateTime.Today;
        private DateTime EndDate { get; set; } = DateTime.Today.AddDays(30);
        private DOOH.Server.Models.Enums.Status Status { get; set; } = DOOH.Server.Models.Enums.Status.Draft;
        
        
        private int _campaignId = 0;
        private DOOH.Server.Models.DOOHDB.Campaign _campaign;
        private IEnumerable<DOOH.Server.Models.DOOHDB.Adboard> _selectedAdboards = new List<DOOH.Server.Models.DOOHDB.Adboard>();
        private IEnumerable<DOOH.Server.Models.DOOHDB.Advertisement> _advertisements = new List<DOOH.Server.Models.DOOHDB.Advertisement>();
        private IEnumerable<DOOH.Server.Models.DOOHDB.Schedule> _schedules = new List<DOOH.Server.Models.DOOHDB.Schedule>();
        // private IEnumerable<DOOH.Server.Models.DOOHDB.ScheduleAdboard> _scheduleAdboards = new List<DOOH.Server.Models.DOOHDB.ScheduleAdboard>();


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
                if (CampaignId != null && int.TryParse(CampaignId.ToString(), out int campaignId))
                {
                    _campaignId = campaignId;
                    var expand = "CampaignAdboards($expand=Adboard),Advertisements($expand=Upload),Schedules($expand=ScheduleAdboards($expand=Adboard))";
                    _campaign = await DoohdbService.GetCampaignByCampaignId(campaignId: _campaignId, expand: expand);
                    if (_campaign != null)
                    {
                        _campaignId = _campaign.CampaignId;
                        CampaignName = _campaign.CampaignName;
                        StartDate = _campaign.StartDate;
                        EndDate = _campaign.EndDate;
                        Budget = _campaign.Budget;
                        BudgetType = (BudgetType)_campaign.BudgetType;
                        IsDraft = DOOH.Server.Extensions.StatusExtensions.IsDraft((DOOH.Server.Models.Enums.Status)_campaign.Status);
                        Status = (DOOH.Server.Models.Enums.Status)_campaign.Status;
                        _selectedAdboards = _campaign.CampaignAdboards.Select(ca => ca.Adboard).ToList();
                        _advertisements = _campaign.Advertisements;
                        _schedules = _campaign.Schedules.Where(x => x.Date >= DateTime.Today);
                        // _scheduleAdboards = _schedules.SelectMany(x => x.ScheduleAdboards);
                    }
                }
            }
            finally
            {
                IsSaving = false;
                StateHasChanged();
            }
        }
        
        private async Task LoadSchedules()
        {
            var expand = "ScheduleAdboards($expand=Adboard)";
            var result = await DoohdbService.GetSchedules(filter: $"CampaignId eq {_campaignId}", expand: expand);
            _schedules = result.Value.AsODataEnumerable();
        }

        private int _selectedTabIndex = 0;


        private async Task SaveCampaignAdboards()
        {
            var existing = _campaign.CampaignAdboards.Select(ca => ca.Adboard).ToList();
            var toAdd = _selectedAdboards.Except(existing).ToList();
            var toRemove = existing.Except(_selectedAdboards).ToList();
            
            foreach (var adboard in toAdd)
            {
                var campaignAdboard = new DOOH.Server.Models.DOOHDB.CampaignAdboard();
                campaignAdboard.CampaignId = _campaign.CampaignId;
                campaignAdboard.AdboardId = adboard.AdboardId;
                await DoohdbService.CreateCampaignAdboard(campaignAdboard);
            }
            
            foreach (var adboard in toRemove)
            {
                var campaignAdboard = _campaign.CampaignAdboards.FirstOrDefault(ca => ca.AdboardId == adboard.AdboardId);
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
                
                var selectedAdboardsResult = await DoohdbService.GetAdboards(filter: $"AdboardId in ({string.Join(",", _selectedAdboards.Select(x => x.AdboardId))})", top: 1000);
                var selectedAdboards = selectedAdboardsResult.Value.AsODataEnumerable();
                
                // OpenDialog Review
                var reviewDialog = await DialogService.OpenAsync<ReviewPublish>("Review", 
                    new Dictionary<string, object>()
                    {
                        { "Campaign", _campaign },
                        { "Advertisements", _advertisements },
                        { "Adboards", selectedAdboards },
                        { "Schedules", _schedules }
                    }, new DialogOptions()
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
                _campaign.CampaignId = _campaignId;
                _campaign.CampaignName = CampaignName;
                _campaign.StartDate = StartDate;
                _campaign.EndDate = EndDate;
                _campaign.Budget = Budget;
                _campaign.BudgetType = (int)BudgetType;
                _campaign.Status = (int)Status;
                
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


        private async Task OnAddAdboard(DOOH.Server.Models.DOOHDB.Adboard adboard)
        {
            if (_selectedAdboards.All(x => x.AdboardId != adboard.AdboardId))
            {
                _selectedAdboards = _selectedAdboards.Append(adboard);
            }
        }

        private async Task OnRemoveAdboard(DOOH.Server.Models.DOOHDB.Adboard adboard)
        {
            _selectedAdboards = _selectedAdboards.Where(x => x.AdboardId != adboard.AdboardId);
        }

        private async Task OnClearAdboards()
        {
            _selectedAdboards = new List<DOOH.Server.Models.DOOHDB.Adboard>();
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
        
        private async Task AddSchedule(DOOH.Server.Models.DOOHDB.Schedule schedule)
        {
            schedule = await DoohdbService.CreateSchedule(schedule);
            var toAdd = schedule.ScheduleAdboards.ToList();
            foreach (var scheduleAdboard in toAdd)
            {
                scheduleAdboard.ScheduleId = schedule.ScheduleId;
                await DoohdbService.CreateScheduleAdboard(scheduleAdboard);
            }
            await LoadSchedules();
            StateHasChanged();
        }
        
        private async Task UpdateSchedule(DOOH.Server.Models.DOOHDB.Schedule schedule)
        {
            await DoohdbService.UpdateSchedule(schedule.ScheduleId, schedule);
            var existing = await DoohdbService.GetScheduleAdboards(filter: $"ScheduleId eq {schedule.ScheduleId}");
            var existingScheduleAdboards = existing.Value.AsODataEnumerable();
            var toRemove = existingScheduleAdboards.Except(schedule.ScheduleAdboards).ToList();
            var toAdd = schedule.ScheduleAdboards.Except(existingScheduleAdboards).ToList();
            foreach (var scheduleAdboard in toAdd)
            {
                await DoohdbService.CreateScheduleAdboard(scheduleAdboard);
            }
            foreach (var scheduleAdboard in toRemove)
            {
                await DoohdbService.DeleteScheduleAdboard(scheduleAdboard.ScheduleId, scheduleAdboard.AdboardId);
            }

            await LoadSchedules();
            StateHasChanged();
        }
        
        private async Task DeleteSchedule(DOOH.Server.Models.DOOHDB.Schedule schedule)
        {
            // _schedules = _schedules.Where(x => x.ScheduleId != schedule.ScheduleId);
            await DoohdbService.DeleteSchedule(schedule.ScheduleId);
            var existing = await DoohdbService.GetScheduleAdboards(filter: $"ScheduleId eq {schedule.ScheduleId}");
            var existingScheduleAdboards = existing.Value.AsODataEnumerable();
            var toRemove = existingScheduleAdboards.Except(schedule.ScheduleAdboards).ToList();
            
            foreach (var scheduleAdboard in toRemove)
            {
                await DoohdbService.DeleteScheduleAdboard(scheduleAdboard.ScheduleId, scheduleAdboard.AdboardId);
            }
            await LoadSchedules();
            StateHasChanged();
        }
    }
}