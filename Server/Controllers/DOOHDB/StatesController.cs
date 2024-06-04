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
    [Route("odata/DOOHDB/States")]
    public partial class StatesController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public StatesController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.State> GetStates()
        {
            var items = this.context.States.AsQueryable<DOOH.Server.Models.DOOHDB.State>();
            this.OnStatesRead(ref items);

            return items;
        }

        partial void OnStatesRead(ref IQueryable<DOOH.Server.Models.DOOHDB.State> items);

        partial void OnStateGet(ref SingleResult<DOOH.Server.Models.DOOHDB.State> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/States(StateName={StateName})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.State> GetState(string key)
        {
            var items = this.context.States.Where(i => i.StateName == Uri.UnescapeDataString(key));
            var result = SingleResult.Create(items);

            OnStateGet(ref result);

            return result;
        }
        partial void OnStateDeleted(DOOH.Server.Models.DOOHDB.State item);
        partial void OnAfterStateDeleted(DOOH.Server.Models.DOOHDB.State item);

        [HttpDelete("/odata/DOOHDB/States(StateName={StateName})")]
        public IActionResult DeleteState(string key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.States
                    .Where(i => i.StateName == Uri.UnescapeDataString(key))
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnStateDeleted(item);
                this.context.States.Remove(item);
                this.context.SaveChanges();
                this.OnAfterStateDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStateUpdated(DOOH.Server.Models.DOOHDB.State item);
        partial void OnAfterStateUpdated(DOOH.Server.Models.DOOHDB.State item);

        [HttpPut("/odata/DOOHDB/States(StateName={StateName})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutState(string key, [FromBody]DOOH.Server.Models.DOOHDB.State item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.StateName != Uri.UnescapeDataString(key)))
                {
                    return BadRequest();
                }
                this.OnStateUpdated(item);
                this.context.States.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.States.Where(i => i.StateName == Uri.UnescapeDataString(key));
                Request.QueryString = Request.QueryString.Add("$expand", "Country");
                this.OnAfterStateUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/States(StateName={StateName})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchState(string key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.State> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.States.Where(i => i.StateName == Uri.UnescapeDataString(key)).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnStateUpdated(item);
                this.context.States.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.States.Where(i => i.StateName == Uri.UnescapeDataString(key));
                Request.QueryString = Request.QueryString.Add("$expand", "Country");
                this.OnAfterStateUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStateCreated(DOOH.Server.Models.DOOHDB.State item);
        partial void OnAfterStateCreated(DOOH.Server.Models.DOOHDB.State item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.State item)
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

                this.OnStateCreated(item);
                this.context.States.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.States.Where(i => i.StateName == item.StateName);

                Request.QueryString = Request.QueryString.Add("$expand", "Country");

                this.OnAfterStateCreated(item);

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
