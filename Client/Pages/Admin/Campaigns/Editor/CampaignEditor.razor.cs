using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using DOOH.Server.Models.DOOHDB;
using DOOH.Server.Models.Enums;
using Radzen.Blazor;

namespace DOOH.Client.Pages.Admin.Campaigns.Editor
{
    public partial class CampaignEditor
    {
        [Parameter]
        public object CampaignId { get; set; }
        private int _campaignId = 0;
        
        
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
        public DOOHDBService DbService { get; set; }
        [Inject]
        protected SecurityService Security { get; set; }

        
        private RadzenTabs _tabs;
        private int SelectedTabIndex { get; set; }= 0;
        private string CampaignName { get; set; } = "Unnamed Campaign";
        private BudgetType BudgetType { get; set; } = BudgetType.Total;
        private decimal Budget { get; set; } = 0;
        private bool IsDraft { get; set; } = true;
        private bool IsSaving { get; set; } = false;
        private DateTime StartDate { get; set; } = DateTime.Today;
        private DateTime EndDate { get; set; } = DateTime.Today.AddDays(30);
        private DOOH.Server.Models.Enums.Status Status { get; set; } = DOOH.Server.Models.Enums.Status.Draft;
        

        protected bool CampaignNameEditable { get; set; } = false;
        private string CampaignIdLabel => $"{(_campaignId != 0 ? $"#{CampaignId}" : "New")}";
        private bool IsCampaignLoading { get; set; } = false;
        private bool IsAdvertisementsLoading { get; set; } = false;
        private bool IsSchedulesLoading { get; set; } = false;
        
        
        private DOOH.Server.Models.DOOHDB.Campaign _campaign;
        private IEnumerable<DOOH.Server.Models.DOOHDB.Adboard> _selectedAdboards = new List<DOOH.Server.Models.DOOHDB.Adboard>();
        private IEnumerable<DOOH.Server.Models.DOOHDB.CampaignAdboard> _selectedCampaignAdboards = new List<DOOH.Server.Models.DOOHDB.CampaignAdboard>();
        private IEnumerable<DOOH.Server.Models.DOOHDB.Advertisement> _advertisements = new List<DOOH.Server.Models.DOOHDB.Advertisement>();
        private IEnumerable<DOOH.Server.Models.DOOHDB.Schedule> _schedules = new List<DOOH.Server.Models.DOOHDB.Schedule>();

        
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
        
        private void OnRefresh()
        {
            StateHasChanged();
        }

        /// <summary>
        /// Campaign
        /// </summary>
        /// 
        private async Task LoadCampaign()
        {
            try
            {
                if(IsCampaignLoading) return;
                IsCampaignLoading = true;
                StateHasChanged();
                if (CampaignId != null && int.TryParse(CampaignId.ToString(), out int campaignId))
                {
                    _campaignId = campaignId;
                    const string expand = "CampaignAdboards($expand=Adboard),Advertisements,Schedules($expand=ScheduleAdboards($expand=Adboard),ScheduleAdvertisements($expand=Advertisement))";
                    _campaign = await DbService.GetCampaignByCampaignId(campaignId: _campaignId, expand: expand);
                    if (_campaign != null)
                    {
                        _campaignId = _campaign.CampaignId;
                        CampaignName = _campaign.CampaignName;
                        StartDate = _campaign?.StartDate ?? DateTime.Now;
                        EndDate = _campaign?.EndDate ?? DateTime.Now;
                        Budget = _campaign?.Budget ?? 0;
                        BudgetType = ((BudgetType?)_campaign?.BudgetType) ?? BudgetType.Total;
                        IsDraft = DOOH.Server.Extensions.StatusExtensions.IsDraft((DOOH.Server.Models.Enums.Status?)_campaign?.Status ?? Status.Draft);
                        Status = (DOOH.Server.Models.Enums.Status?)_campaign?.Status ?? Status.Draft;
                        _selectedCampaignAdboards = _campaign?.CampaignAdboards;
                        _selectedAdboards = _campaign?.CampaignAdboards?.Select(x => x.Adboard);
                        _advertisements = _campaign?.Advertisements;
                        _schedules = _campaign?.Schedules;
                        // _scheduleAdboards = _schedules.SelectMany(x => x.ScheduleAdboards);
                    }
                }
            }
            finally
            {
                IsCampaignLoading = false;
                StateHasChanged();
            }

            if (CampaignId == null || !int.TryParse(CampaignId.ToString(), out var _))
            {
                NavigationManager.NavigateTo("admin/campaigns");
            }
        }

