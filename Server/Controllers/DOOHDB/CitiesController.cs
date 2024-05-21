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
    [Route("odata/DOOHDB/Cities")]
    public partial class CitiesController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public CitiesController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.City> GetCities()
        {
            var items = this.context.Cities.AsQueryable<DOOH.Server.Models.DOOHDB.City>();
            this.OnCitiesRead(ref items);

            return items;
        }

        partial void OnCitiesRead(ref IQueryable<DOOH.Server.Models.DOOHDB.City> items);

        partial void OnCityGet(ref SingleResult<DOOH.Server.Models.DOOHDB.City> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/Cities(CityName={CityName})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.City> GetCity(string key)
        {
            var items = this.context.Cities.Where(i => i.CityName == Uri.UnescapeDataString(key));
            var result = SingleResult.Create(items);

            OnCityGet(ref result);

            return result;
        }
        partial void OnCityDeleted(DOOH.Server.Models.DOOHDB.City item);
        partial void OnAfterCityDeleted(DOOH.Server.Models.DOOHDB.City item);

        [HttpDelete("/odata/DOOHDB/Cities(CityName={CityName})")]
        public IActionResult DeleteCity(string key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Cities
                    .Where(i => i.CityName == Uri.UnescapeDataString(key))
                    .Include(i => i.Adboards)
                    .Include(i => i.Providers)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.City>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnCityDeleted(item);
                this.context.Cities.Remove(item);
                this.context.SaveChanges();
                this.OnAfterCityDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnCityUpdated(DOOH.Server.Models.DOOHDB.City item);
        partial void OnAfterCityUpdated(DOOH.Server.Models.DOOHDB.City item);

        [HttpPut("/odata/DOOHDB/Cities(CityName={CityName})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutCity(string key, [FromBody]DOOH.Server.Models.DOOHDB.City item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Cities
                    .Where(i => i.CityName == Uri.UnescapeDataString(key))
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.City>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnCityUpdated(item);
                this.context.Cities.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Cities.Where(i => i.CityName == Uri.UnescapeDataString(key));
                Request.QueryString = Request.QueryString.Add("$expand", "State");
                this.OnAfterCityUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/Cities(CityName={CityName})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchCity(string key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.City> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Cities
                    .Where(i => i.CityName == Uri.UnescapeDataString(key))
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<DOOH.Server.Models.DOOHDB.City>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnCityUpdated(item);
                this.context.Cities.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Cities.Where(i => i.CityName == Uri.UnescapeDataString(key));
                Request.QueryString = Request.QueryString.Add("$expand", "State");
                this.OnAfterCityUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnCityCreated(DOOH.Server.Models.DOOHDB.City item);
        partial void OnAfterCityCreated(DOOH.Server.Models.DOOHDB.City item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.City item)
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

                this.OnCityCreated(item);
                this.context.Cities.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Cities.Where(i => i.CityName == item.CityName);

                Request.QueryString = Request.QueryString.Add("$expand", "State");

                this.OnAfterCityCreated(item);

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
