using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DOOH.Client.Templates
{
    public partial class AdboardTemplate
    {
        [Parameter]
        public DOOH.Server.Models.DOOHDB.Adboard Context { get; set; }

        [Parameter]
        public RenderFragment<DOOH.Server.Models.DOOHDB.Adboard> Actions { get; set; }

        [Inject]
        protected IConfiguration Configuration { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        //protected string gMapApiKey => Configuration.GetValue<string>("Google:API_KEY");

        protected override void OnInitialized()
        {
            //JSRuntime.InvokeVoidAsync("console.log", gMapApiKey);
            base.OnInitialized();
        }

    }
}
