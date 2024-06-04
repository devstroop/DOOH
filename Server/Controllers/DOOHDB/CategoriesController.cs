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
    [Route("odata/DOOHDB/Categories")]
    public partial class CategoriesController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public CategoriesController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.Category> GetCategories()
        {
            var items = this.context.Categories.AsQueryable<DOOH.Server.Models.DOOHDB.Category>();
            this.OnCategoriesRead(ref items);

            return items;
        }

        partial void OnCategoriesRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Category> items);

        partial void OnCategoryGet(ref SingleResult<DOOH.Server.Models.DOOHDB.Category> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/Categories(CategoryId={CategoryId})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.Category> GetCategory(int key)
        {
            var items = this.context.Categories.Where(i => i.CategoryId == key);
            var result = SingleResult.Create(items);

            OnCategoryGet(ref result);

            return result;
        }
        partial void OnCategoryDeleted(DOOH.Server.Models.DOOHDB.Category item);
        partial void OnAfterCategoryDeleted(DOOH.Server.Models.DOOHDB.Category item);

        [HttpDelete("/odata/DOOHDB/Categories(CategoryId={CategoryId})")]
        public IActionResult DeleteCategory(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Categories
                    .Where(i => i.CategoryId == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnCategoryDeleted(item);
                this.context.Categories.Remove(item);
                this.context.SaveChanges();
                this.OnAfterCategoryDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnCategoryUpdated(DOOH.Server.Models.DOOHDB.Category item);
        partial void OnAfterCategoryUpdated(DOOH.Server.Models.DOOHDB.Category item);

        [HttpPut("/odata/DOOHDB/Categories(CategoryId={CategoryId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutCategory(int key, [FromBody]DOOH.Server.Models.DOOHDB.Category item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.CategoryId != key))
                {
                    return BadRequest();
                }
                this.OnCategoryUpdated(item);
                this.context.Categories.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Categories.Where(i => i.CategoryId == key);
                
                this.OnAfterCategoryUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/Categories(CategoryId={CategoryId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchCategory(int key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.Category> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Categories.Where(i => i.CategoryId == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnCategoryUpdated(item);
                this.context.Categories.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Categories.Where(i => i.CategoryId == key);
                
                this.OnAfterCategoryUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnCategoryCreated(DOOH.Server.Models.DOOHDB.Category item);
        partial void OnAfterCategoryCreated(DOOH.Server.Models.DOOHDB.Category item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.Category item)
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

                this.OnCategoryCreated(item);
                this.context.Categories.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Categories.Where(i => i.CategoryId == item.CategoryId);

                

                this.OnAfterCategoryCreated(item);

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
