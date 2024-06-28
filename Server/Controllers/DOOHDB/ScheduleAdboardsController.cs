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
    [Route("odata/DOOHDB/ScheduleAdboards")]
    public partial class ScheduleAdboardsController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public ScheduleAdboardsController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.ScheduleAdboard> GetScheduleAdboards()
        {
            var items = this.context.ScheduleAdboards.AsQueryable<DOOH.Server.Models.DOOHDB.ScheduleAdboard>();
            this.OnScheduleAdboardsRead(ref items);

            return items;
        }

        partial void OnScheduleAdboardsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.ScheduleAdboard> items);

        partial void OnScheduleAdboardGet(ref SingleResult<DOOH.Server.Models.DOOHDB.ScheduleAdboard> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/ScheduleAdboards(ScheduleId={keyScheduleId},AdboardId={keyAdboardId})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.ScheduleAdboard> GetScheduleAdboard([FromODataUri] int keyScheduleId, [FromODataUri] int keyAdboardId)
        {
            var items = this.context.ScheduleAdboards.Where(i => i.ScheduleId == keyScheduleId && i.AdboardId == keyAdboardId);
            var result = SingleResult.Create(items);

            OnScheduleAdboardGet(ref result);

            return result;
        }
        partial void OnScheduleAdboardDeleted(DOOH.Server.Models.DOOHDB.ScheduleAdboard item);
        partial void OnAfterScheduleAdboardDeleted(DOOH.Server.Models.DOOHDB.ScheduleAdboard item);

        [HttpDelete("/odata/DOOHDB/ScheduleAdboards(ScheduleId={keyScheduleId},AdboardId={keyAdboardId})")]
        public IActionResult DeleteScheduleAdboard([FromODataUri] int keyScheduleId, [FromODataUri] int keyAdboardId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.ScheduleAdboards
                    .Where(i => i.ScheduleId == keyScheduleId && i.AdboardId == keyAdboardId)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnScheduleAdboardDeleted(item);
                this.context.ScheduleAdboards.Remove(item);
                this.context.SaveChanges();
                this.OnAfterScheduleAdboardDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnScheduleAdboardUpdated(DOOH.Server.Models.DOOHDB.ScheduleAdboard item);
        partial void OnAfterScheduleAdboardUpdated(DOOH.Server.Models.DOOHDB.ScheduleAdboard item);

        [HttpPut("/odata/DOOHDB/ScheduleAdboards(ScheduleId={keyScheduleId},AdboardId={keyAdboardId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutScheduleAdboard([FromODataUri] int keyScheduleId, [FromODataUri] int keyAdboardId, [FromBody]DOOH.Server.Models.DOOHDB.ScheduleAdboard item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.ScheduleId != keyScheduleId && item.AdboardId != keyAdboardId))
                {
                    return BadRequest();
                }
                this.OnScheduleAdboardUpdated(item);
                this.context.ScheduleAdboards.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ScheduleAdboards.Where(i => i.ScheduleId == keyScheduleId && i.AdboardId == keyAdboardId);
                Request.QueryString = Request.QueryString.Add("$expand", "Adboard,Schedule");
                this.OnAfterScheduleAdboardUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/ScheduleAdboards(ScheduleId={keyScheduleId},AdboardId={keyAdboardId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchScheduleAdboard([FromODataUri] int keyScheduleId, [FromODataUri] int keyAdboardId, [FromBody]Delta<DOOH.Server.Models.DOOHDB.ScheduleAdboard> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.ScheduleAdboards.Where(i => i.ScheduleId == keyScheduleId && i.AdboardId == keyAdboardId).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnScheduleAdboardUpdated(item);
                this.context.ScheduleAdboards.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ScheduleAdboards.Where(i => i.ScheduleId == keyScheduleId && i.AdboardId == keyAdboardId);
                Request.QueryString = Request.QueryString.Add("$expand", "Adboard,Schedule");
                this.OnAfterScheduleAdboardUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnScheduleAdboardCreated(DOOH.Server.Models.DOOHDB.ScheduleAdboard item);
        partial void OnAfterScheduleAdboardCreated(DOOH.Server.Models.DOOHDB.ScheduleAdboard item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.ScheduleAdboard item)
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

                this.OnScheduleAdboardCreated(item);
                this.context.ScheduleAdboards.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ScheduleAdboards.Where(i => i.ScheduleId == item.ScheduleId && i.AdboardId == item.AdboardId);

                Request.QueryString = Request.QueryString.Add("$expand", "Adboard,Schedule");

                this.OnAfterScheduleAdboardCreated(item);

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
