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
    [Route("odata/DOOHDB/Brands")]
    public partial class BrandsController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public BrandsController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.Brand> GetBrands()
        {
            var items = this.context.Brands.AsQueryable<DOOH.Server.Models.DOOHDB.Brand>();
            this.OnBrandsRead(ref items);

            return items;
        }

        partial void OnBrandsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Brand> items);

        partial void OnBrandGet(ref SingleResult<DOOH.Server.Models.DOOHDB.Brand> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/Brands(BrandId={BrandId})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.Brand> GetBrand(int key)
        {
            var items = this.context.Brands.Where(i => i.BrandId == key);
            var result = SingleResult.Create(items);

            OnBrandGet(ref result);

            return result;
        }
        partial void OnBrandDeleted(DOOH.Server.Models.DOOHDB.Brand item);
        partial void OnAfterBrandDeleted(DOOH.Server.Models.DOOHDB.Brand item);

        [HttpDelete("/odata/DOOHDB/Brands(BrandId={BrandId})")]
        public IActionResult DeleteBrand(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Brands
                    .Where(i => i.BrandId == key)
                    .Include(i => i.Displays)
                    .Include(i => i.Motherboards)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.Brand>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnBrandDeleted(item);
                this.context.Brands.Remove(item);
                this.context.SaveChanges();
                this.OnAfterBrandDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnBrandUpdated(DOOH.Server.Models.DOOHDB.Brand item);
        partial void OnAfterBrandUpdated(DOOH.Server.Models.DOOHDB.Brand item);

        [HttpPut("/odata/DOOHDB/Brands(BrandId={BrandId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutBrand(int key, [FromBody]DOOH.Server.Models.DOOHDB.Brand item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Brands
                    .Where(i => i.BrandId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.Brand>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnBrandUpdated(item);
                this.context.Brands.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Brands.Where(i => i.BrandId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Attachment");
                this.OnAfterBrandUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/Brands(BrandId={BrandId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchBrand(int key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.Brand> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Brands
                    .Where(i => i.BrandId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.Brand>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnBrandUpdated(item);
                this.context.Brands.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Brands.Where(i => i.BrandId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Attachment");
                this.OnAfterBrandUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnBrandCreated(DOOH.Server.Models.DOOHDB.Brand item);
        partial void OnAfterBrandCreated(DOOH.Server.Models.DOOHDB.Brand item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.Brand item)
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

                this.OnBrandCreated(item);
                this.context.Brands.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Brands.Where(i => i.BrandId == item.BrandId);

                Request.QueryString = Request.QueryString.Add("$expand", "Attachment");

                this.OnAfterBrandCreated(item);

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
