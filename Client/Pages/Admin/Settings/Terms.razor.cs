using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace DOOH.Client.Pages.Admin.Settings
{
    public partial class Terms
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
        public DOOHDBService DOOHDBService { get; set; }

        protected bool isEditing = false;
        protected bool isNew = false;

        protected DOOH.Server.Models.DOOHDB.Policy policy;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                policy = await DOOHDBService.GetPolicyById(id: "terms");
                return;
            }
            catch { }

            policy = policy ?? new DOOH.Server.Models.DOOHDB.Policy { Id = "terms" };
            isNew = true;
        }

        protected async Task SaveClick(MouseEventArgs args)
        {
            try
            {
                if (isNew)
                {
                    await DOOHDBService.CreatePolicy(policy);
                    isNew = false;
                }
                else
                {
                    await DOOHDBService.UpdatePolicy(policy.Id, policy);
                }
                NotificationService.Notify(NotificationSeverity.Success, "Success", "Terms of use saved!");
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error", "An error has occurred");
            }
        }

        protected async Task CancelClick(MouseEventArgs args)
        {
            await OnInitializedAsync();
            isEditing = false;
            StateHasChanged();
        }
        protected async Task EditClick(MouseEventArgs args)
        {
            isEditing = true;
            StateHasChanged();
        }
    }
}