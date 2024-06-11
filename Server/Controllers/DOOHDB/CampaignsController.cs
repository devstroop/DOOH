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
    [Route("odata/DOOHDB/Campaigns")]
    public partial class CampaignsController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public CampaignsController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.Campaign> GetCampaigns()
        {
            var items = this.context.Campaigns.AsQueryable<DOOH.Server.Models.DOOHDB.Campaign>();
            this.OnCampaignsRead(ref items);

            return items;
        }

        partial void OnCampaignsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Campaign> items);

        partial void OnCampaignGet(ref SingleResult<DOOH.Server.Models.DOOHDB.Campaign> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/Campaigns(CampaignId={CampaignId})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.Campaign> GetCampaign(int key)
        {
            var items = this.context.Campaigns.Where(i => i.CampaignId == key);
            var result = SingleResult.Create(items);

            OnCampaignGet(ref result);

            return result;
        }
        partial void OnCampaignDeleted(DOOH.Server.Models.DOOHDB.Campaign item);
        partial void OnAfterCampaignDeleted(DOOH.Server.Models.DOOHDB.Campaign item);

        [HttpDelete("/odata/DOOHDB/Campaigns(CampaignId={CampaignId})")]
        public IActionResult DeleteCampaign(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Campaigns
                    .Where(i => i.CampaignId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnCampaignDeleted(item);
                this.context.Campaigns.Remove(item);
                this.context.SaveChanges();
                this.OnAfterCampaignDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnCampaignUpdated(DOOH.Server.Models.DOOHDB.Campaign item);
        partial void OnAfterCampaignUpdated(DOOH.Server.Models.DOOHDB.Campaign item);

        [HttpPut("/odata/DOOHDB/Campaigns(CampaignId={CampaignId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutCampaign(int key, [FromBody]DOOH.Server.Models.DOOHDB.Campaign item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.CampaignId != key))
                {
                    return BadRequest();
                }
                this.OnCampaignUpdated(item);
                this.context.Campaigns.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Campaigns.Where(i => i.CampaignId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Status,AspNetUser");
                this.OnAfterCampaignUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/Campaigns(CampaignId={CampaignId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchCampaign(int key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.Campaign> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Campaigns.Where(i => i.CampaignId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnCampaignUpdated(item);
                this.context.Campaigns.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Campaigns.Where(i => i.CampaignId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Status,AspNetUser");
                this.OnAfterCampaignUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnCampaignCreated(DOOH.Server.Models.DOOHDB.Campaign item);
        partial void OnAfterCampaignCreated(DOOH.Server.Models.DOOHDB.Campaign item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.Campaign item)
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

                this.OnCampaignCreated(item);
                this.context.Campaigns.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Campaigns.Where(i => i.CampaignId == item.CampaignId);

                Request.QueryString = Request.QueryString.Add("$expand", "Status,AspNetUser");

                this.OnAfterCampaignCreated(item);

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
