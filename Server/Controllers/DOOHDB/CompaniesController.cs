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
        [HttpGet("/odata/DOOHDB/Companies(Id={Id})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.Company> GetCompany(int key)
        {
            var items = this.context.Companies.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnCompanyGet(ref result);

            return result;
        }
        partial void OnCompanyDeleted(DOOH.Server.Models.DOOHDB.Company item);
        partial void OnAfterCompanyDeleted(DOOH.Server.Models.DOOHDB.Company item);

        [HttpDelete("/odata/DOOHDB/Companies(Id={Id})")]
        public IActionResult DeleteCompany(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Companies
                    .Where(i => i.Id == key)
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

        [HttpPut("/odata/DOOHDB/Companies(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutCompany(int key, [FromBody]DOOH.Server.Models.DOOHDB.Company item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.Id != key))
                {
                    return BadRequest();
                }
                this.OnCompanyUpdated(item);
                this.context.Companies.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Companies.Where(i => i.Id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "City,Country,State");
                this.OnAfterCompanyUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/Companies(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchCompany(int key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.Company> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Companies.Where(i => i.Id == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnCompanyUpdated(item);
                this.context.Companies.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Companies.Where(i => i.Id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "City,Country,State");
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

                var itemToReturn = this.context.Companies.Where(i => i.Id == item.Id);

                Request.QueryString = Request.QueryString.Add("$expand", "City,Country,State");

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
