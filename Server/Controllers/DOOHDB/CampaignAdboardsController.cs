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
    [Route("odata/DOOHDB/CampaignAdboards")]
    public partial class CampaignAdboardsController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public CampaignAdboardsController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.CampaignAdboard> GetCampaignAdboards()
        {
            var items = this.context.CampaignAdboards.AsQueryable<DOOH.Server.Models.DOOHDB.CampaignAdboard>();
            this.OnCampaignAdboardsRead(ref items);

            return items;
        }

        partial void OnCampaignAdboardsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.CampaignAdboard> items);

        partial void OnCampaignAdboardGet(ref SingleResult<DOOH.Server.Models.DOOHDB.CampaignAdboard> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/CampaignAdboards(CampaignId={keyCampaignId},AdboardId={keyAdboardId})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.CampaignAdboard> GetCampaignAdboard([FromODataUri] int keyCampaignId, [FromODataUri] int keyAdboardId)
        {
            var items = this.context.CampaignAdboards.Where(i => i.CampaignId == keyCampaignId && i.AdboardId == keyAdboardId);
            var result = SingleResult.Create(items);

            OnCampaignAdboardGet(ref result);

            return result;
        }
        partial void OnCampaignAdboardDeleted(DOOH.Server.Models.DOOHDB.CampaignAdboard item);
        partial void OnAfterCampaignAdboardDeleted(DOOH.Server.Models.DOOHDB.CampaignAdboard item);

        [HttpDelete("/odata/DOOHDB/CampaignAdboards(CampaignId={keyCampaignId},AdboardId={keyAdboardId})")]
        public IActionResult DeleteCampaignAdboard([FromODataUri] int keyCampaignId, [FromODataUri] int keyAdboardId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.CampaignAdboards
                    .Where(i => i.CampaignId == keyCampaignId && i.AdboardId == keyAdboardId)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnCampaignAdboardDeleted(item);
                this.context.CampaignAdboards.Remove(item);
                this.context.SaveChanges();
                this.OnAfterCampaignAdboardDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnCampaignAdboardUpdated(DOOH.Server.Models.DOOHDB.CampaignAdboard item);
        partial void OnAfterCampaignAdboardUpdated(DOOH.Server.Models.DOOHDB.CampaignAdboard item);

        [HttpPut("/odata/DOOHDB/CampaignAdboards(CampaignId={keyCampaignId},AdboardId={keyAdboardId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutCampaignAdboard([FromODataUri] int keyCampaignId, [FromODataUri] int keyAdboardId, [FromBody]DOOH.Server.Models.DOOHDB.CampaignAdboard item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.CampaignId != keyCampaignId && item.AdboardId != keyAdboardId))
                {
                    return BadRequest();
                }
                this.OnCampaignAdboardUpdated(item);
                this.context.CampaignAdboards.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.CampaignAdboards.Where(i => i.CampaignId == keyCampaignId && i.AdboardId == keyAdboardId);
                Request.QueryString = Request.QueryString.Add("$expand", "Adboard,Campaign");
                this.OnAfterCampaignAdboardUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/CampaignAdboards(CampaignId={keyCampaignId},AdboardId={keyAdboardId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchCampaignAdboard([FromODataUri] int keyCampaignId, [FromODataUri] int keyAdboardId, [FromBody]Delta<DOOH.Server.Models.DOOHDB.CampaignAdboard> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.CampaignAdboards.Where(i => i.CampaignId == keyCampaignId && i.AdboardId == keyAdboardId).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnCampaignAdboardUpdated(item);
                this.context.CampaignAdboards.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.CampaignAdboards.Where(i => i.CampaignId == keyCampaignId && i.AdboardId == keyAdboardId);
                Request.QueryString = Request.QueryString.Add("$expand", "Adboard,Campaign");
                this.OnAfterCampaignAdboardUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnCampaignAdboardCreated(DOOH.Server.Models.DOOHDB.CampaignAdboard item);
        partial void OnAfterCampaignAdboardCreated(DOOH.Server.Models.DOOHDB.CampaignAdboard item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.CampaignAdboard item)
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

                this.OnCampaignAdboardCreated(item);
                this.context.CampaignAdboards.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.CampaignAdboards.Where(i => i.CampaignId == item.CampaignId && i.AdboardId == item.AdboardId);

                Request.QueryString = Request.QueryString.Add("$expand", "Adboard,Campaign");

                this.OnAfterCampaignAdboardCreated(item);

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
