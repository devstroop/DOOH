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
    [Route("odata/DOOHDB/Companies")]
    public partial class CompaniesController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public CompaniesController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.Company> GetCompanies()
        {
            var items = this.context.Companies.AsQueryable<DOOH.Server.Models.DOOHDB.Company>();
            this.OnCompaniesRead(ref items);

            return items;
        }

        partial void OnCompaniesRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Company> items);

        partial void OnCompanyGet(ref SingleResult<DOOH.Server.Models.DOOHDB.Company> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/Companies(Key={Key})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.Company> GetCompany(string key)
        {
            var items = this.context.Companies.Where(i => i.Key == Uri.UnescapeDataString(key));
            var result = SingleResult.Create(items);

            OnCompanyGet(ref result);

            return result;
        }
        partial void OnCompanyDeleted(DOOH.Server.Models.DOOHDB.Company item);
        partial void OnAfterCompanyDeleted(DOOH.Server.Models.DOOHDB.Company item);

        [HttpDelete("/odata/DOOHDB/Companies(Key={Key})")]
        public IActionResult DeleteCompany(string key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Companies
                    .Where(i => i.Key == Uri.UnescapeDataString(key))
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnCompanyDeleted(item);
                this.context.Companies.Remove(item);
                this.context.SaveChanges();
                this.OnAfterCompanyDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnCompanyUpdated(DOOH.Server.Models.DOOHDB.Company item);
        partial void OnAfterCompanyUpdated(DOOH.Server.Models.DOOHDB.Company item);

        [HttpPut("/odata/DOOHDB/Companies(Key={Key})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutCompany(string key, [FromBody]DOOH.Server.Models.DOOHDB.Company item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.Key != Uri.UnescapeDataString(key)))
                {
                    return BadRequest();
                }
                this.OnCompanyUpdated(item);
                this.context.Companies.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Companies.Where(i => i.Key == Uri.UnescapeDataString(key));
                
                this.OnAfterCompanyUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/Companies(Key={Key})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchCompany(string key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.Company> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Companies.Where(i => i.Key == Uri.UnescapeDataString(key)).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnCompanyUpdated(item);
                this.context.Companies.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Companies.Where(i => i.Key == Uri.UnescapeDataString(key));
                
                this.OnAfterCompanyUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnCompanyCreated(DOOH.Server.Models.DOOHDB.Company item);
        partial void OnAfterCompanyCreated(DOOH.Server.Models.DOOHDB.Company item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.Company item)
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

                this.OnCompanyCreated(item);
                this.context.Companies.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Companies.Where(i => i.Key == item.Key);

                

                this.OnAfterCompanyCreated(item);

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
