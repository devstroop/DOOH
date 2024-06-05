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
    [Route("odata/DOOHDB/Policies")]
    public partial class PoliciesController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public PoliciesController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.Policy> GetPolicies()
        {
            var items = this.context.Policies.AsQueryable<DOOH.Server.Models.DOOHDB.Policy>();
            this.OnPoliciesRead(ref items);

            return items;
        }

        partial void OnPoliciesRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Policy> items);

        partial void OnPolicyGet(ref SingleResult<DOOH.Server.Models.DOOHDB.Policy> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/Policies(Id={Id})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.Policy> GetPolicy(string key)
        {
            var items = this.context.Policies.Where(i => i.Id == Uri.UnescapeDataString(key));
            var result = SingleResult.Create(items);

            OnPolicyGet(ref result);

            return result;
        }
        partial void OnPolicyDeleted(DOOH.Server.Models.DOOHDB.Policy item);
        partial void OnAfterPolicyDeleted(DOOH.Server.Models.DOOHDB.Policy item);

        [HttpDelete("/odata/DOOHDB/Policies(Id={Id})")]
        public IActionResult DeletePolicy(string key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Policies
                    .Where(i => i.Id == Uri.UnescapeDataString(key))
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnPolicyDeleted(item);
                this.context.Policies.Remove(item);
                this.context.SaveChanges();
                this.OnAfterPolicyDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnPolicyUpdated(DOOH.Server.Models.DOOHDB.Policy item);
        partial void OnAfterPolicyUpdated(DOOH.Server.Models.DOOHDB.Policy item);

        [HttpPut("/odata/DOOHDB/Policies(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutPolicy(string key, [FromBody]DOOH.Server.Models.DOOHDB.Policy item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.Id != Uri.UnescapeDataString(key)))
                {
                    return BadRequest();
                }
                this.OnPolicyUpdated(item);
                this.context.Policies.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Policies.Where(i => i.Id == Uri.UnescapeDataString(key));
                
                this.OnAfterPolicyUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/Policies(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchPolicy(string key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.Policy> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Policies.Where(i => i.Id == Uri.UnescapeDataString(key)).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnPolicyUpdated(item);
                this.context.Policies.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Policies.Where(i => i.Id == Uri.UnescapeDataString(key));
                
                this.OnAfterPolicyUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnPolicyCreated(DOOH.Server.Models.DOOHDB.Policy item);
        partial void OnAfterPolicyCreated(DOOH.Server.Models.DOOHDB.Policy item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.Policy item)
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

                this.OnPolicyCreated(item);
                this.context.Policies.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Policies.Where(i => i.Id == item.Id);

                

                this.OnAfterPolicyCreated(item);

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
