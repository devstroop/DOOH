using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace DOOH.Client.Services
{
    public class BrowserService : IAsyncDisposable
    {
        private readonly IJSRuntime _js;
        private DotNetObjectReference<BrowserService> _dotNetHelper;

        public BrowserService(IJSRuntime js)
        {
            _js = js;
            _dotNetHelper = DotNetObjectReference.Create(this);
        }

        public async Task<WindowDimension> GetDimensions()
        {
            return await _js.InvokeAsync<WindowDimension>("getWindowDimensions");
        }

        public async Task<string> GetCurrentBreakpoint()
        {
            return await _js.InvokeAsync<string>("getCurrentBreakpoint");
        }

        // Viewport change event
        public event Action<WindowDimension> OnViewportChanged;

        [JSInvokable]
        public void ViewportChanged(WindowDimension dimension)
        {
            OnViewportChanged?.Invoke(dimension);
        }

        public async Task RegisterResizeEvent()
        {
            await _js.InvokeVoidAsync("registerResizeEvent", _dotNetHelper);
        }

        public async ValueTask DisposeAsync()
        {
            if (_dotNetHelper != null)
            {
                _dotNetHelper.Dispose();
            }
        }
    }

    public class WindowDimension
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
