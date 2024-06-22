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
    [Route("odata/DOOHDB/AdboardStatuses")]
    public partial class AdboardStatusesController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public AdboardStatusesController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.AdboardStatus> GetAdboardStatuses()
        {
            var items = this.context.AdboardStatuses.AsQueryable<DOOH.Server.Models.DOOHDB.AdboardStatus>();
            this.OnAdboardStatusesRead(ref items);

            return items;
        }

        partial void OnAdboardStatusesRead(ref IQueryable<DOOH.Server.Models.DOOHDB.AdboardStatus> items);

        partial void OnAdboardStatusGet(ref SingleResult<DOOH.Server.Models.DOOHDB.AdboardStatus> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/AdboardStatuses(AdboardId={AdboardId})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.AdboardStatus> GetAdboardStatus(int key)
        {
            var items = this.context.AdboardStatuses.Where(i => i.AdboardId == key);
            var result = SingleResult.Create(items);

            OnAdboardStatusGet(ref result);

            return result;
        }
        partial void OnAdboardStatusDeleted(DOOH.Server.Models.DOOHDB.AdboardStatus item);
        partial void OnAfterAdboardStatusDeleted(DOOH.Server.Models.DOOHDB.AdboardStatus item);

        [HttpDelete("/odata/DOOHDB/AdboardStatuses(AdboardId={AdboardId})")]
        public IActionResult DeleteAdboardStatus(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.AdboardStatuses
                    .Where(i => i.AdboardId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnAdboardStatusDeleted(item);
                this.context.AdboardStatuses.Remove(item);
                this.context.SaveChanges();
                this.OnAfterAdboardStatusDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAdboardStatusUpdated(DOOH.Server.Models.DOOHDB.AdboardStatus item);
        partial void OnAfterAdboardStatusUpdated(DOOH.Server.Models.DOOHDB.AdboardStatus item);

        [HttpPut("/odata/DOOHDB/AdboardStatuses(AdboardId={AdboardId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAdboardStatus(int key, [FromBody]DOOH.Server.Models.DOOHDB.AdboardStatus item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.AdboardId != key))
                {
                    return BadRequest();
                }
                this.OnAdboardStatusUpdated(item);
                this.context.AdboardStatuses.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AdboardStatuses.Where(i => i.AdboardId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Adboard");
                this.OnAfterAdboardStatusUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/AdboardStatuses(AdboardId={AdboardId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchAdboardStatus(int key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.AdboardStatus> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.AdboardStatuses.Where(i => i.AdboardId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnAdboardStatusUpdated(item);
                this.context.AdboardStatuses.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AdboardStatuses.Where(i => i.AdboardId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Adboard");
                this.OnAfterAdboardStatusUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAdboardStatusCreated(DOOH.Server.Models.DOOHDB.AdboardStatus item);
        partial void OnAfterAdboardStatusCreated(DOOH.Server.Models.DOOHDB.AdboardStatus item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.AdboardStatus item)
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

                this.OnAdboardStatusCreated(item);
                this.context.AdboardStatuses.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AdboardStatuses.Where(i => i.AdboardId == item.AdboardId);

                Request.QueryString = Request.QueryString.Add("$expand", "Adboard");

                this.OnAfterAdboardStatusCreated(item);

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
