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
    [Route("odata/DOOHDB/Providers")]
    public partial class ProvidersController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public ProvidersController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.Provider> GetProviders()
        {
            var items = this.context.Providers.AsQueryable<DOOH.Server.Models.DOOHDB.Provider>();
            this.OnProvidersRead(ref items);

            return items;
        }

        partial void OnProvidersRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Provider> items);

        partial void OnProviderGet(ref SingleResult<DOOH.Server.Models.DOOHDB.Provider> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/Providers(ProviderId={ProviderId})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.Provider> GetProvider(int key)
        {
            var items = this.context.Providers.Where(i => i.ProviderId == key);
            var result = SingleResult.Create(items);

            OnProviderGet(ref result);

            return result;
        }
        partial void OnProviderDeleted(DOOH.Server.Models.DOOHDB.Provider item);
        partial void OnAfterProviderDeleted(DOOH.Server.Models.DOOHDB.Provider item);

        [HttpDelete("/odata/DOOHDB/Providers(ProviderId={ProviderId})")]
        public IActionResult DeleteProvider(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Providers
                    .Where(i => i.ProviderId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnProviderDeleted(item);
                this.context.Providers.Remove(item);
                this.context.SaveChanges();
                this.OnAfterProviderDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnProviderUpdated(DOOH.Server.Models.DOOHDB.Provider item);
        partial void OnAfterProviderUpdated(DOOH.Server.Models.DOOHDB.Provider item);

        [HttpPut("/odata/DOOHDB/Providers(ProviderId={ProviderId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutProvider(int key, [FromBody]DOOH.Server.Models.DOOHDB.Provider item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.ProviderId != key))
                {
                    return BadRequest();
                }
                this.OnProviderUpdated(item);
                this.context.Providers.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Providers.Where(i => i.ProviderId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "AspNetUser");
                this.OnAfterProviderUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/Providers(ProviderId={ProviderId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchProvider(int key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.Provider> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Providers.Where(i => i.ProviderId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnProviderUpdated(item);
                this.context.Providers.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Providers.Where(i => i.ProviderId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "AspNetUser");
                this.OnAfterProviderUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnProviderCreated(DOOH.Server.Models.DOOHDB.Provider item);
        partial void OnAfterProviderCreated(DOOH.Server.Models.DOOHDB.Provider item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.Provider item)
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

                this.OnProviderCreated(item);
                this.context.Providers.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Providers.Where(i => i.ProviderId == item.ProviderId);

                Request.QueryString = Request.QueryString.Add("$expand", "AspNetUser");

                this.OnAfterProviderCreated(item);

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
