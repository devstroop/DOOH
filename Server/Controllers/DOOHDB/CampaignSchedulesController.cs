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
    [Route("odata/DOOHDB/CampaignSchedules")]
    public partial class CampaignSchedulesController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public CampaignSchedulesController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.CampaignSchedule> GetCampaignSchedules()
        {
            var items = this.context.CampaignSchedules.AsQueryable<DOOH.Server.Models.DOOHDB.CampaignSchedule>();
            this.OnCampaignSchedulesRead(ref items);

            return items;
        }

        partial void OnCampaignSchedulesRead(ref IQueryable<DOOH.Server.Models.DOOHDB.CampaignSchedule> items);

        partial void OnCampaignScheduleGet(ref SingleResult<DOOH.Server.Models.DOOHDB.CampaignSchedule> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/CampaignSchedules(ScheduleId={ScheduleId})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.CampaignSchedule> GetCampaignSchedule(int key)
        {
            var items = this.context.CampaignSchedules.Where(i => i.ScheduleId == key);
            var result = SingleResult.Create(items);

            OnCampaignScheduleGet(ref result);

            return result;
        }
        partial void OnCampaignScheduleDeleted(DOOH.Server.Models.DOOHDB.CampaignSchedule item);
        partial void OnAfterCampaignScheduleDeleted(DOOH.Server.Models.DOOHDB.CampaignSchedule item);

        [HttpDelete("/odata/DOOHDB/CampaignSchedules(ScheduleId={ScheduleId})")]
        public IActionResult DeleteCampaignSchedule(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.CampaignSchedules
                    .Where(i => i.ScheduleId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnCampaignScheduleDeleted(item);
                this.context.CampaignSchedules.Remove(item);
                this.context.SaveChanges();
                this.OnAfterCampaignScheduleDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnCampaignScheduleUpdated(DOOH.Server.Models.DOOHDB.CampaignSchedule item);
        partial void OnAfterCampaignScheduleUpdated(DOOH.Server.Models.DOOHDB.CampaignSchedule item);

        [HttpPut("/odata/DOOHDB/CampaignSchedules(ScheduleId={ScheduleId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutCampaignSchedule(int key, [FromBody]DOOH.Server.Models.DOOHDB.CampaignSchedule item)
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
                this.OnCampaignScheduleUpdated(item);
                this.context.CampaignSchedules.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.CampaignSchedules.Where(i => i.ScheduleId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Campaign");
                this.OnAfterCampaignScheduleUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/CampaignSchedules(ScheduleId={ScheduleId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchCampaignSchedule(int key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.CampaignSchedule> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.CampaignSchedules.Where(i => i.ScheduleId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnCampaignScheduleUpdated(item);
                this.context.CampaignSchedules.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.CampaignSchedules.Where(i => i.ScheduleId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Campaign");
                this.OnAfterCampaignScheduleUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnCampaignScheduleCreated(DOOH.Server.Models.DOOHDB.CampaignSchedule item);
        partial void OnAfterCampaignScheduleCreated(DOOH.Server.Models.DOOHDB.CampaignSchedule item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.CampaignSchedule item)
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

                this.OnCampaignScheduleCreated(item);
                this.context.CampaignSchedules.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.CampaignSchedules.Where(i => i.ScheduleId == item.ScheduleId);

                Request.QueryString = Request.QueryString.Add("$expand", "Campaign");

                this.OnAfterCampaignScheduleCreated(item);

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
