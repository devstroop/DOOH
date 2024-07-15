using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace DOOH.Client.Templates
{
    public partial class CampaignAdboardTemplate
    {
        [Parameter]
        public DOOH.Server.Models.DOOHDB.Adboard Context { get; set; }

        [Parameter]
        public RenderFragment<DOOH.Server.Models.DOOHDB.Adboard> Actions { get; set; }

        [Parameter]
        public EventCallback<DOOH.Server.Models.DOOHDB.Adboard> OnClick { get; set; }
        
        [Parameter]
        public EventCallback<DOOH.Server.Models.DOOHDB.Adboard> OnSelect { get; set; }

        [Parameter]
        public EventCallback<DOOH.Server.Models.DOOHDB.Adboard> OnUnselect { get; set; }

        [Parameter]
        public bool Selected { get; set; }

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

        protected async System.Threading.Tasks.Task SelectClick(MouseEventArgs args)
        {
            if (OnSelect.HasDelegate)
            {
                await OnSelect.InvokeAsync(Context);
            }
        }

        protected async System.Threading.Tasks.Task UnselectClick(MouseEventArgs args)
        {
            if (OnUnselect.HasDelegate)
            {
                await OnUnselect.InvokeAsync(Context);
            }
        }
    }
}