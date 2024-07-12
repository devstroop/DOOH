using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using DOOH.Server.Models.DOOHDB;
using System.Text.Json.Nodes;

namespace DOOH.Client.Pages.Admin.Adboards
{
    public partial class EditAdboard
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
        public int AdboardId { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }

        protected List<AdboardImage> adboardImages;

        protected override async Task OnInitializedAsync()
        {
            adboard = await DOOHDBService.GetAdboardByAdboardId(adboardId:AdboardId, expand: "AdboardImages");
            adboardImages = adboard.AdboardImages.ToList();
        }
        protected bool errorVisible;
        protected DOOH.Server.Models.DOOHDB.Adboard adboard;


        protected IEnumerable<DOOH.Server.Models.DOOHDB.Provider> providersForProviderId;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Category> categoriesForCategoryId;


        protected int providersForProviderIdCount;
        protected DOOH.Server.Models.DOOHDB.Provider providersForProviderIdValue;
        protected async Task providersForProviderIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetProviders(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(ContactName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                providersForProviderId = result.Value.AsODataEnumerable();
                providersForProviderIdCount = result.Count;

                if (!object.Equals(adboard.ProviderId, null))
                {
                    var valueResult = await DOOHDBService.GetProviders(filter: $"ProviderId eq {adboard.ProviderId}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        providersForProviderIdValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Provider" });
            }
        }

        protected int categoriesForCategoryIdCount;
        protected DOOH.Server.Models.DOOHDB.Category categoriesForCategoryIdValue;
        protected async Task categoriesForCategoryIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetCategories(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(CategoryName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                categoriesForCategoryId = result.Value.AsODataEnumerable();
                categoriesForCategoryIdCount = result.Count;

                if (!object.Equals(adboard.CategoryId, null))
                {
                    var valueResult = await DOOHDBService.GetCategories(filter: $"CategoryId eq {adboard.CategoryId}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        categoriesForCategoryIdValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Category" });
            }
        }

        protected bool IsSaving { get; set; } = false;
        protected async Task FormSubmit()
        {
            try
            {
                IsSaving = true;
                StateHasChanged();

                var result = await DOOHDBService.UpdateAdboard(adboardId:AdboardId, adboard);

                if (result != null)
                {
                    foreach (var each in adboardImages)
                    {
                        if (each.AdboardImageId == 0)
                        {
                            var adboardImageResult = await DOOHDBService.CreateAdboardImage(each);
                            if (adboardImageResult != null)
                            {
                                each.AdboardImageId = adboardImageResult.AdboardImageId;
                            }
                        }
                        else
                        {
                            await DOOHDBService.UpdateAdboardImage(each.AdboardImageId, each);
                        }
                    }
                    adboard.AdboardImages = adboardImages;
                    DialogService.Close(adboard);
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

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Display> displaysForDisplayId;

        protected IEnumerable<DOOH.Server.Models.DOOHDB.Motherboard> motherboardsForMotherboardId;


        protected int displaysForDisplayIdCount;
        protected DOOH.Server.Models.DOOHDB.Display displaysForDisplayIdValue;
        protected async Task displaysForDisplayIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetDisplays(top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null, filter: $"contains(Model, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}", expand: "Brand");
                displaysForDisplayId = result.Value.AsODataEnumerable();
                displaysForDisplayIdCount = result.Count;

                if (!object.Equals(adboard.DisplayId, null))
                {
                    var valueResult = await DOOHDBService.GetDisplays(filter: $"DisplayId eq {adboard.DisplayId}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        displaysForDisplayIdValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Display" });
            }
        }

        protected int motherboardsForMotherboardIdCount;
        protected DOOH.Server.Models.DOOHDB.Motherboard motherboardsForMotherboardIdValue;
        protected async Task motherboardsForMotherboardIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DOOHDBService.GetMotherboards(top: args.Top, skip: args.Skip, count: args.Top != null && args.Skip != null, filter: $"contains(Rom, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}", expand: "Brand");
                motherboardsForMotherboardId = result.Value.AsODataEnumerable();
                motherboardsForMotherboardIdCount = result.Count;

                if (!object.Equals(adboard.MotherboardId, null))
                {
                    var valueResult = await DOOHDBService.GetMotherboards(filter: $"MotherboardId eq {adboard.MotherboardId}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        motherboardsForMotherboardIdValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Motherboard" });
            }
        }


        protected async void OnDeleteImage(string image)
        {
            var adboardImage = adboardImages.FirstOrDefault(x => x.Image == image);
            if (adboardImage != null)
            {
                if (adboardImage.AdboardImageId != 0)
                {
                    await DOOHDBService.DeleteAdboardImage(adboardImage.AdboardImageId);
                }
                adboardImages.Remove(adboardImage);
            }
            StateHasChanged();
        }

        protected async void OnAddImage(string image)
        {
            var adboardImage = new DOOH.Server.Models.DOOHDB.AdboardImage() { AdboardImageId = 0, AdboardId = AdboardId, Image = image };
            //adboardImage = await DOOHDBService.CreateAdboardImage(adboardImage);
            adboardImages.Add(adboardImage);
            StateHasChanged();
        }

        protected void OnRefreshImage() => StateHasChanged();

        protected async Task GetLocation()
        {
            var coords = await JSRuntime.InvokeAsync<JsonNode>("getCoords");
            if (coords != null)
            {
                adboard.Longitude = (float)coords[0];
                adboard.Latitude = (float)coords[1];
            }
        }
    }
}