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
    [Route("odata/DOOHDB/Pages")]
    public partial class PagesController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public PagesController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.Page> GetPages()
        {
            var items = this.context.Pages.AsQueryable<DOOH.Server.Models.DOOHDB.Page>();
            this.OnPagesRead(ref items);

            return items;
        }

        partial void OnPagesRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Page> items);

        partial void OnPageGet(ref SingleResult<DOOH.Server.Models.DOOHDB.Page> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/Pages(Slag={Slag})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.Page> GetPage(string key)
        {
            var items = this.context.Pages.Where(i => i.Slag == Uri.UnescapeDataString(key));
            var result = SingleResult.Create(items);

            OnPageGet(ref result);

            return result;
        }
        partial void OnPageDeleted(DOOH.Server.Models.DOOHDB.Page item);
        partial void OnAfterPageDeleted(DOOH.Server.Models.DOOHDB.Page item);

        [HttpDelete("/odata/DOOHDB/Pages(Slag={Slag})")]
        public IActionResult DeletePage(string key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Pages
                    .Where(i => i.Slag == Uri.UnescapeDataString(key))
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnPageDeleted(item);
                this.context.Pages.Remove(item);
                this.context.SaveChanges();
                this.OnAfterPageDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnPageUpdated(DOOH.Server.Models.DOOHDB.Page item);
        partial void OnAfterPageUpdated(DOOH.Server.Models.DOOHDB.Page item);

        [HttpPut("/odata/DOOHDB/Pages(Slag={Slag})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutPage(string key, [FromBody]DOOH.Server.Models.DOOHDB.Page item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.Slag != Uri.UnescapeDataString(key)))
                {
                    return BadRequest();
                }
                this.OnPageUpdated(item);
                this.context.Pages.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Pages.Where(i => i.Slag == Uri.UnescapeDataString(key));
                
                this.OnAfterPageUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/Pages(Slag={Slag})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchPage(string key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.Page> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Pages.Where(i => i.Slag == Uri.UnescapeDataString(key)).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnPageUpdated(item);
                this.context.Pages.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Pages.Where(i => i.Slag == Uri.UnescapeDataString(key));
                
                this.OnAfterPageUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnPageCreated(DOOH.Server.Models.DOOHDB.Page item);
        partial void OnAfterPageCreated(DOOH.Server.Models.DOOHDB.Page item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.Page item)
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

                this.OnPageCreated(item);
                this.context.Pages.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Pages.Where(i => i.Slag == item.Slag);

                

                this.OnAfterPageCreated(item);

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
