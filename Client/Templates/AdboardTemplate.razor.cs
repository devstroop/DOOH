using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DOOH.Client.Templates
{
    public partial class AdboardTemplate
    {
        [Parameter] public DOOH.Server.Models.DOOHDB.Adboard Context { get; set; }

        [Parameter] public RenderFragment<DOOH.Server.Models.DOOHDB.Adboard> Actions { get; set; }

        [Inject] protected IConfiguration Configuration { get; set; }

        [Inject] protected IJSRuntime JsRuntime { get; set; }
        
        private Tuple<bool, DateTime?> _liveStatus;

        private Tuple<bool, DateTime?> Status()
        {
            if (_liveStatus != null)
            {
                return _liveStatus;
            }
            var status = Context.AdboardStatuses.FirstOrDefault();
            if (status is { Connected: true } && status.ConnectedAt.AddMilliseconds(status.Delay) > DateTime.Now)
            {
                return new Tuple<bool, DateTime?>(true, status.ConnectedAt.AddMilliseconds(status.Delay));
            }

            return new Tuple<bool, DateTime?>(false, null);
        }
        
        [JSInvokable]
        public async Task UpdateAdboardStatus(int adboardId, bool status)
        {
            if (Context.AdboardId == adboardId)
            {
                _liveStatus = new Tuple<bool, DateTime?>(status, DateTime.Now);
                StateHasChanged();
            }
        }
    }
}