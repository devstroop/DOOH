using DOOH.Server.Models.DOOHDB;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace DOOH.Client.Pages.Admin.Campaigns.Editor;

public partial class ImportAttachmment
{
    [Parameter]
    public EventCallback<Attachment> Import { get; set; }

    [Inject]
    protected DOOHDBService DoohdbService { get; set; }
    
    [Inject]
    protected NotificationService NotificationService { get; set; }
    
    [Inject]
    protected DialogService DialogService { get; set; }
    
    [Inject]
    protected SecurityService SecurityService { get; set; }

    private IEnumerable<DOOH.Server.Models.DOOHDB.Attachment> attachments;

    private int attachmentsCount;

    private async Task AttachmentsLoadData(LoadDataArgs args)
    {
        try
        {
            var orderBy = $"CreatedAt desc";
            var result = await DoohdbService.GetAttachments(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: args.Filter, orderby: orderBy);
    
            attachments = result.Value.AsODataEnumerable();
            attachmentsCount = result.Count;
        }
        catch (Exception)
        {
            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load" });
        }
    }
    
    private void OnHoverClick(string args)
    {
        NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Info, Summary = "Hover", Detail = args });
    }

}