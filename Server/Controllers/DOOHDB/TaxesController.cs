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
    [Route("odata/DOOHDB/Taxes")]
    public partial class TaxesController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public TaxesController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.Tax> GetTaxes()
        {
            var items = this.context.Taxes.AsQueryable<DOOH.Server.Models.DOOHDB.Tax>();
            this.OnTaxesRead(ref items);

            return items;
        }

        partial void OnTaxesRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Tax> items);

        partial void OnTaxGet(ref SingleResult<DOOH.Server.Models.DOOHDB.Tax> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/Taxes(TaxId={TaxId})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.Tax> GetTax(int key)
        {
            var items = this.context.Taxes.Where(i => i.TaxId == key);
            var result = SingleResult.Create(items);

            OnTaxGet(ref result);

            return result;
        }
        partial void OnTaxDeleted(DOOH.Server.Models.DOOHDB.Tax item);
        partial void OnAfterTaxDeleted(DOOH.Server.Models.DOOHDB.Tax item);

        [HttpDelete("/odata/DOOHDB/Taxes(TaxId={TaxId})")]
        public IActionResult DeleteTax(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Taxes
                    .Where(i => i.TaxId == key)
                    .Include(i => i.Billings)
                    .Include(i => i.Taxes1)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.Tax>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnTaxDeleted(item);
                this.context.Taxes.Remove(item);
                this.context.SaveChanges();
                this.OnAfterTaxDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTaxUpdated(DOOH.Server.Models.DOOHDB.Tax item);
        partial void OnAfterTaxUpdated(DOOH.Server.Models.DOOHDB.Tax item);

        [HttpPut("/odata/DOOHDB/Taxes(TaxId={TaxId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutTax(int key, [FromBody]DOOH.Server.Models.DOOHDB.Tax item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Taxes
                    .Where(i => i.TaxId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.Tax>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnTaxUpdated(item);
                this.context.Taxes.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Taxes.Where(i => i.TaxId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Tax1");
                this.OnAfterTaxUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/Taxes(TaxId={TaxId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchTax(int key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.Tax> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Taxes
                    .Where(i => i.TaxId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.Tax>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnTaxUpdated(item);
                this.context.Taxes.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Taxes.Where(i => i.TaxId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Tax1");
                this.OnAfterTaxUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTaxCreated(DOOH.Server.Models.DOOHDB.Tax item);
        partial void OnAfterTaxCreated(DOOH.Server.Models.DOOHDB.Tax item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.Tax item)
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

                this.OnTaxCreated(item);
                this.context.Taxes.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Taxes.Where(i => i.TaxId == item.TaxId);

                Request.QueryString = Request.QueryString.Add("$expand", "Tax1");

                this.OnAfterTaxCreated(item);

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
