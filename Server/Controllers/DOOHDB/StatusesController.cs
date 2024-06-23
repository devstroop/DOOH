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
    [Route("odata/DOOHDB/Statuses")]
    public partial class StatusesController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public StatusesController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.Status> GetStatuses()
        {
            var items = this.context.Statuses.AsQueryable<DOOH.Server.Models.DOOHDB.Status>();
            this.OnStatusesRead(ref items);

            return items;
        }

        partial void OnStatusesRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Status> items);

        partial void OnStatusGet(ref SingleResult<DOOH.Server.Models.DOOHDB.Status> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/Statuses(StatusId={StatusId})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.Status> GetStatus(int key)
        {
            var items = this.context.Statuses.Where(i => i.StatusId == key);
            var result = SingleResult.Create(items);

            OnStatusGet(ref result);

            return result;
        }
        partial void OnStatusDeleted(DOOH.Server.Models.DOOHDB.Status item);
        partial void OnAfterStatusDeleted(DOOH.Server.Models.DOOHDB.Status item);

        [HttpDelete("/odata/DOOHDB/Statuses(StatusId={StatusId})")]
        public IActionResult DeleteStatus(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Statuses
                    .Where(i => i.StatusId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnStatusDeleted(item);
                this.context.Statuses.Remove(item);
                this.context.SaveChanges();
                this.OnAfterStatusDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStatusUpdated(DOOH.Server.Models.DOOHDB.Status item);
        partial void OnAfterStatusUpdated(DOOH.Server.Models.DOOHDB.Status item);

        [HttpPut("/odata/DOOHDB/Statuses(StatusId={StatusId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutStatus(int key, [FromBody]DOOH.Server.Models.DOOHDB.Status item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.StatusId != key))
                {
                    return BadRequest();
                }
                this.OnStatusUpdated(item);
                this.context.Statuses.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Statuses.Where(i => i.StatusId == key);
                
                this.OnAfterStatusUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/Statuses(StatusId={StatusId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchStatus(int key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.Status> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Statuses.Where(i => i.StatusId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnStatusUpdated(item);
                this.context.Statuses.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Statuses.Where(i => i.StatusId == key);
                
                this.OnAfterStatusUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStatusCreated(DOOH.Server.Models.DOOHDB.Status item);
        partial void OnAfterStatusCreated(DOOH.Server.Models.DOOHDB.Status item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.Status item)
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

                this.OnStatusCreated(item);
                this.context.Statuses.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Statuses.Where(i => i.StatusId == item.StatusId);

                

                this.OnAfterStatusCreated(item);

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
