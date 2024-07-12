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
    [Route("odata/DOOHDB/Advertisements")]
    public partial class AdvertisementsController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public AdvertisementsController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.Advertisement> GetAdvertisements()
        {
            var items = this.context.Advertisements.AsQueryable<DOOH.Server.Models.DOOHDB.Advertisement>();
            this.OnAdvertisementsRead(ref items);

            return items;
        }

        partial void OnAdvertisementsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Advertisement> items);

        partial void OnAdvertisementGet(ref SingleResult<DOOH.Server.Models.DOOHDB.Advertisement> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/Advertisements(AdvertisementId={AdvertisementId})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.Advertisement> GetAdvertisement(int key)
        {
            var items = this.context.Advertisements.Where(i => i.AdvertisementId == key);
            var result = SingleResult.Create(items);

            OnAdvertisementGet(ref result);

            return result;
        }
        partial void OnAdvertisementDeleted(DOOH.Server.Models.DOOHDB.Advertisement item);
        partial void OnAfterAdvertisementDeleted(DOOH.Server.Models.DOOHDB.Advertisement item);

        [HttpDelete("/odata/DOOHDB/Advertisements(AdvertisementId={AdvertisementId})")]
        public IActionResult DeleteAdvertisement(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Advertisements
                    .Where(i => i.AdvertisementId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnAdvertisementDeleted(item);
                this.context.Advertisements.Remove(item);
                this.context.SaveChanges();
                this.OnAfterAdvertisementDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAdvertisementUpdated(DOOH.Server.Models.DOOHDB.Advertisement item);
        partial void OnAfterAdvertisementUpdated(DOOH.Server.Models.DOOHDB.Advertisement item);

        [HttpPut("/odata/DOOHDB/Advertisements(AdvertisementId={AdvertisementId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAdvertisement(int key, [FromBody]DOOH.Server.Models.DOOHDB.Advertisement item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.AdvertisementId != key))
                {
                    return BadRequest();
                }
                this.OnAdvertisementUpdated(item);
                this.context.Advertisements.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Advertisements.Where(i => i.AdvertisementId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Campaign");
                this.OnAfterAdvertisementUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/Advertisements(AdvertisementId={AdvertisementId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchAdvertisement(int key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.Advertisement> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Advertisements.Where(i => i.AdvertisementId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnAdvertisementUpdated(item);
                this.context.Advertisements.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Advertisements.Where(i => i.AdvertisementId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Campaign");
                this.OnAfterAdvertisementUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAdvertisementCreated(DOOH.Server.Models.DOOHDB.Advertisement item);
        partial void OnAfterAdvertisementCreated(DOOH.Server.Models.DOOHDB.Advertisement item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.Advertisement item)
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

                this.OnAdvertisementCreated(item);
                this.context.Advertisements.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Advertisements.Where(i => i.AdvertisementId == item.AdvertisementId);

                Request.QueryString = Request.QueryString.Add("$expand", "Campaign");

                this.OnAfterAdvertisementCreated(item);

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
