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
    [Route("odata/DOOHDB/Motherboards")]
    public partial class MotherboardsController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public MotherboardsController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.Motherboard> GetMotherboards()
        {
            var items = this.context.Motherboards.AsQueryable<DOOH.Server.Models.DOOHDB.Motherboard>();
            this.OnMotherboardsRead(ref items);

            return items;
        }

        partial void OnMotherboardsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Motherboard> items);

        partial void OnMotherboardGet(ref SingleResult<DOOH.Server.Models.DOOHDB.Motherboard> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/Motherboards(MotherboardId={MotherboardId})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.Motherboard> GetMotherboard(int key)
        {
            var items = this.context.Motherboards.Where(i => i.MotherboardId == key);
            var result = SingleResult.Create(items);

            OnMotherboardGet(ref result);

            return result;
        }
        partial void OnMotherboardDeleted(DOOH.Server.Models.DOOHDB.Motherboard item);
        partial void OnAfterMotherboardDeleted(DOOH.Server.Models.DOOHDB.Motherboard item);

        [HttpDelete("/odata/DOOHDB/Motherboards(MotherboardId={MotherboardId})")]
        public IActionResult DeleteMotherboard(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Motherboards
                    .Where(i => i.MotherboardId == key)
                    .Include(i => i.Adboards)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.Motherboard>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnMotherboardDeleted(item);
                this.context.Motherboards.Remove(item);
                this.context.SaveChanges();
                this.OnAfterMotherboardDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnMotherboardUpdated(DOOH.Server.Models.DOOHDB.Motherboard item);
        partial void OnAfterMotherboardUpdated(DOOH.Server.Models.DOOHDB.Motherboard item);

        [HttpPut("/odata/DOOHDB/Motherboards(MotherboardId={MotherboardId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutMotherboard(int key, [FromBody]DOOH.Server.Models.DOOHDB.Motherboard item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Motherboards
                    .Where(i => i.MotherboardId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.Motherboard>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnMotherboardUpdated(item);
                this.context.Motherboards.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Motherboards.Where(i => i.MotherboardId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Brand");
                this.OnAfterMotherboardUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/Motherboards(MotherboardId={MotherboardId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchMotherboard(int key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.Motherboard> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Motherboards
                    .Where(i => i.MotherboardId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.Motherboard>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnMotherboardUpdated(item);
                this.context.Motherboards.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Motherboards.Where(i => i.MotherboardId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Brand");
                this.OnAfterMotherboardUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnMotherboardCreated(DOOH.Server.Models.DOOHDB.Motherboard item);
        partial void OnAfterMotherboardCreated(DOOH.Server.Models.DOOHDB.Motherboard item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.Motherboard item)
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

                this.OnMotherboardCreated(item);
                this.context.Motherboards.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Motherboards.Where(i => i.MotherboardId == item.MotherboardId);

                Request.QueryString = Request.QueryString.Add("$expand", "Brand");

                this.OnAfterMotherboardCreated(item);

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
