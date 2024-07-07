using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OpenLayer.NET.Models;


namespace OpenLayer.NET;
public partial class OpenLayerMap
{
    private DotNetObjectReference<OpenLayerMap>? objectReference;
    [Parameter]
    public string Id { get; set; } = "map";
    
    [Parameter]
    public Location Center { get; set; } = new Location("Test");
    
    [Parameter]
    public IEnumerable<Location> Locations { get; set; } = new List<Location>();
    
    [Parameter]
    public string Style { get; set; } = "min-height: 400px; width: 100%;";
    
    // private string clickedPostcode = "none yet";
    protected override async Task OnParametersSetAsync()
    {
        objectReference = objectReference ?? DotNetObjectReference.Create(this);
        
        await Runtime.InvokeAsync<object>("PostcodesOSMInterop.ShowMap", new object[] { Id, objectReference, Center, Locations });
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            await Runtime.InvokeAsync<object>("PostcodesOSMInterop.ShowMap", new object[] { "map", objectReference, Center, Locations });
        }
    }

    [JSInvokable("SetClickedPostcode")]
    public void SetClickedPostcode(string postcode)
    {
        Runtime.InvokeAsync<object>("alert", $"You clicked on {postcode}");
        StateHasChanged();
    }
    
    
    [Inject]
    private IJSRuntime Runtime { get; set; }
}