        /// <summary>
        /// Adborads
        /// </summary>
        /// <param name="adboard"></param>
        private async Task OnAddCampaignAdboard(DOOH.Server.Models.DOOHDB.CampaignAdboard adboard)
        {
            if (_selectedCampaignAdboards.All(x => x.AdboardId != adboard.AdboardId))
            {
                adboard = await DbService.CreateCampaignAdboard(adboard);
                _selectedCampaignAdboards = _selectedCampaignAdboards.Append(adboard);  
                _selectedAdboards = _selectedAdboards.Append(adboard.Adboard);
            }
        }

        private async Task OnRemoveAdboard(DOOH.Server.Models.DOOHDB.CampaignAdboard adboard)
        {
            var result = await DbService.DeleteCampaignAdboard(adboardId: adboard.AdboardId, campaignId: adboard.CampaignId);
            _selectedCampaignAdboards = _selectedCampaignAdboards.Where(x => x.AdboardId != adboard.AdboardId);
            _selectedAdboards = _selectedAdboards.Where(x => x.AdboardId != adboard.AdboardId);
        }

        private async Task OnClearAdboards()
        {
            _selectedCampaignAdboards = new List<DOOH.Server.Models.DOOHDB.CampaignAdboard>();
            _selectedAdboards = new List<DOOH.Server.Models.DOOHDB.Adboard>();
        }
        
        
        /// <summary>
        /// Advertisements
        /// </summary>
        /// <param name="advertisementId"></param>
        private async Task OnDeleteAdvertisement(int advertisementId)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this advertisement?") == true)
                {
                    var result = await DbService.DeleteAdvertisement(advertisementId);
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
            // if (_advertisements.Any(x => x.AdvertisementId == advertisement.AdvertisementId))
            // {
            //     NotificationService.Notify(new NotificationMessage
            //         { Severity = NotificationSeverity.Warning, Summary = "Warning", Detail = "Advertisement already added" });
            //     return;
            // }
            advertisement.CampaignId = _campaignId;
            advertisement = await DbService.CreateAdvertisement(advertisement);
            await LoadCampaign();
        }
        
        /// <summary>
        /// Schedule
        /// </summary>
        /// <param name="schedule"></param>
        
        private async Task LoadSchedules()
        {
            try
            {
                if (IsSchedulesLoading) return;
                IsSchedulesLoading = true;
                StateHasChanged();
                const string expand = "ScheduleAdboards($expand=Adboard),ScheduleAdvertisements($expand=Advertisement)";
                var result = await DbService.GetSchedules(filter: $"CampaignId eq {_campaignId}", expand: expand,
                    orderby: "Start");
                _schedules = result.Value.AsODataEnumerable();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                    { Severity = NotificationSeverity.Error, Summary = "Error", Detail = ex.Message });
            }
            finally
            {
                IsSchedulesLoading = false;
                StateHasChanged();
            }
        }
        
        private async Task AddSchedule(DOOH.Server.Models.DOOHDB.Schedule schedule)
        {
            schedule = await DbService.CreateSchedule(schedule);
            var toAddScheduleAdboards = schedule.ScheduleAdboards.ToList();
            var toAddScheduleAdvertisements = schedule.ScheduleAdvertisements.ToList();
            foreach (var scheduleAdboard in toAddScheduleAdboards)
            {
                scheduleAdboard.ScheduleId = schedule.ScheduleId;
                await DbService.CreateScheduleAdboard(scheduleAdboard);
            }
            foreach (var scheduleAdvertisement in toAddScheduleAdvertisements)
            {
                scheduleAdvertisement.ScheduleId = schedule.ScheduleId;
                await DbService.CreateScheduleAdvertisement(scheduleAdvertisement);
            }
            await LoadSchedules();
            StateHasChanged();
        }
        
