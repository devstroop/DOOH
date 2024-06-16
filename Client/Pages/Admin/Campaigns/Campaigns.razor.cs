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
    public partial class Campaigns
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

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Campaign> campaigns;

        protected RadzenDataList<DOOH.Server.Models.DOOHDB.Campaign> list0;
        protected int count;
        protected bool IsLoading = true;

        protected string search { get; set; } = "";

        [Inject]
        protected SecurityService Security { get; set; }

        protected int campaignsCount;

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await list0.GoToPage(0);

            await list0.Reload();
        }
        protected async void RefreshClick(MouseEventArgs args)
        {
            await list0.Reload();
        }

        protected async Task Search(MouseEventArgs args)
        {

            await list0.GoToPage(0);

            await list0.Reload();
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<Admin.Campaigns.Editor.CampaignEditor>(string.Empty, null, options: new DialogOptions
            {
                Width = "100%",
                Height = "100%",
                ShowClose = true,
                ShowTitle = false,
                Style = "border-radius: 0px;"
            });
            await list0.Reload();

            //NavigationManager.NavigateTo("admin/campaigns/editor");
        }

        protected async Task EditButtonClick(MouseEventArgs args, DOOH.Server.Models.DOOHDB.Campaign campaign)
        {
            await DialogService.OpenAsync<Admin.Campaigns.Editor.CampaignEditor>(string.Empty, new Dictionary<string, object> { { "CampaignId", campaign.CampaignId } }, options: new DialogOptions
            {
                Width = "100%",
                Height = "100%",
                ShowClose = true,
                ShowTitle = false,
                Style = "border-radius: 0px;"
            });
            await list0.Reload();
        }

        protected async Task DeleteButtonClick(MouseEventArgs args, DOOH.Server.Models.DOOHDB.Campaign campaign)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await DOOHDBService.DeleteCampaign(campaignId:campaign.CampaignId);

                    if (deleteResult != null)
                    {
                        await list0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete Campaign"
                });
            }
        }

        protected int selectedIndex { get; set; } = 2;

        protected async Task campaignsLoadData(LoadDataArgs args)
        {
            try
            {
                IsLoading = true;
                //var result = await DOOHDBService.GetCampaigns(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: args.Filter, orderby: args.OrderBy);
                var result = await DOOHDBService.GetCampaigns(filter: $@"(contains(CampaignName,""{search}"")) and {(string.IsNullOrEmpty(args.Filter) ? "true" : args.Filter)}", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null, expand: "Advertisements($expand=Attachment), CampaignAdboards($expand=Adboard)");

                campaigns = result.Value.AsODataEnumerable();
                campaignsCount = result.Count;
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load" });
            }
            finally
            {
                IsLoading = false;
            }

        }
    }
}