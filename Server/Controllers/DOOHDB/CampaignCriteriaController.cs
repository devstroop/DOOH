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
    [Route("odata/DOOHDB/CampaignCriteria")]
    public partial class CampaignCriteriaController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public CampaignCriteriaController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.CampaignCriterion> GetCampaignCriteria()
        {
            var items = this.context.CampaignCriteria.AsQueryable<DOOH.Server.Models.DOOHDB.CampaignCriterion>();
            this.OnCampaignCriteriaRead(ref items);

            return items;
        }

        partial void OnCampaignCriteriaRead(ref IQueryable<DOOH.Server.Models.DOOHDB.CampaignCriterion> items);

        partial void OnCampaignCriterionGet(ref SingleResult<DOOH.Server.Models.DOOHDB.CampaignCriterion> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/CampaignCriteria(CampaignCriteriaId={CampaignCriteriaId})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.CampaignCriterion> GetCampaignCriterion(int key)
        {
            var items = this.context.CampaignCriteria.Where(i => i.CampaignCriteriaId == key);
            var result = SingleResult.Create(items);

            OnCampaignCriterionGet(ref result);

            return result;
        }
        partial void OnCampaignCriterionDeleted(DOOH.Server.Models.DOOHDB.CampaignCriterion item);
        partial void OnAfterCampaignCriterionDeleted(DOOH.Server.Models.DOOHDB.CampaignCriterion item);

        [HttpDelete("/odata/DOOHDB/CampaignCriteria(CampaignCriteriaId={CampaignCriteriaId})")]
        public IActionResult DeleteCampaignCriterion(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.CampaignCriteria
                    .Where(i => i.CampaignCriteriaId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnCampaignCriterionDeleted(item);
                this.context.CampaignCriteria.Remove(item);
                this.context.SaveChanges();
                this.OnAfterCampaignCriterionDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnCampaignCriterionUpdated(DOOH.Server.Models.DOOHDB.CampaignCriterion item);
        partial void OnAfterCampaignCriterionUpdated(DOOH.Server.Models.DOOHDB.CampaignCriterion item);

        [HttpPut("/odata/DOOHDB/CampaignCriteria(CampaignCriteriaId={CampaignCriteriaId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutCampaignCriterion(int key, [FromBody]DOOH.Server.Models.DOOHDB.CampaignCriterion item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.CampaignCriteriaId != key))
                {
                    return BadRequest();
                }
                this.OnCampaignCriterionUpdated(item);
                this.context.CampaignCriteria.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.CampaignCriteria.Where(i => i.CampaignCriteriaId == key);
                
                this.OnAfterCampaignCriterionUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/CampaignCriteria(CampaignCriteriaId={CampaignCriteriaId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchCampaignCriterion(int key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.CampaignCriterion> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.CampaignCriteria.Where(i => i.CampaignCriteriaId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnCampaignCriterionUpdated(item);
                this.context.CampaignCriteria.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.CampaignCriteria.Where(i => i.CampaignCriteriaId == key);
                
                this.OnAfterCampaignCriterionUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnCampaignCriterionCreated(DOOH.Server.Models.DOOHDB.CampaignCriterion item);
        partial void OnAfterCampaignCriterionCreated(DOOH.Server.Models.DOOHDB.CampaignCriterion item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.CampaignCriterion item)
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

                this.OnCampaignCriterionCreated(item);
                this.context.CampaignCriteria.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.CampaignCriteria.Where(i => i.CampaignCriteriaId == item.CampaignCriteriaId);

                

                this.OnAfterCampaignCriterionCreated(item);

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
