using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using DOOH.Server.Models.DOOHDB;

namespace DOOH.Client.Pages.Admin.Settings.Categories
{
    public partial class Categories
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
        protected DOOH.Client.DOOHDBService DOOHDBService { get; set; }

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Category> categories;

        protected int categoriesCount;

        protected bool isCategoriesLoading = false;

        protected async Task categoriesLoadData(LoadDataArgs args)
        {
            try
            {
                isCategoriesLoading = true;
                var result = await DOOHDBService.GetCategories(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: args.Filter, orderby: args.OrderBy);

                categories = result.Value.AsODataEnumerable();
                categoriesCount = result.Count;
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load" });
            }
            finally
            {
                isCategoriesLoading = false;
            }
        }

        protected async Task AddCategoryClick(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddCategory>("Add Category", null);
            if (dialogResult != null)
            {
                await categoriesLoadData(new LoadDataArgs() { });
            }
        }

        protected async void MoreVertClick(RadzenSplitButtonItem item, Category category)
        {
            if (item.Text == "Edit")
            {
                var dialogResult = await DialogService.OpenAsync<EditCategory>("Edit Category", new Dictionary<string, object>() { { "CategoryId", category.CategoryId } });
                if (dialogResult != null)
                {
                    await categoriesLoadData(new LoadDataArgs() { });
                    StateHasChanged();
                }
            }
            else if (item.Text == "Delete")
            {
                var dialogResult = await DialogService.Confirm("Are you sure you want to delete this category?", "Delete Category");
                if (dialogResult == true)
                {
                    await DOOHDBService.DeleteCategory(categoryId: category.CategoryId);
                    await categoriesLoadData(new LoadDataArgs() { });
                    StateHasChanged();
                }
            }
        }

    }
}