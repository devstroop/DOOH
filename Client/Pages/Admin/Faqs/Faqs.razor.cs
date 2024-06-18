using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace DOOH.Client.Pages.Admin.Faqs
{
    public partial class Faqs
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

        protected bool IsLoading { get; set; } = false;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Faq> faqs;

        protected RadzenDataGrid<DOOH.Server.Models.DOOHDB.Faq> grid0;
        protected int count;

        [Inject]
        protected SecurityService Security { get; set; }

        protected int faqsCount;

        protected async Task Grid0LoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetFaqs(filter: $"{args.Filter}", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                faqs = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Faqs" });
            }
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddFaq>("Add Faq", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DOOH.Server.Models.DOOHDB.Faq args)
        {
            await DialogService.OpenAsync<EditFaq>("Edit Faq", new Dictionary<string, object> { {"FaqId", args.FaqId} });
            await grid0.Reload();
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, DOOH.Server.Models.DOOHDB.Faq faq)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await DOOHDBService.DeleteFaq(faqId:faq.FaqId);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete Faq"
                });
            }
        }


        protected async Task faqsLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetFaqs(new Query { Top = args.Top, Skip = args.Skip, Filter = args.Filter, OrderBy = args.OrderBy });

                faqs = result.Value.AsODataEnumerable();
                faqsCount = result.Count;
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load" });
            }
        }

        // SplitButtonClick
        protected async Task SplitButtonClick(RadzenSplitButtonItem item, DOOH.Server.Models.DOOHDB.Faq faq)
        {
            if (item.Text == "Edit")
            {
                await EditRow(faq);
            }
            else if (item.Text == "Delete")
            {
                await GridDeleteButtonClick(null, faq);
            }
        }
    }
}