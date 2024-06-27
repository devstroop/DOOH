using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace DOOH.Client.Pages.Admin
{
    public partial class Dashboard
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
        protected SecurityService Security { get; set; }

        [Inject]
        protected DOOHDBService DBService { get; set; }

        public class UserRegistrationTrend
        {
            public string Day { get; set; }
            public int Count { get; set; }
        }
        
        // Last 7 days user registration trend
        protected IEnumerable<UserRegistrationTrend> userRegistrationTrends = new List<UserRegistrationTrend>
            {
                new UserRegistrationTrend { Day = "Monday", Count = 20 },
                new UserRegistrationTrend { Day = "Tuesday", Count = 18 },
                new UserRegistrationTrend { Day = "Wednesday", Count = 30 },
                new UserRegistrationTrend { Day = "Thursday", Count = 15 },
                new UserRegistrationTrend { Day = "Friday", Count = 5 },
                new UserRegistrationTrend { Day = "Saturday", Count = 25 },
                new UserRegistrationTrend { Day = "Sunday", Count = 55 }
            };
            
        
        [Inject]
        protected DOOH.Client.DOOHDBService DOOHDBService { get; set; }

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Adboard> adboards;

        protected int adboardsCount;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Campaign> campaigns;

        protected int campaignsCount;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Provider> providers;

        protected int providersCount;
        
        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await adboardsLoadData(new LoadDataArgs());
            }
        }


        protected async Task adboardsLoadData(LoadDataArgs args)
        {
            try
            {
                var expand = $"Category, Motherboard($expand=Brand), Provider, Display($expand=Brand), AdboardImages";
                var result = await DOOHDBService.GetAdboards(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: args.Filter, orderby: args.OrderBy, expand: expand);

                adboards = result.Value.AsODataEnumerable();
                adboardsCount = result.Count;
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load" });
            }
        }


        protected async Task campaignsLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetCampaigns(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: args.Filter, orderby: args.OrderBy);

                campaigns = result.Value.AsODataEnumerable();
                campaignsCount = result.Count;
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load" });
            }
        }


        protected async Task providersLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetProviders(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: args.Filter, orderby: args.OrderBy);

                providers = result.Value.AsODataEnumerable();
                providersCount = result.Count;
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load" });
            }
        }

    }
}