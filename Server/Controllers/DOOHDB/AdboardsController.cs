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
using Microsoft.AspNetCore.Authorization;

namespace DOOH.Server.Controllers.DOOHDB
{
    [Route("odata/DOOHDB/Adboards")]
    public partial class AdboardsController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public AdboardsController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }


        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.Adboard> GetAdboards()
        {
            var items = this.context.Adboards.AsQueryable<DOOH.Server.Models.DOOHDB.Adboard>();
            this.OnAdboardsRead(ref items);

            return items;
        }

        partial void OnAdboardsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Adboard> items);

        partial void OnAdboardGet(ref SingleResult<DOOH.Server.Models.DOOHDB.Adboard> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/Adboards(AdboardId={AdboardId})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.Adboard> GetAdboard(int key)
        {
            var items = this.context.Adboards.Where(i => i.AdboardId == key);
            var result = SingleResult.Create(items);

            OnAdboardGet(ref result);

            return result;
        }
        partial void OnAdboardDeleted(DOOH.Server.Models.DOOHDB.Adboard item);
        partial void OnAfterAdboardDeleted(DOOH.Server.Models.DOOHDB.Adboard item);

        [HttpDelete("/odata/DOOHDB/Adboards(AdboardId={AdboardId})")]
        public IActionResult DeleteAdboard(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Adboards
                    .Where(i => i.AdboardId == key)
                    .Include(i => i.AdboardNetworks)
                    .Include(i => i.AdboardTokens)
                    .Include(i => i.AdboardWifis)
                    .Include(i => i.Analytics)
                    .Include(i => i.CampaignAdboards)
                    .Include(i => i.Earnings)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.Adboard>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAdboardDeleted(item);
                this.context.Adboards.Remove(item);
                this.context.SaveChanges();
                this.OnAfterAdboardDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAdboardUpdated(DOOH.Server.Models.DOOHDB.Adboard item);
        partial void OnAfterAdboardUpdated(DOOH.Server.Models.DOOHDB.Adboard item);

        [HttpPut("/odata/DOOHDB/Adboards(AdboardId={AdboardId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAdboard(int key, [FromBody]DOOH.Server.Models.DOOHDB.Adboard item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Adboards
                    .Where(i => i.AdboardId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.Adboard>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAdboardUpdated(item);
                this.context.Adboards.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Adboards.Where(i => i.AdboardId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "AdboardModel,Attachment,Category,City,Country,Provider,State");
                this.OnAfterAdboardUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/Adboards(AdboardId={AdboardId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchAdboard(int key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.Adboard> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Adboards
                    .Where(i => i.AdboardId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.Adboard>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnAdboardUpdated(item);
                this.context.Adboards.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Adboards.Where(i => i.AdboardId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "AdboardModel,Attachment,Category,City,Country,Provider,State");
                this.OnAfterAdboardUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAdboardCreated(DOOH.Server.Models.DOOHDB.Adboard item);
        partial void OnAfterAdboardCreated(DOOH.Server.Models.DOOHDB.Adboard item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.Adboard item)
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

                this.OnAdboardCreated(item);
                this.context.Adboards.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Adboards.Where(i => i.AdboardId == item.AdboardId);

                Request.QueryString = Request.QueryString.Add("$expand", "AdboardModel,Attachment,Category,City,Country,Provider,State");

                this.OnAfterAdboardCreated(item);

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
