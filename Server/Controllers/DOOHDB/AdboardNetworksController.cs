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
    [Route("odata/DOOHDB/AdboardNetworks")]
    public partial class AdboardNetworksController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public AdboardNetworksController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.AdboardNetwork> GetAdboardNetworks()
        {
            var items = this.context.AdboardNetworks.AsQueryable<DOOH.Server.Models.DOOHDB.AdboardNetwork>();
            this.OnAdboardNetworksRead(ref items);

            return items;
        }

        partial void OnAdboardNetworksRead(ref IQueryable<DOOH.Server.Models.DOOHDB.AdboardNetwork> items);

        partial void OnAdboardNetworkGet(ref SingleResult<DOOH.Server.Models.DOOHDB.AdboardNetwork> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/AdboardNetworks(AdboardId={AdboardId})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.AdboardNetwork> GetAdboardNetwork(int key)
        {
            var items = this.context.AdboardNetworks.Where(i => i.AdboardId == key);
            var result = SingleResult.Create(items);

            OnAdboardNetworkGet(ref result);

            return result;
        }
        partial void OnAdboardNetworkDeleted(DOOH.Server.Models.DOOHDB.AdboardNetwork item);
        partial void OnAfterAdboardNetworkDeleted(DOOH.Server.Models.DOOHDB.AdboardNetwork item);

        [HttpDelete("/odata/DOOHDB/AdboardNetworks(AdboardId={AdboardId})")]
        public IActionResult DeleteAdboardNetwork(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.AdboardNetworks
                    .Where(i => i.AdboardId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnAdboardNetworkDeleted(item);
                this.context.AdboardNetworks.Remove(item);
                this.context.SaveChanges();
                this.OnAfterAdboardNetworkDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAdboardNetworkUpdated(DOOH.Server.Models.DOOHDB.AdboardNetwork item);
        partial void OnAfterAdboardNetworkUpdated(DOOH.Server.Models.DOOHDB.AdboardNetwork item);

        [HttpPut("/odata/DOOHDB/AdboardNetworks(AdboardId={AdboardId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAdboardNetwork(int key, [FromBody]DOOH.Server.Models.DOOHDB.AdboardNetwork item)
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
                this.OnAdboardNetworkUpdated(item);
                this.context.AdboardNetworks.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AdboardNetworks.Where(i => i.AdboardId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Adboard");
                this.OnAfterAdboardNetworkUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/AdboardNetworks(AdboardId={AdboardId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchAdboardNetwork(int key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.AdboardNetwork> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.AdboardNetworks.Where(i => i.AdboardId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnAdboardNetworkUpdated(item);
                this.context.AdboardNetworks.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AdboardNetworks.Where(i => i.AdboardId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Adboard");
                this.OnAfterAdboardNetworkUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAdboardNetworkCreated(DOOH.Server.Models.DOOHDB.AdboardNetwork item);
        partial void OnAfterAdboardNetworkCreated(DOOH.Server.Models.DOOHDB.AdboardNetwork item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.AdboardNetwork item)
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

                this.OnAdboardNetworkCreated(item);
                this.context.AdboardNetworks.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AdboardNetworks.Where(i => i.AdboardId == item.AdboardId);

                Request.QueryString = Request.QueryString.Add("$expand", "Adboard");

                this.OnAfterAdboardNetworkCreated(item);

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
