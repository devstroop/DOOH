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
    [Route("odata/DOOHDB/AdboardTokens")]
    public partial class AdboardTokensController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public AdboardTokensController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.AdboardToken> GetAdboardTokens()
        {
            var items = this.context.AdboardTokens.AsQueryable<DOOH.Server.Models.DOOHDB.AdboardToken>();
            this.OnAdboardTokensRead(ref items);

            return items;
        }

        partial void OnAdboardTokensRead(ref IQueryable<DOOH.Server.Models.DOOHDB.AdboardToken> items);

        partial void OnAdboardTokenGet(ref SingleResult<DOOH.Server.Models.DOOHDB.AdboardToken> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/AdboardTokens(AdboardId={AdboardId})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.AdboardToken> GetAdboardToken(int key)
        {
            var items = this.context.AdboardTokens.Where(i => i.AdboardId == key);
            var result = SingleResult.Create(items);

            OnAdboardTokenGet(ref result);

            return result;
        }
        partial void OnAdboardTokenDeleted(DOOH.Server.Models.DOOHDB.AdboardToken item);
        partial void OnAfterAdboardTokenDeleted(DOOH.Server.Models.DOOHDB.AdboardToken item);

        [HttpDelete("/odata/DOOHDB/AdboardTokens(AdboardId={AdboardId})")]
        public IActionResult DeleteAdboardToken(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.AdboardTokens
                    .Where(i => i.AdboardId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.AdboardToken>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAdboardTokenDeleted(item);
                this.context.AdboardTokens.Remove(item);
                this.context.SaveChanges();
                this.OnAfterAdboardTokenDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAdboardTokenUpdated(DOOH.Server.Models.DOOHDB.AdboardToken item);
        partial void OnAfterAdboardTokenUpdated(DOOH.Server.Models.DOOHDB.AdboardToken item);

        [HttpPut("/odata/DOOHDB/AdboardTokens(AdboardId={AdboardId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAdboardToken(int key, [FromBody]DOOH.Server.Models.DOOHDB.AdboardToken item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AdboardTokens
                    .Where(i => i.AdboardId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.AdboardToken>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAdboardTokenUpdated(item);
                this.context.AdboardTokens.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AdboardTokens.Where(i => i.AdboardId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Adboard");
                this.OnAfterAdboardTokenUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/AdboardTokens(AdboardId={AdboardId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchAdboardToken(int key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.AdboardToken> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AdboardTokens
                    .Where(i => i.AdboardId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.AdboardToken>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnAdboardTokenUpdated(item);
                this.context.AdboardTokens.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AdboardTokens.Where(i => i.AdboardId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Adboard");
                this.OnAfterAdboardTokenUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAdboardTokenCreated(DOOH.Server.Models.DOOHDB.AdboardToken item);
        partial void OnAfterAdboardTokenCreated(DOOH.Server.Models.DOOHDB.AdboardToken item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.AdboardToken item)
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

                this.OnAdboardTokenCreated(item);
                this.context.AdboardTokens.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AdboardTokens.Where(i => i.AdboardId == item.AdboardId);

                Request.QueryString = Request.QueryString.Add("$expand", "Adboard");

                this.OnAfterAdboardTokenCreated(item);

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
