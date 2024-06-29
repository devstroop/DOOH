using DOOH.Server.Models.Enums;

namespace DOOH.Server.Extensions;

public static class StatusExtensions
{
    public static string ToFriendlyString(this int status) => ((Status)status).ToFriendlyString();
    public static string ToFriendlyString(this Status status)
    {
        return status switch
        {
            Status.Draft => "Draft",
            Status.Pending => "Pending",
            Status.Active => "Active",
            Status.Completed => "Completed",
            Status.Cancelled => "Cancelled",
            Status.Suspended => "Suspended",
            _ => null
        };
    }
    
    public static Status? FromFriendlyString(string status)
    {
        return status switch
        {
            "Draft" => Status.Draft,
            "Pending" => Status.Pending,
            "Active" => Status.Active,
            "Completed" => Status.Completed,
            "Cancelled" => Status.Cancelled,
            "Suspended" => Status.Suspended,
            _ => null
        };
    }
    
    public static string GetCssClass(this int status) => ((Status)status).GetCssClass();
    public static string GetCssClass(this Status status)
    {
        return status switch
        {
            Status.Draft => "badge badge-info",
            Status.Pending => "badge badge-primary",
            Status.Active => "badge badge-success",
            Status.Completed => "badge badge-secondary",
            Status.Cancelled => "badge badge-danger",
            Status.Suspended => "badge badge-warning",
            _ => null
        };
    }
    
    public static string GetIcon(this int status) => ((Status)status).GetIcon();
    public static string GetIcon(this Status status)
    {
        return status switch
        {
            Status.Draft => "fiber_manual_record",
            Status.Pending => "fiber_manual_record",
            Status.Active => "fiber_manual_record",
            Status.Completed => "fiber_manual_record",
            Status.Cancelled => "fiber_manual_record",
            Status.Suspended => "fiber_manual_record",
            _ => null
        };
    }
    
    public static string GetColor(this int status) => ((Status)status).GetColor();
    public static string GetColor(this Status status)
    {
        return status switch
        {
            Status.Draft => "#17a2b8",
            Status.Pending => "#007bff",
            Status.Active => "#28a745",
            Status.Completed => "#6c757d",
            Status.Cancelled => "#dc3545",
            Status.Suspended => "#ffc107",
            _ => null
        };
    }
    
    public static bool IsDraft(this DOOH.Server.Models.Enums.Status status)
    {
        return (DOOH.Server.Models.Enums.Status)status == DOOH.Server.Models.Enums.Status.Draft;
    }
}