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
    [Route("odata/DOOHDB/Schedules")]
    public partial class SchedulesController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public SchedulesController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.Schedule> GetSchedules()
        {
            var items = this.context.Schedules.AsQueryable<DOOH.Server.Models.DOOHDB.Schedule>();
            this.OnSchedulesRead(ref items);

            return items;
        }

        partial void OnSchedulesRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Schedule> items);

        partial void OnScheduleGet(ref SingleResult<DOOH.Server.Models.DOOHDB.Schedule> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/Schedules(ScheduleId={ScheduleId})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.Schedule> GetSchedule(int key)
        {
            var items = this.context.Schedules.Where(i => i.ScheduleId == key);
            var result = SingleResult.Create(items);

            OnScheduleGet(ref result);

            return result;
        }
        partial void OnScheduleDeleted(DOOH.Server.Models.DOOHDB.Schedule item);
        partial void OnAfterScheduleDeleted(DOOH.Server.Models.DOOHDB.Schedule item);

        [HttpDelete("/odata/DOOHDB/Schedules(ScheduleId={ScheduleId})")]
        public IActionResult DeleteSchedule(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Schedules
                    .Where(i => i.ScheduleId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnScheduleDeleted(item);
                this.context.Schedules.Remove(item);
                this.context.SaveChanges();
                this.OnAfterScheduleDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnScheduleUpdated(DOOH.Server.Models.DOOHDB.Schedule item);
        partial void OnAfterScheduleUpdated(DOOH.Server.Models.DOOHDB.Schedule item);

        [HttpPut("/odata/DOOHDB/Schedules(ScheduleId={ScheduleId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutSchedule(int key, [FromBody]DOOH.Server.Models.DOOHDB.Schedule item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.ScheduleId != key))
                {
                    return BadRequest();
                }
                this.OnScheduleUpdated(item);
                this.context.Schedules.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Schedules.Where(i => i.ScheduleId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Campaign");
                this.OnAfterScheduleUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/Schedules(ScheduleId={ScheduleId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchSchedule(int key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.Schedule> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Schedules.Where(i => i.ScheduleId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnScheduleUpdated(item);
                this.context.Schedules.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Schedules.Where(i => i.ScheduleId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Campaign");
                this.OnAfterScheduleUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnScheduleCreated(DOOH.Server.Models.DOOHDB.Schedule item);
        partial void OnAfterScheduleCreated(DOOH.Server.Models.DOOHDB.Schedule item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.Schedule item)
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

                this.OnScheduleCreated(item);
                this.context.Schedules.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Schedules.Where(i => i.ScheduleId == item.ScheduleId);

                Request.QueryString = Request.QueryString.Add("$expand", "Campaign");

                this.OnAfterScheduleCreated(item);

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
