using Amazon.S3.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Radzen;

namespace DOOH.Client.Pages;

public partial class Uploads
{
    [Inject] public IJSRuntime Runtime { get; set; }
    [Inject] public SecurityService Security { get; set; }
    [Inject] public NotificationService Notification { get; set; }
    [Inject] public HttpClient Http { get; set; }
    
    [Parameter] public EventCallback<dynamic> OnSelect { get; set; }
    
    private bool Loading { get; set; } = true;
    private IEnumerable<S3Object> objects = new List<S3Object>();
    private int objectsCount = 0;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadObjects();
            StateHasChanged();
        }
    }

    private async Task LoadObjects()
    {
        try
        {
            var path = Security.IsInRole("Admin") ? "/api/CDN/objects" : $"/api/CDN/objects/{Security.User.Id}";
            var response = await Http.GetAsync(path);
        
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                objects = JsonConvert.DeserializeObject<List<S3Object>>(json);
                objectsCount = objects.Count();
            }
            else
            {
                Notification.Notify(NotificationSeverity.Error, "Error", response.ReasonPhrase);
            }
        }
        catch (Exception ex)
        {
            Notification.Notify(NotificationSeverity.Error, "Error", ex.Message);
        }
    }
}