        private async Task UpdateSchedule(DOOH.Server.Models.DOOHDB.Schedule schedule)
        {
            await DbService.UpdateSchedule(schedule.ScheduleId, schedule);
            var existing = await DbService.GetScheduleAdboards(filter: $"ScheduleId eq {schedule.ScheduleId}");
            var existingScheduleAdboards = existing.Value.AsODataEnumerable();
            var toRemove = existingScheduleAdboards.Except(schedule.ScheduleAdboards).ToList();
            var toAdd = schedule.ScheduleAdboards.Except(existingScheduleAdboards).ToList();
            foreach (var scheduleAdboard in toAdd)
            {
                await DbService.CreateScheduleAdboard(scheduleAdboard);
            }
            foreach (var scheduleAdboard in toRemove)
            {
                await DbService.DeleteScheduleAdboard(scheduleAdboard.ScheduleId, scheduleAdboard.AdboardId);
            }

            await LoadSchedules();
            StateHasChanged();
        }
        
        private async Task DeleteSchedule(DOOH.Server.Models.DOOHDB.Schedule schedule)
        {
            // _schedules = _schedules.Where(x => x.ScheduleId != schedule.ScheduleId);
            await DbService.DeleteSchedule(schedule.ScheduleId);
            var existing = await DbService.GetScheduleAdboards(filter: $"ScheduleId eq {schedule.ScheduleId}");
            var existingScheduleAdboards = existing.Value.AsODataEnumerable();
            var toRemove = existingScheduleAdboards.Except(schedule.ScheduleAdboards).ToList();
            
            foreach (var scheduleAdboard in toRemove)
            {
                await DbService.DeleteScheduleAdboard(scheduleAdboard.ScheduleId, scheduleAdboard.AdboardId);
            }
            await LoadSchedules();
            StateHasChanged();
        }
        
        
        /// <summary>
        /// Actions
        /// </summary>
        /// <param name="args"></param>

        private async Task ContinueClick(MouseEventArgs args)
        {
            if (SelectedTabIndex < 2)
            {
                SelectedTabIndex++;
                return;
            }
            try
            {
                var confirmariotnResult = await DialogService.Confirm("Are you sure you want to save?");
                if (confirmariotnResult != true)
                {
                    return;
                }
                
                var selectedAdboardsResult = await DbService.GetAdboards(filter: $"AdboardId in ({string.Join(",", _selectedCampaignAdboards.Select(x => x.AdboardId))})", top: 1000);
                var selectedAdboards = selectedAdboardsResult.Value.AsODataEnumerable();
                
                // OpenDialog Review
                // var reviewDialog = await DialogService.OpenAsync<ReviewPublish>("Review", 
                //     new Dictionary<string, object>()
                //     {
                //         { "Campaign", _campaign },
                //         { "Advertisements", _advertisements },
                //         { "Adboards", selectedAdboards },
                //         { "Schedules", _schedules }
                //     }, new DialogOptions()
                // {
                //     Width = "100%",
                //     Height = "100%",
                // });
                // if (reviewDialog != true)
                // {
                //     return;
                // }
                
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
                    var result = await DbService.CreateCampaign(_campaign);
                    if(result != null)
                    {
                        _campaign.CampaignId = result.CampaignId;
                        // await SaveCampaignAdboards();
                        NavigationManager.NavigateTo("admin/campaigns");
                    }
                }
                else
                {
                    var result = await DbService.UpdateCampaign(_campaign.CampaignId, _campaign);
                    if (result != null)
                    {
                        // await SaveCampaignAdboards();
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

        private async Task DiscardClick(MouseEventArgs args)
        {
            var result = await DialogService.Confirm("Are you sure you want to cancel?");
            if (result == true)
            {
                NavigationManager.NavigateTo("admin/campaigns");
            }
        }
    }
}