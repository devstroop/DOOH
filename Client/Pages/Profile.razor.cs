using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace DOOH.Client.Pages
{
    public partial class Profile
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

        protected bool twoFactorEnabled = false;
        protected string oldPassword = "";
        protected string newPassword = "";
        protected string confirmPassword = "";
        protected DOOH.Server.Models.ApplicationUser user;
        protected string error;
        protected bool errorVisible;
        protected bool successVisible;
        protected bool isDeveloper = false;

        [Inject]
        protected SecurityService Security { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadProfile();
        }
        
        // After Render
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadProfile();
            }
        }
        
        protected async Task LoadProfile()
        {
            if (String.IsNullOrEmpty(Security.User.Id) && Security.User.Name == "admin" && Security.User.Email == "admin")
            {
                isDeveloper = true;
                // StateHasChanged();
            }
            else
            {
                user = await Security.GetUserById($"{Security.User.Id}");
                twoFactorEnabled = Security.User.TwoFactorEnabled;
            }
        }

        protected async Task FormSubmit()
        {
            try
            {
                bool flag = false;
                if(oldPassword != newPassword)
                {
                    await Security.ChangePassword(oldPassword, newPassword);
                    flag = true;
                }
                if (twoFactorEnabled != Security.User.TwoFactorEnabled)
                {
                    var user = Security.User;
                    user.TwoFactorEnabled = twoFactorEnabled;
                    await Security.UpdateUser(user.Id, user);
                    flag = true;
                }
                if (flag)
                {
                    successVisible = true;
                }
            }
            catch (Exception ex)
            {
                errorVisible = true;
                error = ex.Message;
            }
        }
    }
}