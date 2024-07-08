using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace DOOH.Client.Pages.Admin.Providers.Categories
{
    public partial class EditCategory
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

        [Parameter]
        public int CategoryId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            category = await DOOHDBService.GetCategoryByCategoryId(categoryId:CategoryId);
        }
        protected bool errorVisible;
        protected DOOH.Server.Models.DOOHDB.Category category;

        protected bool IsSaving { get; set; } = false;
        protected async Task FormSubmit()
        {
            try
            {
                IsSaving = true;
                StateHasChanged();
                var result = await DOOHDBService.UpdateCategory(categoryId:CategoryId, category);

                if (result != null)
                {
                    DialogService.Close(category);
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

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }

        [Inject]
        protected SecurityService Security { get; set; }
    }
}