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
    [Route("odata/DOOHDB/AdboardWifis")]
    public partial class AdboardWifisController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public AdboardWifisController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.AdboardWifi> GetAdboardWifis()
        {
            var items = this.context.AdboardWifis.AsQueryable<DOOH.Server.Models.DOOHDB.AdboardWifi>();
            this.OnAdboardWifisRead(ref items);

            return items;
        }

        partial void OnAdboardWifisRead(ref IQueryable<DOOH.Server.Models.DOOHDB.AdboardWifi> items);

        partial void OnAdboardWifiGet(ref SingleResult<DOOH.Server.Models.DOOHDB.AdboardWifi> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/AdboardWifis(AdboardId={AdboardId})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.AdboardWifi> GetAdboardWifi(int key)
        {
            var items = this.context.AdboardWifis.Where(i => i.AdboardId == key);
            var result = SingleResult.Create(items);

            OnAdboardWifiGet(ref result);

            return result;
        }
        partial void OnAdboardWifiDeleted(DOOH.Server.Models.DOOHDB.AdboardWifi item);
        partial void OnAfterAdboardWifiDeleted(DOOH.Server.Models.DOOHDB.AdboardWifi item);

        [HttpDelete("/odata/DOOHDB/AdboardWifis(AdboardId={AdboardId})")]
        public IActionResult DeleteAdboardWifi(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.AdboardWifis
                    .Where(i => i.AdboardId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnAdboardWifiDeleted(item);
                this.context.AdboardWifis.Remove(item);
                this.context.SaveChanges();
                this.OnAfterAdboardWifiDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAdboardWifiUpdated(DOOH.Server.Models.DOOHDB.AdboardWifi item);
        partial void OnAfterAdboardWifiUpdated(DOOH.Server.Models.DOOHDB.AdboardWifi item);

        [HttpPut("/odata/DOOHDB/AdboardWifis(AdboardId={AdboardId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAdboardWifi(int key, [FromBody]DOOH.Server.Models.DOOHDB.AdboardWifi item)
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
                this.OnAdboardWifiUpdated(item);
                this.context.AdboardWifis.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AdboardWifis.Where(i => i.AdboardId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Adboard");
                this.OnAfterAdboardWifiUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/AdboardWifis(AdboardId={AdboardId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchAdboardWifi(int key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.AdboardWifi> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.AdboardWifis.Where(i => i.AdboardId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnAdboardWifiUpdated(item);
                this.context.AdboardWifis.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AdboardWifis.Where(i => i.AdboardId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Adboard");
                this.OnAfterAdboardWifiUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAdboardWifiCreated(DOOH.Server.Models.DOOHDB.AdboardWifi item);
        partial void OnAfterAdboardWifiCreated(DOOH.Server.Models.DOOHDB.AdboardWifi item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.AdboardWifi item)
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

                this.OnAdboardWifiCreated(item);
                this.context.AdboardWifis.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AdboardWifis.Where(i => i.AdboardId == item.AdboardId);

                Request.QueryString = Request.QueryString.Add("$expand", "Adboard");

                this.OnAfterAdboardWifiCreated(item);

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
