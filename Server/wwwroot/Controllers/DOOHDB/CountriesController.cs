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
    [Route("odata/DOOHDB/Countries")]
    public partial class CountriesController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public CountriesController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.Country> GetCountries()
        {
            var items = this.context.Countries.AsQueryable<DOOH.Server.Models.DOOHDB.Country>();
            this.OnCountriesRead(ref items);

            return items;
        }

        partial void OnCountriesRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Country> items);

        partial void OnCountryGet(ref SingleResult<DOOH.Server.Models.DOOHDB.Country> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/Countries(CountryName={CountryName})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.Country> GetCountry(string key)
        {
            var items = this.context.Countries.Where(i => i.CountryName == Uri.UnescapeDataString(key));
            var result = SingleResult.Create(items);

            OnCountryGet(ref result);

            return result;
        }
        partial void OnCountryDeleted(DOOH.Server.Models.DOOHDB.Country item);
        partial void OnAfterCountryDeleted(DOOH.Server.Models.DOOHDB.Country item);

        [HttpDelete("/odata/DOOHDB/Countries(CountryName={CountryName})")]
        public IActionResult DeleteCountry(string key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Countries
                    .Where(i => i.CountryName == Uri.UnescapeDataString(key))
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnCountryDeleted(item);
                this.context.Countries.Remove(item);
                this.context.SaveChanges();
                this.OnAfterCountryDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnCountryUpdated(DOOH.Server.Models.DOOHDB.Country item);
        partial void OnAfterCountryUpdated(DOOH.Server.Models.DOOHDB.Country item);

        [HttpPut("/odata/DOOHDB/Countries(CountryName={CountryName})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutCountry(string key, [FromBody]DOOH.Server.Models.DOOHDB.Country item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.CountryName != Uri.UnescapeDataString(key)))
                {
                    return BadRequest();
                }
                this.OnCountryUpdated(item);
                this.context.Countries.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Countries.Where(i => i.CountryName == Uri.UnescapeDataString(key));
                
                this.OnAfterCountryUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/Countries(CountryName={CountryName})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchCountry(string key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.Country> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Countries.Where(i => i.CountryName == Uri.UnescapeDataString(key)).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnCountryUpdated(item);
                this.context.Countries.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Countries.Where(i => i.CountryName == Uri.UnescapeDataString(key));
                
                this.OnAfterCountryUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnCountryCreated(DOOH.Server.Models.DOOHDB.Country item);
        partial void OnAfterCountryCreated(DOOH.Server.Models.DOOHDB.Country item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.Country item)
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

                this.OnCountryCreated(item);
                this.context.Countries.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Countries.Where(i => i.CountryName == item.CountryName);

                

                this.OnAfterCountryCreated(item);

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
