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
    [Route("odata/DOOHDB/Displays")]
    public partial class DisplaysController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public DisplaysController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.Display> GetDisplays()
        {
            var items = this.context.Displays.AsQueryable<DOOH.Server.Models.DOOHDB.Display>();
            this.OnDisplaysRead(ref items);

            return items;
        }

        partial void OnDisplaysRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Display> items);

        partial void OnDisplayGet(ref SingleResult<DOOH.Server.Models.DOOHDB.Display> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/Displays(DisplayId={DisplayId})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.Display> GetDisplay(int key)
        {
            var items = this.context.Displays.Where(i => i.DisplayId == key);
            var result = SingleResult.Create(items);

            OnDisplayGet(ref result);

            return result;
        }
        partial void OnDisplayDeleted(DOOH.Server.Models.DOOHDB.Display item);
        partial void OnAfterDisplayDeleted(DOOH.Server.Models.DOOHDB.Display item);

        [HttpDelete("/odata/DOOHDB/Displays(DisplayId={DisplayId})")]
        public IActionResult DeleteDisplay(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Displays
                    .Where(i => i.DisplayId == key)
                    .Include(i => i.AdboardModels)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.Display>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnDisplayDeleted(item);
                this.context.Displays.Remove(item);
                this.context.SaveChanges();
                this.OnAfterDisplayDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnDisplayUpdated(DOOH.Server.Models.DOOHDB.Display item);
        partial void OnAfterDisplayUpdated(DOOH.Server.Models.DOOHDB.Display item);

        [HttpPut("/odata/DOOHDB/Displays(DisplayId={DisplayId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutDisplay(int key, [FromBody]DOOH.Server.Models.DOOHDB.Display item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Displays
                    .Where(i => i.DisplayId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.Display>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnDisplayUpdated(item);
                this.context.Displays.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Displays.Where(i => i.DisplayId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Brand");
                this.OnAfterDisplayUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/Displays(DisplayId={DisplayId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchDisplay(int key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.Display> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Displays
                    .Where(i => i.DisplayId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.Display>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnDisplayUpdated(item);
                this.context.Displays.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Displays.Where(i => i.DisplayId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Brand");
                this.OnAfterDisplayUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnDisplayCreated(DOOH.Server.Models.DOOHDB.Display item);
        partial void OnAfterDisplayCreated(DOOH.Server.Models.DOOHDB.Display item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.Display item)
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

                this.OnDisplayCreated(item);
                this.context.Displays.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Displays.Where(i => i.DisplayId == item.DisplayId);

                Request.QueryString = Request.QueryString.Add("$expand", "Brand");

                this.OnAfterDisplayCreated(item);

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
