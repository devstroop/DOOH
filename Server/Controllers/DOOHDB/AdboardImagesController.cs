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
    [Route("odata/DOOHDB/AdboardImages")]
    public partial class AdboardImagesController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public AdboardImagesController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.AdboardImage> GetAdboardImages()
        {
            var items = this.context.AdboardImages.AsQueryable<DOOH.Server.Models.DOOHDB.AdboardImage>();
            this.OnAdboardImagesRead(ref items);

            return items;
        }

        partial void OnAdboardImagesRead(ref IQueryable<DOOH.Server.Models.DOOHDB.AdboardImage> items);

        partial void OnAdboardImageGet(ref SingleResult<DOOH.Server.Models.DOOHDB.AdboardImage> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/AdboardImages(AdboardImageId={AdboardImageId})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.AdboardImage> GetAdboardImage(int key)
        {
            var items = this.context.AdboardImages.Where(i => i.AdboardImageId == key);
            var result = SingleResult.Create(items);

            OnAdboardImageGet(ref result);

            return result;
        }
        partial void OnAdboardImageDeleted(DOOH.Server.Models.DOOHDB.AdboardImage item);
        partial void OnAfterAdboardImageDeleted(DOOH.Server.Models.DOOHDB.AdboardImage item);

        [HttpDelete("/odata/DOOHDB/AdboardImages(AdboardImageId={AdboardImageId})")]
        public IActionResult DeleteAdboardImage(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.AdboardImages
                    .Where(i => i.AdboardImageId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.AdboardImage>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAdboardImageDeleted(item);
                this.context.AdboardImages.Remove(item);
                this.context.SaveChanges();
                this.OnAfterAdboardImageDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAdboardImageUpdated(DOOH.Server.Models.DOOHDB.AdboardImage item);
        partial void OnAfterAdboardImageUpdated(DOOH.Server.Models.DOOHDB.AdboardImage item);

        [HttpPut("/odata/DOOHDB/AdboardImages(AdboardImageId={AdboardImageId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAdboardImage(int key, [FromBody]DOOH.Server.Models.DOOHDB.AdboardImage item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AdboardImages
                    .Where(i => i.AdboardImageId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.AdboardImage>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAdboardImageUpdated(item);
                this.context.AdboardImages.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AdboardImages.Where(i => i.AdboardImageId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Adboard");
                this.OnAfterAdboardImageUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/AdboardImages(AdboardImageId={AdboardImageId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchAdboardImage(int key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.AdboardImage> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AdboardImages
                    .Where(i => i.AdboardImageId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.AdboardImage>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnAdboardImageUpdated(item);
                this.context.AdboardImages.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AdboardImages.Where(i => i.AdboardImageId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Adboard");
                this.OnAfterAdboardImageUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAdboardImageCreated(DOOH.Server.Models.DOOHDB.AdboardImage item);
        partial void OnAfterAdboardImageCreated(DOOH.Server.Models.DOOHDB.AdboardImage item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.AdboardImage item)
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

                this.OnAdboardImageCreated(item);
                this.context.AdboardImages.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AdboardImages.Where(i => i.AdboardImageId == item.AdboardImageId);

                Request.QueryString = Request.QueryString.Add("$expand", "Adboard");

                this.OnAfterAdboardImageCreated(item);

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
