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
    [Route("odata/DOOHDB/Analytics")]
    public partial class AnalyticsController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public AnalyticsController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.Analytic> GetAnalytics()
        {
            var items = this.context.Analytics.AsQueryable<DOOH.Server.Models.DOOHDB.Analytic>();
            this.OnAnalyticsRead(ref items);

            return items;
        }

        partial void OnAnalyticsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Analytic> items);

        partial void OnAnalyticGet(ref SingleResult<DOOH.Server.Models.DOOHDB.Analytic> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/Analytics(AnalyticId={AnalyticId})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.Analytic> GetAnalytic(int key)
        {
            var items = this.context.Analytics.Where(i => i.AnalyticId == key);
            var result = SingleResult.Create(items);

            OnAnalyticGet(ref result);

            return result;
        }
        partial void OnAnalyticDeleted(DOOH.Server.Models.DOOHDB.Analytic item);
        partial void OnAfterAnalyticDeleted(DOOH.Server.Models.DOOHDB.Analytic item);

        [HttpDelete("/odata/DOOHDB/Analytics(AnalyticId={AnalyticId})")]
        public IActionResult DeleteAnalytic(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Analytics
                    .Where(i => i.AnalyticId == key)
                    .Include(i => i.Billings)
                    .Include(i => i.Earnings)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.Analytic>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAnalyticDeleted(item);
                this.context.Analytics.Remove(item);
                this.context.SaveChanges();
                this.OnAfterAnalyticDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAnalyticUpdated(DOOH.Server.Models.DOOHDB.Analytic item);
        partial void OnAfterAnalyticUpdated(DOOH.Server.Models.DOOHDB.Analytic item);

        [HttpPut("/odata/DOOHDB/Analytics(AnalyticId={AnalyticId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAnalytic(int key, [FromBody]DOOH.Server.Models.DOOHDB.Analytic item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Analytics
                    .Where(i => i.AnalyticId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.Analytic>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAnalyticUpdated(item);
                this.context.Analytics.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Analytics.Where(i => i.AnalyticId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Adboard,Advertisement");
                this.OnAfterAnalyticUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/Analytics(AnalyticId={AnalyticId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchAnalytic(int key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.Analytic> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Analytics
                    .Where(i => i.AnalyticId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.Analytic>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnAnalyticUpdated(item);
                this.context.Analytics.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Analytics.Where(i => i.AnalyticId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Adboard,Advertisement");
                this.OnAfterAnalyticUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAnalyticCreated(DOOH.Server.Models.DOOHDB.Analytic item);
        partial void OnAfterAnalyticCreated(DOOH.Server.Models.DOOHDB.Analytic item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.Analytic item)
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

                this.OnAnalyticCreated(item);
                this.context.Analytics.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Analytics.Where(i => i.AnalyticId == item.AnalyticId);

                Request.QueryString = Request.QueryString.Add("$expand", "Adboard,Advertisement");

                this.OnAfterAnalyticCreated(item);

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
