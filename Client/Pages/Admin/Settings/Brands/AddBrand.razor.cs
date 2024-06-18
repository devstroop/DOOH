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

namespace DOOH.Client.Pages.Admin.Settings.Brands
{
    public partial class AddBrand
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

        protected override async Task OnInitializedAsync()
        {
            brand = new DOOH.Server.Models.DOOHDB.Brand();
        }



        protected bool errorVisible;
        protected DOOH.Server.Models.DOOHDB.Brand brand;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Attachment> attachmentsForBrandLogoAttachmentKey;

        protected bool IsSaving { get; set; } = false;
        protected async Task FormSubmit()
        {
            try
            {
                IsSaving = true;
                StateHasChanged();
                brand.BrandLogo = Images.FirstOrDefault();
                var result = await DOOHDBService.CreateBrand(brand);
                DialogService.Close(brand);
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

        protected List<string> Images = new List<string>();

        protected void OnRefreshImage() => StateHasChanged();

        protected async void OnAddLogo(string image)
        {
            Images = new List<string> { image };
            StateHasChanged();
        }
        protected async void OnDeleteLogo(string image)
        {
            Images.Clear();
            StateHasChanged();
        }

    }
}