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
    [Route("odata/DOOHDB/Faqs")]
    public partial class FaqsController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public FaqsController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.Faq> GetFaqs()
        {
            var items = this.context.Faqs.AsQueryable<DOOH.Server.Models.DOOHDB.Faq>();
            this.OnFaqsRead(ref items);

            return items;
        }

        partial void OnFaqsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Faq> items);

        partial void OnFaqGet(ref SingleResult<DOOH.Server.Models.DOOHDB.Faq> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/Faqs(FaqId={FaqId})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.Faq> GetFaq(int key)
        {
            var items = this.context.Faqs.Where(i => i.FaqId == key);
            var result = SingleResult.Create(items);

            OnFaqGet(ref result);

            return result;
        }
        partial void OnFaqDeleted(DOOH.Server.Models.DOOHDB.Faq item);
        partial void OnAfterFaqDeleted(DOOH.Server.Models.DOOHDB.Faq item);

        [HttpDelete("/odata/DOOHDB/Faqs(FaqId={FaqId})")]
        public IActionResult DeleteFaq(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Faqs
                    .Where(i => i.FaqId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnFaqDeleted(item);
                this.context.Faqs.Remove(item);
                this.context.SaveChanges();
                this.OnAfterFaqDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnFaqUpdated(DOOH.Server.Models.DOOHDB.Faq item);
        partial void OnAfterFaqUpdated(DOOH.Server.Models.DOOHDB.Faq item);

        [HttpPut("/odata/DOOHDB/Faqs(FaqId={FaqId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutFaq(int key, [FromBody]DOOH.Server.Models.DOOHDB.Faq item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.FaqId != key))
                {
                    return BadRequest();
                }
                this.OnFaqUpdated(item);
                this.context.Faqs.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Faqs.Where(i => i.FaqId == key);
                
                this.OnAfterFaqUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/Faqs(FaqId={FaqId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchFaq(int key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.Faq> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Faqs.Where(i => i.FaqId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnFaqUpdated(item);
                this.context.Faqs.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Faqs.Where(i => i.FaqId == key);
                
                this.OnAfterFaqUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnFaqCreated(DOOH.Server.Models.DOOHDB.Faq item);
        partial void OnAfterFaqCreated(DOOH.Server.Models.DOOHDB.Faq item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.Faq item)
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

                this.OnFaqCreated(item);
                this.context.Faqs.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Faqs.Where(i => i.FaqId == item.FaqId);

                

                this.OnAfterFaqCreated(item);

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
