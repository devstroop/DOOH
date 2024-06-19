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

        public class DataItem
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }
        
        protected IEnumerable<DataItem> UserRegistrationTrends = new[]
            {
                new DataItem { Name = "January", Value = 100 },
                new DataItem { Name = "February", Value = 200 },
                new DataItem { Name = "March", Value = 300 },
                new DataItem { Name = "April", Value = 400 },
                new DataItem { Name = "May", Value = 500 },
                new DataItem { Name = "June", Value = 600 },
                new DataItem { Name = "July", Value = 700 },
                new DataItem { Name = "August", Value = 800 },
                new DataItem { Name = "September", Value = 900 },
                new DataItem { Name = "October", Value = 1000 },
                new DataItem { Name = "November", Value = 1100 },
                new DataItem { Name = "December", Value = 1200 }
            };

        [Inject]
        protected DOOH.Client.DOOHDBService DOOHDBService { get; set; }

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Adboard> adboards;

        protected int adboardsCount;
        
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
                var expand = $"Category, City, State, Country, Motherboard($expand=Brand), Provider, Display($expand=Brand), AdboardImages";
                var result = await DOOHDBService.GetAdboards(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: args.Filter, orderby: args.OrderBy, expand: expand);

                adboards = result.Value.AsODataEnumerable();
                adboardsCount = result.Count;
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load" });
            }
        }

    }
}