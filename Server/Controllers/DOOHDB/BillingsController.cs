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
    [Route("odata/DOOHDB/Billings")]
    public partial class BillingsController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public BillingsController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.Billing> GetBillings()
        {
            var items = this.context.Billings.AsQueryable<DOOH.Server.Models.DOOHDB.Billing>();
            this.OnBillingsRead(ref items);

            return items;
        }

        partial void OnBillingsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Billing> items);

        partial void OnBillingGet(ref SingleResult<DOOH.Server.Models.DOOHDB.Billing> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/Billings(BillingId={BillingId})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.Billing> GetBilling(int key)
        {
            var items = this.context.Billings.Where(i => i.BillingId == key);
            var result = SingleResult.Create(items);

            OnBillingGet(ref result);

            return result;
        }
        partial void OnBillingDeleted(DOOH.Server.Models.DOOHDB.Billing item);
        partial void OnAfterBillingDeleted(DOOH.Server.Models.DOOHDB.Billing item);

        [HttpDelete("/odata/DOOHDB/Billings(BillingId={BillingId})")]
        public IActionResult DeleteBilling(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Billings
                    .Where(i => i.BillingId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnBillingDeleted(item);
                this.context.Billings.Remove(item);
                this.context.SaveChanges();
                this.OnAfterBillingDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnBillingUpdated(DOOH.Server.Models.DOOHDB.Billing item);
        partial void OnAfterBillingUpdated(DOOH.Server.Models.DOOHDB.Billing item);

        [HttpPut("/odata/DOOHDB/Billings(BillingId={BillingId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutBilling(int key, [FromBody]DOOH.Server.Models.DOOHDB.Billing item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.BillingId != key))
                {
                    return BadRequest();
                }
                this.OnBillingUpdated(item);
                this.context.Billings.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Billings.Where(i => i.BillingId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Analytic");
                this.OnAfterBillingUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/Billings(BillingId={BillingId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchBilling(int key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.Billing> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Billings.Where(i => i.BillingId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnBillingUpdated(item);
                this.context.Billings.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Billings.Where(i => i.BillingId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Analytic");
                this.OnAfterBillingUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnBillingCreated(DOOH.Server.Models.DOOHDB.Billing item);
        partial void OnAfterBillingCreated(DOOH.Server.Models.DOOHDB.Billing item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.Billing item)
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

                this.OnBillingCreated(item);
                this.context.Billings.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Billings.Where(i => i.BillingId == item.BillingId);

                Request.QueryString = Request.QueryString.Add("$expand", "Analytic");

                this.OnAfterBillingCreated(item);

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
