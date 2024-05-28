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
    [Route("odata/DOOHDB/Attachments")]
    public partial class AttachmentsController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public AttachmentsController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.Attachment> GetAttachments()
        {
            var items = this.context.Attachments.AsQueryable<DOOH.Server.Models.DOOHDB.Attachment>();
            this.OnAttachmentsRead(ref items);

            return items;
        }

        partial void OnAttachmentsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Attachment> items);

        partial void OnAttachmentGet(ref SingleResult<DOOH.Server.Models.DOOHDB.Attachment> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/Attachments(AttachmentKey={AttachmentKey})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.Attachment> GetAttachment(string key)
        {
            var items = this.context.Attachments.Where(i => i.AttachmentKey == Uri.UnescapeDataString(key));
            var result = SingleResult.Create(items);

            OnAttachmentGet(ref result);

            return result;
        }
        partial void OnAttachmentDeleted(DOOH.Server.Models.DOOHDB.Attachment item);
        partial void OnAfterAttachmentDeleted(DOOH.Server.Models.DOOHDB.Attachment item);

        [HttpDelete("/odata/DOOHDB/Attachments(AttachmentKey={AttachmentKey})")]
        public IActionResult DeleteAttachment(string key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Attachments
                    .Where(i => i.AttachmentKey == Uri.UnescapeDataString(key))
                    .Include(i => i.AdboardModels)
                    .Include(i => i.Advertisements)
                    .Include(i => i.Brands)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.Attachment>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAttachmentDeleted(item);
                this.context.Attachments.Remove(item);
                this.context.SaveChanges();
                this.OnAfterAttachmentDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAttachmentUpdated(DOOH.Server.Models.DOOHDB.Attachment item);
        partial void OnAfterAttachmentUpdated(DOOH.Server.Models.DOOHDB.Attachment item);

        [HttpPut("/odata/DOOHDB/Attachments(AttachmentKey={AttachmentKey})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAttachment(string key, [FromBody]DOOH.Server.Models.DOOHDB.Attachment item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Attachments
                    .Where(i => i.AttachmentKey == Uri.UnescapeDataString(key))
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.Attachment>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAttachmentUpdated(item);
                this.context.Attachments.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Attachments.Where(i => i.AttachmentKey == Uri.UnescapeDataString(key));
                
                this.OnAfterAttachmentUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/Attachments(AttachmentKey={AttachmentKey})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchAttachment(string key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.Attachment> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Attachments
                    .Where(i => i.AttachmentKey == Uri.UnescapeDataString(key))
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.Attachment>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnAttachmentUpdated(item);
                this.context.Attachments.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Attachments.Where(i => i.AttachmentKey == Uri.UnescapeDataString(key));
                
                this.OnAfterAttachmentUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAttachmentCreated(DOOH.Server.Models.DOOHDB.Attachment item);
        partial void OnAfterAttachmentCreated(DOOH.Server.Models.DOOHDB.Attachment item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.Attachment item)
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

                this.OnAttachmentCreated(item);
                this.context.Attachments.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Attachments.Where(i => i.AttachmentKey == item.AttachmentKey);

                

                this.OnAfterAttachmentCreated(item);

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
