// using System.Text.RegularExpressions;
// using DOOH.Client.Components;
// using DOOH.Client.Extensions;
// using DOOH.Server.Models.DOOHDB;
// using Microsoft.AspNetCore.Components;
// using Microsoft.AspNetCore.Components.Web;
// using Microsoft.JSInterop;
// using Radzen;
// using Radzen.Blazor;
//
// namespace DOOH.Client.Pages.Admin.Campaigns.Editor;
//
// public partial class ImportFromUploads
// {
//     [Inject] protected DOOHDBService DoohdbService { get; set; }
//
//     [Inject] protected NotificationService NotificationService { get; set; }
//
//     [Inject] protected DialogService DialogService { get; set; }
//
//     [Inject] protected SecurityService SecurityService { get; set; }
//     
//
//     private RadzenDataList<DOOH.Server.Models.DOOHDB.Upload> list0;
//
//     private IEnumerable<DOOH.Server.Models.DOOHDB.Upload> uploads;
//
//     private int uploadsCount;
//
//     private async Task UploadsLoadData(LoadDataArgs args)
//     {
//         try
//         {
//             var isAdmin = SecurityService.IsInRole("Admin");
//             var userId = SecurityService.User?.Id ?? SecurityService.User?.UserName;
//             var orderBy = $"CreatedAt desc";
//             var filter = isAdmin ? string.Empty : $"Owner eq '{userId}'";
//             var result = await DoohdbService.GetUploads(top: args.Top, skip: args.Skip,
//                 count: args.Top != null && args.Skip != null, filter: filter, orderby: orderBy);
//
//             uploads = result.Value.AsODataEnumerable();
//             uploadsCount = result.Count;
//         }
//         catch (Exception)
//         {
//             NotificationService.Notify(new NotificationMessage
//                 { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to load" });
//         }
//     }
//
//     private async Task PlayClick(MouseEventArgs args, Upload upload)
//     {
//         var url = upload.GetUrl();
//         await DialogService.OpenAsync<Player>($"{upload.Key.Replace("\\", "/").Split('/').LastOrDefault()}",
//             new Dictionary<string, object>() { { "Src", url } }, new DialogOptions() { Width = "400px" });
//     }
//
//
//     private async Task DeleteClick(MouseEventArgs args, Upload upload)
//     {
//         try
//         {
//             if (await DialogService.Confirm("Are you sure you want to delete this upload?") == true)
//             {
//                 var result = await DoohdbService.DeleteUpload(upload.Key);
//                 if (result != null)
//                 {
//                     NotificationService.Notify(new NotificationMessage
//                     {
//                         Severity = NotificationSeverity.Success, Summary = "Success",
//                         Detail = "Upload deleted successfully"
//                     });
//                 }
//             }
//         }
//         catch (Exception)
//         {
//             NotificationService.Notify(new NotificationMessage
//                 { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Unable to delete" });
//         }
//         finally
//         {
//             await list0.Reload();
//         }
//     }
//
//     private async Task ImportClick(MouseEventArgs args, Upload upload)
//     {
//         DialogService.Close(upload);
//     }
//
//
//     private RadzenUpload upload;
//     private RadzenUpload uploadDd;
//
//     private int progress;
//     private bool showProgress;
//     private bool cancelUpload;
//
//     private void OnChange(UploadChangeEventArgs args)
//     {
//         foreach (var file in args.Files)
//         {
//             Console.WriteLine(file.Name);
//             //console.Log($"File: {file.Name} / {file.Size} bytes");
//         }
//
//         //console.Log($"{name} changed");
//         Console.WriteLine($"{args.Files.Count()} files selected");
//     }
//
//     private void OnProgress(UploadProgressArgs args)
//     {
//         showProgress = true;
//         progress = args.Progress;
//         // progressMessage = $"{args.Progress}% / {args.Loaded} of {args.Total} bytes.";
//         args.Cancel = cancelUpload;
//         if (cancelUpload)
//         {
//             showProgress = false;
//             NotificationService.Notify(new NotificationMessage
//             {
//                 Severity = NotificationSeverity.Warning, Summary = "Warning",
//                 Detail = "Upload cancelled"
//             });
//         }
//         StateHasChanged();
//     }
//
//     private void OnComplete(UploadCompleteEventArgs args)
//     {
//         showProgress = false;
//         progress = 100;
//         // progressMessage = !args.Cancelled ? "Upload Complete!" : "Upload Cancelled!";
//
//         var rawResponse = args.RawResponse;
//         Console.WriteLine(rawResponse);
//         list0.Reload();
//         NotificationService.Notify(new NotificationMessage
//         {
//             Severity = NotificationSeverity.Success, Summary = "Success",
//             Detail = "Upload completed successfully"
//         });
//         StateHasChanged();
//     }
//     
//     private void OnError(UploadErrorEventArgs args)
//     {
//         // args.Message
//         /*
//          Drag and drop here or click to choose files
//            Error {"type":"https://tools.ietf.org/html/rfc9110#section-15.5.1","title":"One or more validation errors occurred.","status":400,"errors":{"":["Failed to read the request form. Request body too large. The max request body size is 30000000 bytes."]},"traceId":"00-e2b33179b9477f55d0fd84643b5efa4d-60c85108740884bf-00"}
//          */
//         // Exract the error message 'Failed to read the request form. Request body too large. The max request body size is 30000000 bytes.'
//         var error = args.Message;
//         // Filter with regex
//         var match = Regex.Match(error, @"(?<=\["").*?(?=""\])");
//         var message = match.Success ? match.Value : error;
//         NotificationService.Notify(new NotificationMessage
//         {
//             Severity = NotificationSeverity.Error, Summary = "Error",
//             Detail = message
//         });
//         showProgress = false;
//         StateHasChanged();
//     }
// }
