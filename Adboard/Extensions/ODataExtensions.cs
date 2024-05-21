using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DOOH.Adboard.Extensions
{
    public static class ODataExtensions
    {
        public static Uri GetODataUri(this Uri uri, string filter = null, int? top = null, int? skip = null, string orderby = null, string expand = null, string select = null, bool? count = null)
        {
            UriBuilder uriBuilder = new UriBuilder(uri);
            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(uriBuilder.Query);
            if (!string.IsNullOrEmpty(filter))
            {
                nameValueCollection["$filter"] = filter.Replace("\"", "'") ?? "";
            }

            if (top.HasValue)
            {
                nameValueCollection["$top"] = $"{top}";
            }

            if (skip.HasValue)
            {
                nameValueCollection["$skip"] = $"{skip}";
            }

            if (!string.IsNullOrEmpty(orderby))
            {
                nameValueCollection["$orderby"] = orderby ?? "";
            }

            if (!string.IsNullOrEmpty(expand))
            {
                nameValueCollection["$expand"] = expand ?? "";
            }

            if (!string.IsNullOrEmpty(select))
            {
                nameValueCollection["$select"] = select ?? "";
            }

            if (count.HasValue)
            {
                nameValueCollection["$count"] = $"{count}".ToLower();
            }

            uriBuilder.Query = nameValueCollection.ToString();
            return uriBuilder.Uri;
        }
    }
}
