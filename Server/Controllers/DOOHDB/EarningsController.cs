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
    [Route("odata/DOOHDB/Earnings")]
    public partial class EarningsController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public EarningsController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.Earning> GetEarnings()
        {
            var items = this.context.Earnings.AsQueryable<DOOH.Server.Models.DOOHDB.Earning>();
            this.OnEarningsRead(ref items);

            return items;
        }

        partial void OnEarningsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Earning> items);

        partial void OnEarningGet(ref SingleResult<DOOH.Server.Models.DOOHDB.Earning> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/Earnings(EarningId={EarningId})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.Earning> GetEarning(int key)
        {
            var items = this.context.Earnings.Where(i => i.EarningId == key);
            var result = SingleResult.Create(items);

            OnEarningGet(ref result);

            return result;
        }
        partial void OnEarningDeleted(DOOH.Server.Models.DOOHDB.Earning item);
        partial void OnAfterEarningDeleted(DOOH.Server.Models.DOOHDB.Earning item);

        [HttpDelete("/odata/DOOHDB/Earnings(EarningId={EarningId})")]
        public IActionResult DeleteEarning(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Earnings
                    .Where(i => i.EarningId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.Earning>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnEarningDeleted(item);
                this.context.Earnings.Remove(item);
                this.context.SaveChanges();
                this.OnAfterEarningDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnEarningUpdated(DOOH.Server.Models.DOOHDB.Earning item);
        partial void OnAfterEarningUpdated(DOOH.Server.Models.DOOHDB.Earning item);

        [HttpPut("/odata/DOOHDB/Earnings(EarningId={EarningId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutEarning(int key, [FromBody]DOOH.Server.Models.DOOHDB.Earning item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Earnings
                    .Where(i => i.EarningId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.Earning>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnEarningUpdated(item);
                this.context.Earnings.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Earnings.Where(i => i.EarningId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Adboard,Analytic,Provider");
                this.OnAfterEarningUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/Earnings(EarningId={EarningId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchEarning(int key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.Earning> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Earnings
                    .Where(i => i.EarningId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.Earning>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnEarningUpdated(item);
                this.context.Earnings.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Earnings.Where(i => i.EarningId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Adboard,Analytic,Provider");
                this.OnAfterEarningUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnEarningCreated(DOOH.Server.Models.DOOHDB.Earning item);
        partial void OnAfterEarningCreated(DOOH.Server.Models.DOOHDB.Earning item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.Earning item)
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

                this.OnEarningCreated(item);
                this.context.Earnings.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Earnings.Where(i => i.EarningId == item.EarningId);

                Request.QueryString = Request.QueryString.Add("$expand", "Adboard,Analytic,Provider");

                this.OnAfterEarningCreated(item);

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
