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
    [Route("odata/DOOHDB/UserInformations")]
    public partial class UserInformationsController : ODataController
    {
        private DOOH.Server.Data.DOOHDBContext context;

        public UserInformationsController(DOOH.Server.Data.DOOHDBContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DOOH.Server.Models.DOOHDB.UserInformation> GetUserInformations()
        {
            var items = this.context.UserInformations.AsQueryable<DOOH.Server.Models.DOOHDB.UserInformation>();
            this.OnUserInformationsRead(ref items);

            return items;
        }

        partial void OnUserInformationsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.UserInformation> items);

        partial void OnUserInformationGet(ref SingleResult<DOOH.Server.Models.DOOHDB.UserInformation> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DOOHDB/UserInformations(UserId={UserId})")]
        public SingleResult<DOOH.Server.Models.DOOHDB.UserInformation> GetUserInformation(string key)
        {
            var items = this.context.UserInformations.Where(i => i.UserId == Uri.UnescapeDataString(key));
            var result = SingleResult.Create(items);

            OnUserInformationGet(ref result);

            return result;
        }
        partial void OnUserInformationDeleted(DOOH.Server.Models.DOOHDB.UserInformation item);
        partial void OnAfterUserInformationDeleted(DOOH.Server.Models.DOOHDB.UserInformation item);

        [HttpDelete("/odata/DOOHDB/UserInformations(UserId={UserId})")]
        public IActionResult DeleteUserInformation(string key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.UserInformations
                    .Where(i => i.UserId == Uri.UnescapeDataString(key))
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnUserInformationDeleted(item);
                this.context.UserInformations.Remove(item);
                this.context.SaveChanges();
                this.OnAfterUserInformationDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnUserInformationUpdated(DOOH.Server.Models.DOOHDB.UserInformation item);
        partial void OnAfterUserInformationUpdated(DOOH.Server.Models.DOOHDB.UserInformation item);

        [HttpPut("/odata/DOOHDB/UserInformations(UserId={UserId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutUserInformation(string key, [FromBody]DOOH.Server.Models.DOOHDB.UserInformation item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.UserId != Uri.UnescapeDataString(key)))
                {
                    return BadRequest();
                }
                this.OnUserInformationUpdated(item);
                this.context.UserInformations.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.UserInformations.Where(i => i.UserId == Uri.UnescapeDataString(key));
                
                this.OnAfterUserInformationUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DOOHDB/UserInformations(UserId={UserId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchUserInformation(string key, [FromBody]Delta<DOOH.Server.Models.DOOHDB.UserInformation> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.UserInformations.Where(i => i.UserId == Uri.UnescapeDataString(key)).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnUserInformationUpdated(item);
                this.context.UserInformations.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.UserInformations.Where(i => i.UserId == Uri.UnescapeDataString(key));
                
                this.OnAfterUserInformationUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnUserInformationCreated(DOOH.Server.Models.DOOHDB.UserInformation item);
        partial void OnAfterUserInformationCreated(DOOH.Server.Models.DOOHDB.UserInformation item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DOOH.Server.Models.DOOHDB.UserInformation item)
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

                this.OnUserInformationCreated(item);
                this.context.UserInformations.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.UserInformations.Where(i => i.UserId == item.UserId);

                

                this.OnAfterUserInformationCreated(item);

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
