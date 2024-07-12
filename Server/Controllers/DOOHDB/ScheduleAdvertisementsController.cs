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
    [Route("odata/DOOHDB/ScheduleAdvertisements")]
    public partial class ScheduleAdvertisementsController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public ScheduleAdvertisementsController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.ScheduleAdvertisement> GetScheduleAdvertisements()
        {
            var items = this.context.ScheduleAdvertisements.AsQueryable<DOOH.Server.Models.DOOHDB.ScheduleAdvertisement>();
            this.OnScheduleAdvertisementsRead(ref items);

            return items;
        }

        partial void OnScheduleAdvertisementsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.ScheduleAdvertisement> items);

        partial void OnScheduleAdvertisementGet(ref SingleResult<DOOH.Server.Models.DOOHDB.ScheduleAdvertisement> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/ScheduleAdvertisements(ScheduleId={keyScheduleId},AdvertisementId={keyAdvertisementId})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.ScheduleAdvertisement> GetScheduleAdvertisement([FromODataUri] int keyScheduleId, [FromODataUri] int keyAdvertisementId)
        {
            var items = this.context.ScheduleAdvertisements.Where(i => i.ScheduleId == keyScheduleId && i.AdvertisementId == keyAdvertisementId);
            var result = SingleResult.Create(items);

            OnScheduleAdvertisementGet(ref result);

            return result;
        }
        partial void OnScheduleAdvertisementDeleted(DOOH.Server.Models.DOOHDB.ScheduleAdvertisement item);
        partial void OnAfterScheduleAdvertisementDeleted(DOOH.Server.Models.DOOHDB.ScheduleAdvertisement item);

        [HttpDelete("/odata/DOOHDB/ScheduleAdvertisements(ScheduleId={keyScheduleId},AdvertisementId={keyAdvertisementId})")]
        public IActionResult DeleteScheduleAdvertisement([FromODataUri] int keyScheduleId, [FromODataUri] int keyAdvertisementId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.ScheduleAdvertisements
                    .Where(i => i.ScheduleId == keyScheduleId && i.AdvertisementId == keyAdvertisementId)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnScheduleAdvertisementDeleted(item);
                this.context.ScheduleAdvertisements.Remove(item);
                this.context.SaveChanges();
                this.OnAfterScheduleAdvertisementDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnScheduleAdvertisementUpdated(DOOH.Server.Models.DOOHDB.ScheduleAdvertisement item);
        partial void OnAfterScheduleAdvertisementUpdated(DOOH.Server.Models.DOOHDB.ScheduleAdvertisement item);

        [HttpPut("/odata/DOOHDB/ScheduleAdvertisements(ScheduleId={keyScheduleId},AdvertisementId={keyAdvertisementId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutScheduleAdvertisement([FromODataUri] int keyScheduleId, [FromODataUri] int keyAdvertisementId, [FromBody]DOOH.Server.Models.DOOHDB.ScheduleAdvertisement item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.ScheduleId != keyScheduleId && item.AdvertisementId != keyAdvertisementId))
                {
                    return BadRequest();
                }
                this.OnScheduleAdvertisementUpdated(item);
                this.context.ScheduleAdvertisements.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ScheduleAdvertisements.Where(i => i.ScheduleId == keyScheduleId && i.AdvertisementId == keyAdvertisementId);
                Request.QueryString = Request.QueryString.Add("$expand", "Advertisement,Schedule");
                this.OnAfterScheduleAdvertisementUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/ScheduleAdvertisements(ScheduleId={keyScheduleId},AdvertisementId={keyAdvertisementId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchScheduleAdvertisement([FromODataUri] int keyScheduleId, [FromODataUri] int keyAdvertisementId, [FromBody]Delta<DOOH.Server.Models.DOOHDB.ScheduleAdvertisement> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.ScheduleAdvertisements.Where(i => i.ScheduleId == keyScheduleId && i.AdvertisementId == keyAdvertisementId).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnScheduleAdvertisementUpdated(item);
                this.context.ScheduleAdvertisements.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ScheduleAdvertisements.Where(i => i.ScheduleId == keyScheduleId && i.AdvertisementId == keyAdvertisementId);
                Request.QueryString = Request.QueryString.Add("$expand", "Advertisement,Schedule");
                this.OnAfterScheduleAdvertisementUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnScheduleAdvertisementCreated(DOOH.Server.Models.DOOHDB.ScheduleAdvertisement item);
        partial void OnAfterScheduleAdvertisementCreated(DOOH.Server.Models.DOOHDB.ScheduleAdvertisement item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.ScheduleAdvertisement item)
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

                this.OnScheduleAdvertisementCreated(item);
                this.context.ScheduleAdvertisements.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ScheduleAdvertisements.Where(i => i.ScheduleId == item.ScheduleId && i.AdvertisementId == item.AdvertisementId);

                Request.QueryString = Request.QueryString.Add("$expand", "Advertisement,Schedule");

                this.OnAfterScheduleAdvertisementCreated(item);

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
