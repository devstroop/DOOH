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
    [Route("odata/DOOHDB/ScheduleCampaignAdboards")]
    public partial class ScheduleCampaignAdboardsController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public ScheduleCampaignAdboardsController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.ScheduleCampaignAdboard> GetScheduleCampaignAdboards()
        {
            var items = this.context.ScheduleCampaignAdboards.AsQueryable<DOOH.Server.Models.DOOHDB.ScheduleCampaignAdboard>();
            this.OnScheduleCampaignAdboardsRead(ref items);

            return items;
        }

        partial void OnScheduleCampaignAdboardsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.ScheduleCampaignAdboard> items);

        partial void OnScheduleCampaignAdboardGet(ref SingleResult<DOOH.Server.Models.DOOHDB.ScheduleCampaignAdboard> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/ScheduleCampaignAdboards(ScheduleId={keyScheduleId},AdboardId={keyAdboardId},CampaignId={keyCampaignId})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.ScheduleCampaignAdboard> GetScheduleCampaignAdboard([FromODataUri] int keyScheduleId, [FromODataUri] int keyAdboardId, [FromODataUri] int keyCampaignId)
        {
            var items = this.context.ScheduleCampaignAdboards.Where(i => i.ScheduleId == keyScheduleId && i.AdboardId == keyAdboardId && i.CampaignId == keyCampaignId);
            var result = SingleResult.Create(items);

            OnScheduleCampaignAdboardGet(ref result);

            return result;
        }
        partial void OnScheduleCampaignAdboardDeleted(DOOH.Server.Models.DOOHDB.ScheduleCampaignAdboard item);
        partial void OnAfterScheduleCampaignAdboardDeleted(DOOH.Server.Models.DOOHDB.ScheduleCampaignAdboard item);

        [HttpDelete("/odata/DOOHDB/ScheduleCampaignAdboards(ScheduleId={keyScheduleId},AdboardId={keyAdboardId},CampaignId={keyCampaignId})")]
        public IActionResult DeleteScheduleCampaignAdboard([FromODataUri] int keyScheduleId, [FromODataUri] int keyAdboardId, [FromODataUri] int keyCampaignId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.ScheduleCampaignAdboards
                    .Where(i => i.ScheduleId == keyScheduleId && i.AdboardId == keyAdboardId && i.CampaignId == keyCampaignId)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnScheduleCampaignAdboardDeleted(item);
                this.context.ScheduleCampaignAdboards.Remove(item);
                this.context.SaveChanges();
                this.OnAfterScheduleCampaignAdboardDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnScheduleCampaignAdboardUpdated(DOOH.Server.Models.DOOHDB.ScheduleCampaignAdboard item);
        partial void OnAfterScheduleCampaignAdboardUpdated(DOOH.Server.Models.DOOHDB.ScheduleCampaignAdboard item);

        [HttpPut("/odata/DOOHDB/ScheduleCampaignAdboards(ScheduleId={keyScheduleId},AdboardId={keyAdboardId},CampaignId={keyCampaignId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutScheduleCampaignAdboard([FromODataUri] int keyScheduleId, [FromODataUri] int keyAdboardId, [FromODataUri] int keyCampaignId, [FromBody]DOOH.Server.Models.DOOHDB.ScheduleCampaignAdboard item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.ScheduleId != keyScheduleId && item.AdboardId != keyAdboardId && item.CampaignId != keyCampaignId))
                {
                    return BadRequest();
                }
                this.OnScheduleCampaignAdboardUpdated(item);
                this.context.ScheduleCampaignAdboards.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ScheduleCampaignAdboards.Where(i => i.ScheduleId == keyScheduleId && i.AdboardId == keyAdboardId && i.CampaignId == keyCampaignId);
                Request.QueryString = Request.QueryString.Add("$expand", "Schedule,CampaignAdboard");
                this.OnAfterScheduleCampaignAdboardUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/ScheduleCampaignAdboards(ScheduleId={keyScheduleId},AdboardId={keyAdboardId},CampaignId={keyCampaignId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchScheduleCampaignAdboard([FromODataUri] int keyScheduleId, [FromODataUri] int keyAdboardId, [FromODataUri] int keyCampaignId, [FromBody]Delta<DOOH.Server.Models.DOOHDB.ScheduleCampaignAdboard> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.ScheduleCampaignAdboards.Where(i => i.ScheduleId == keyScheduleId && i.AdboardId == keyAdboardId && i.CampaignId == keyCampaignId).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnScheduleCampaignAdboardUpdated(item);
                this.context.ScheduleCampaignAdboards.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ScheduleCampaignAdboards.Where(i => i.ScheduleId == keyScheduleId && i.AdboardId == keyAdboardId && i.CampaignId == keyCampaignId);
                Request.QueryString = Request.QueryString.Add("$expand", "Schedule,CampaignAdboard");
                this.OnAfterScheduleCampaignAdboardUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnScheduleCampaignAdboardCreated(DOOH.Server.Models.DOOHDB.ScheduleCampaignAdboard item);
        partial void OnAfterScheduleCampaignAdboardCreated(DOOH.Server.Models.DOOHDB.ScheduleCampaignAdboard item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.ScheduleCampaignAdboard item)
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

                this.OnScheduleCampaignAdboardCreated(item);
                this.context.ScheduleCampaignAdboards.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ScheduleCampaignAdboards.Where(i => i.ScheduleId == item.ScheduleId && i.AdboardId == item.AdboardId && i.CampaignId == item.CampaignId);

                Request.QueryString = Request.QueryString.Add("$expand", "Schedule,CampaignAdboard");

                this.OnAfterScheduleCampaignAdboardCreated(item);

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
