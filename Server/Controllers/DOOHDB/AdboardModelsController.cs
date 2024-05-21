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
    [Route("odata/DOOHDB/AdboardModels")]
    public partial class AdboardModelsController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public AdboardModelsController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.AdboardModel> GetAdboardModels()
        {
            var items = this.context.AdboardModels.AsQueryable<DOOH.Server.Models.DOOHDB.AdboardModel>();
            this.OnAdboardModelsRead(ref items);

            return items;
        }

        partial void OnAdboardModelsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.AdboardModel> items);

        partial void OnAdboardModelGet(ref SingleResult<DOOH.Server.Models.DOOHDB.AdboardModel> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/AdboardModels(AdboardModelId={AdboardModelId})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.AdboardModel> GetAdboardModel(int key)
        {
            var items = this.context.AdboardModels.Where(i => i.AdboardModelId == key);
            var result = SingleResult.Create(items);

            OnAdboardModelGet(ref result);

            return result;
        }
        partial void OnAdboardModelDeleted(DOOH.Server.Models.DOOHDB.AdboardModel item);
        partial void OnAfterAdboardModelDeleted(DOOH.Server.Models.DOOHDB.AdboardModel item);

        [HttpDelete("/odata/DOOHDB/AdboardModels(AdboardModelId={AdboardModelId})")]
        public IActionResult DeleteAdboardModel(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.AdboardModels
                    .Where(i => i.AdboardModelId == key)
                    .Include(i => i.Adboards)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.AdboardModel>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAdboardModelDeleted(item);
                this.context.AdboardModels.Remove(item);
                this.context.SaveChanges();
                this.OnAfterAdboardModelDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAdboardModelUpdated(DOOH.Server.Models.DOOHDB.AdboardModel item);
        partial void OnAfterAdboardModelUpdated(DOOH.Server.Models.DOOHDB.AdboardModel item);

        [HttpPut("/odata/DOOHDB/AdboardModels(AdboardModelId={AdboardModelId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAdboardModel(int key, [FromBody]DOOH.Server.Models.DOOHDB.AdboardModel item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AdboardModels
                    .Where(i => i.AdboardModelId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.AdboardModel>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAdboardModelUpdated(item);
                this.context.AdboardModels.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AdboardModels.Where(i => i.AdboardModelId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Attachment,Display,Motherboard");
                this.OnAfterAdboardModelUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/AdboardModels(AdboardModelId={AdboardModelId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchAdboardModel(int key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.AdboardModel> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AdboardModels
                    .Where(i => i.AdboardModelId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.AdboardModel>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnAdboardModelUpdated(item);
                this.context.AdboardModels.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AdboardModels.Where(i => i.AdboardModelId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Attachment,Display,Motherboard");
                this.OnAfterAdboardModelUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAdboardModelCreated(DOOH.Server.Models.DOOHDB.AdboardModel item);
        partial void OnAfterAdboardModelCreated(DOOH.Server.Models.DOOHDB.AdboardModel item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.AdboardModel item)
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

                this.OnAdboardModelCreated(item);
                this.context.AdboardModels.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AdboardModels.Where(i => i.AdboardModelId == item.AdboardModelId);

                Request.QueryString = Request.QueryString.Add("$expand", "Attachment,Display,Motherboard");

                this.OnAfterAdboardModelCreated(item);

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
