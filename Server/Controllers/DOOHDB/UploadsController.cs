using System;
using System.Net;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DOOH.Server.Controllers.DOOHDB
{
    [Route("odata/DOOHDB/Uploads")]
    public partial class UploadsController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public UploadsController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.Upload> GetUploads()
        {
            var items = this.context.Uploads.AsQueryable<DOOH.Server.Models.DOOHDB.Upload>();
            this.OnUploadsRead(ref items);

            return items;
        }

        partial void OnUploadsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Upload> items);

        partial void OnUploadGet(ref SingleResult<DOOH.Server.Models.DOOHDB.Upload> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/Uploads(Key={Key})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.Upload> GetUpload(string key)
        {
            var items = this.context.Uploads.Where(i => i.Key == Uri.UnescapeDataString(key));
            var result = SingleResult.Create(items);

            OnUploadGet(ref result);

            return result;
        }
        partial void OnUploadDeleted(DOOH.Server.Models.DOOHDB.Upload item);
        partial void OnAfterUploadDeleted(DOOH.Server.Models.DOOHDB.Upload item);

        [HttpDelete("/odata/DOOHDB/Uploads(Key={Key})")]
        public IActionResult DeleteUpload(string key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Uploads
                    .Where(i => i.Key == Uri.UnescapeDataString(key))
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnUploadDeleted(item);
                this.context.Uploads.Remove(item);
                this.context.SaveChanges();
                this.OnAfterUploadDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnUploadUpdated(DOOH.Server.Models.DOOHDB.Upload item);
        partial void OnAfterUploadUpdated(DOOH.Server.Models.DOOHDB.Upload item);

        [HttpPut("/odata/DOOHDB/Uploads(Key={Key})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutUpload(string key, [FromBody]DOOH.Server.Models.DOOHDB.Upload item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.Key != Uri.UnescapeDataString(key)))
                {
                    return BadRequest();
                }
                this.OnUploadUpdated(item);
                this.context.Uploads.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Uploads.Where(i => i.Key == Uri.UnescapeDataString(key));
                Request.QueryString = Request.QueryString.Add("$expand", "UserInformation");
                this.OnAfterUploadUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/Uploads(Key={Key})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchUpload(string key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.Upload> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Uploads.Where(i => i.Key == Uri.UnescapeDataString(key)).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnUploadUpdated(item);
                this.context.Uploads.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Uploads.Where(i => i.Key == Uri.UnescapeDataString(key));
                Request.QueryString = Request.QueryString.Add("$expand", "UserInformation");
                this.OnAfterUploadUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnUploadCreated(DOOH.Server.Models.DOOHDB.Upload item);
        partial void OnAfterUploadCreated(DOOH.Server.Models.DOOHDB.Upload item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.Upload item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null)
                {
                    return BadRequest();
                }

                this.OnUploadCreated(item);
                this.context.Uploads.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Uploads.Where(i => i.Key == item.Key);

                Request.QueryString = Request.QueryString.Add("$expand", "UserInformation");

                this.OnAfterUploadCreated(item);

                return new ObjectResult(SingleResult.Create(itemToReturn))
                {
                    StatusCode = 201
                };
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
