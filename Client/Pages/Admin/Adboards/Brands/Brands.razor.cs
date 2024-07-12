using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace DOOH.Client.Pages.Admin.Adboards.Brands
{
    public partial class Brands
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

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Brand> brands;

        protected RadzenDataList<DOOH.Server.Models.DOOHDB.Brand> list0;
        protected int count;

        protected string search = "";
        protected bool isLoading = true;

        [Inject]
        protected SecurityService Security { get; set; }

        protected int brandsCount;

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await list0.GoToPage(0);

            await list0.Reload();
        }


        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddBrand>("Add Brand", null);
            await list0.Reload();
        }

        protected async Task EditButtonClick(MouseEventArgs args, DOOH.Server.Models.DOOHDB.Brand brand)
        {
            await DialogService.OpenAsync<EditBrand>("Edit Brand", new Dictionary<string, object> { {"BrandId", brand.BrandId} });
            await list0.Reload();
        }

        protected async Task DeleteButtonClick(MouseEventArgs args, DOOH.Server.Models.DOOHDB.Brand brand)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await DOOHDBService.DeleteBrand(brandId:brand.BrandId);

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
                    Detail = $"Unable to delete Brand"
                });
            }
        }


        protected async Task brandsLoadData(LoadDataArgs args)
        {
            isLoading = true;
            StateHasChanged();
            try
            {
                var result = await DOOHDBService.GetBrands(filter: $@"(contains(BrandName,""{search}"")) and {(string.IsNullOrEmpty(args.Filter) ? "true" : args.Filter)}", expand: "", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null);
                brands = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load" });
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }


        protected async void MoreVertClick(RadzenSplitButtonItem item, DOOH.Server.Models.DOOHDB.Brand brand)
        {
            switch (item.Text)
            {
                case "Edit":
                    await EditButtonClick(null, brand);
                    break;
                case "Delete":
                    await DeleteButtonClick(null, brand);
                    break;
            }

            StateHasChanged();
        }
    }
}