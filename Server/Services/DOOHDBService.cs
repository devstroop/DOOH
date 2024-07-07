using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using DOOH.Server.Data;

namespace DOOH.Server
{
    public partial class DOOHDBService
    {
        DOOHDBContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly DOOHDBContext context;
        private readonly NavigationManager navigationManager;

        public DOOHDBService(DOOHDBContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportAdboardsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/adboards/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/adboards/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAdboardsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/adboards/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/adboards/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAdboardsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Adboard> items);

        public async Task<IQueryable<DOOH.Server.Models.DOOHDB.Adboard>> GetAdboards(Query query = null)
        {
            var items = Context.Adboards.AsQueryable();

            items = items.Include(i => i.Category);
            items = items.Include(i => i.Display);
            items = items.Include(i => i.Motherboard);
            items = items.Include(i => i.Provider);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAdboardsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAdboardGet(DOOH.Server.Models.DOOHDB.Adboard item);
        partial void OnGetAdboardByAdboardId(ref IQueryable<DOOH.Server.Models.DOOHDB.Adboard> items);


        public async Task<DOOH.Server.Models.DOOHDB.Adboard> GetAdboardByAdboardId(int adboardid)
        {
            var items = Context.Adboards
                              .AsNoTracking()
                              .Where(i => i.AdboardId == adboardid);

            items = items.Include(i => i.Category);
            items = items.Include(i => i.Display);
            items = items.Include(i => i.Motherboard);
            items = items.Include(i => i.Provider);
 
            OnGetAdboardByAdboardId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAdboardGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAdboardCreated(DOOH.Server.Models.DOOHDB.Adboard item);
        partial void OnAfterAdboardCreated(DOOH.Server.Models.DOOHDB.Adboard item);

        public async Task<DOOH.Server.Models.DOOHDB.Adboard> CreateAdboard(DOOH.Server.Models.DOOHDB.Adboard adboard)
        {
            OnAdboardCreated(adboard);

            var existingItem = Context.Adboards
                              .Where(i => i.AdboardId == adboard.AdboardId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Adboards.Add(adboard);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(adboard).State = EntityState.Detached;
                throw;
            }

            OnAfterAdboardCreated(adboard);

            return adboard;
        }

        public async Task<DOOH.Server.Models.DOOHDB.Adboard> CancelAdboardChanges(DOOH.Server.Models.DOOHDB.Adboard item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAdboardUpdated(DOOH.Server.Models.DOOHDB.Adboard item);
        partial void OnAfterAdboardUpdated(DOOH.Server.Models.DOOHDB.Adboard item);

        public async Task<DOOH.Server.Models.DOOHDB.Adboard> UpdateAdboard(int adboardid, DOOH.Server.Models.DOOHDB.Adboard adboard)
        {
            OnAdboardUpdated(adboard);

            var itemToUpdate = Context.Adboards
                              .Where(i => i.AdboardId == adboard.AdboardId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(adboard);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAdboardUpdated(adboard);

            return adboard;
        }

        partial void OnAdboardDeleted(DOOH.Server.Models.DOOHDB.Adboard item);
        partial void OnAfterAdboardDeleted(DOOH.Server.Models.DOOHDB.Adboard item);

        public async Task<DOOH.Server.Models.DOOHDB.Adboard> DeleteAdboard(int adboardid)
        {
            var itemToDelete = Context.Adboards
                              .Where(i => i.AdboardId == adboardid)
                              .Include(i => i.AdboardImages)
                              .Include(i => i.AdboardNetworks)
                              .Include(i => i.AdboardStatuses)
                              .Include(i => i.AdboardWifis)
                              .Include(i => i.Analytics)
                              .Include(i => i.CampaignAdboards)
                              .Include(i => i.Earnings)
                              .Include(i => i.ScheduleAdboards)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAdboardDeleted(itemToDelete);


            Context.Adboards.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAdboardDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAdboardImagesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/adboardimages/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/adboardimages/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAdboardImagesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/adboardimages/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/adboardimages/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAdboardImagesRead(ref IQueryable<DOOH.Server.Models.DOOHDB.AdboardImage> items);

        public async Task<IQueryable<DOOH.Server.Models.DOOHDB.AdboardImage>> GetAdboardImages(Query query = null)
        {
            var items = Context.AdboardImages.AsQueryable();

            items = items.Include(i => i.Adboard);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAdboardImagesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAdboardImageGet(DOOH.Server.Models.DOOHDB.AdboardImage item);
        partial void OnGetAdboardImageByAdboardImageId(ref IQueryable<DOOH.Server.Models.DOOHDB.AdboardImage> items);


        public async Task<DOOH.Server.Models.DOOHDB.AdboardImage> GetAdboardImageByAdboardImageId(int adboardimageid)
        {
            var items = Context.AdboardImages
                              .AsNoTracking()
                              .Where(i => i.AdboardImageId == adboardimageid);

            items = items.Include(i => i.Adboard);
 
            OnGetAdboardImageByAdboardImageId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAdboardImageGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAdboardImageCreated(DOOH.Server.Models.DOOHDB.AdboardImage item);
        partial void OnAfterAdboardImageCreated(DOOH.Server.Models.DOOHDB.AdboardImage item);

        public async Task<DOOH.Server.Models.DOOHDB.AdboardImage> CreateAdboardImage(DOOH.Server.Models.DOOHDB.AdboardImage adboardimage)
        {
            OnAdboardImageCreated(adboardimage);

            var existingItem = Context.AdboardImages
                              .Where(i => i.AdboardImageId == adboardimage.AdboardImageId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AdboardImages.Add(adboardimage);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(adboardimage).State = EntityState.Detached;
                throw;
            }

            OnAfterAdboardImageCreated(adboardimage);

            return adboardimage;
        }

        public async Task<DOOH.Server.Models.DOOHDB.AdboardImage> CancelAdboardImageChanges(DOOH.Server.Models.DOOHDB.AdboardImage item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAdboardImageUpdated(DOOH.Server.Models.DOOHDB.AdboardImage item);
        partial void OnAfterAdboardImageUpdated(DOOH.Server.Models.DOOHDB.AdboardImage item);

        public async Task<DOOH.Server.Models.DOOHDB.AdboardImage> UpdateAdboardImage(int adboardimageid, DOOH.Server.Models.DOOHDB.AdboardImage adboardimage)
        {
            OnAdboardImageUpdated(adboardimage);

            var itemToUpdate = Context.AdboardImages
                              .Where(i => i.AdboardImageId == adboardimage.AdboardImageId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(adboardimage);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAdboardImageUpdated(adboardimage);

            return adboardimage;
        }

        partial void OnAdboardImageDeleted(DOOH.Server.Models.DOOHDB.AdboardImage item);
        partial void OnAfterAdboardImageDeleted(DOOH.Server.Models.DOOHDB.AdboardImage item);

        public async Task<DOOH.Server.Models.DOOHDB.AdboardImage> DeleteAdboardImage(int adboardimageid)
        {
            var itemToDelete = Context.AdboardImages
                              .Where(i => i.AdboardImageId == adboardimageid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAdboardImageDeleted(itemToDelete);


            Context.AdboardImages.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAdboardImageDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAdboardNetworksToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/adboardnetworks/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/adboardnetworks/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAdboardNetworksToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/adboardnetworks/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/adboardnetworks/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAdboardNetworksRead(ref IQueryable<DOOH.Server.Models.DOOHDB.AdboardNetwork> items);

        public async Task<IQueryable<DOOH.Server.Models.DOOHDB.AdboardNetwork>> GetAdboardNetworks(Query query = null)
        {
            var items = Context.AdboardNetworks.AsQueryable();

            items = items.Include(i => i.Adboard);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAdboardNetworksRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAdboardNetworkGet(DOOH.Server.Models.DOOHDB.AdboardNetwork item);
        partial void OnGetAdboardNetworkByAdboardId(ref IQueryable<DOOH.Server.Models.DOOHDB.AdboardNetwork> items);


        public async Task<DOOH.Server.Models.DOOHDB.AdboardNetwork> GetAdboardNetworkByAdboardId(int adboardid)
        {
            var items = Context.AdboardNetworks
                              .AsNoTracking()
                              .Where(i => i.AdboardId == adboardid);

            items = items.Include(i => i.Adboard);
 
            OnGetAdboardNetworkByAdboardId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAdboardNetworkGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAdboardNetworkCreated(DOOH.Server.Models.DOOHDB.AdboardNetwork item);
        partial void OnAfterAdboardNetworkCreated(DOOH.Server.Models.DOOHDB.AdboardNetwork item);

        public async Task<DOOH.Server.Models.DOOHDB.AdboardNetwork> CreateAdboardNetwork(DOOH.Server.Models.DOOHDB.AdboardNetwork adboardnetwork)
        {
            OnAdboardNetworkCreated(adboardnetwork);

            var existingItem = Context.AdboardNetworks
                              .Where(i => i.AdboardId == adboardnetwork.AdboardId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AdboardNetworks.Add(adboardnetwork);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(adboardnetwork).State = EntityState.Detached;
                throw;
            }

            OnAfterAdboardNetworkCreated(adboardnetwork);

            return adboardnetwork;
        }

        public async Task<DOOH.Server.Models.DOOHDB.AdboardNetwork> CancelAdboardNetworkChanges(DOOH.Server.Models.DOOHDB.AdboardNetwork item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAdboardNetworkUpdated(DOOH.Server.Models.DOOHDB.AdboardNetwork item);
        partial void OnAfterAdboardNetworkUpdated(DOOH.Server.Models.DOOHDB.AdboardNetwork item);

        public async Task<DOOH.Server.Models.DOOHDB.AdboardNetwork> UpdateAdboardNetwork(int adboardid, DOOH.Server.Models.DOOHDB.AdboardNetwork adboardnetwork)
        {
            OnAdboardNetworkUpdated(adboardnetwork);

            var itemToUpdate = Context.AdboardNetworks
                              .Where(i => i.AdboardId == adboardnetwork.AdboardId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(adboardnetwork);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAdboardNetworkUpdated(adboardnetwork);

            return adboardnetwork;
        }

        partial void OnAdboardNetworkDeleted(DOOH.Server.Models.DOOHDB.AdboardNetwork item);
        partial void OnAfterAdboardNetworkDeleted(DOOH.Server.Models.DOOHDB.AdboardNetwork item);

        public async Task<DOOH.Server.Models.DOOHDB.AdboardNetwork> DeleteAdboardNetwork(int adboardid)
        {
            var itemToDelete = Context.AdboardNetworks
                              .Where(i => i.AdboardId == adboardid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAdboardNetworkDeleted(itemToDelete);


            Context.AdboardNetworks.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAdboardNetworkDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAdboardStatusesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/adboardstatuses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/adboardstatuses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAdboardStatusesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/adboardstatuses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/adboardstatuses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAdboardStatusesRead(ref IQueryable<DOOH.Server.Models.DOOHDB.AdboardStatus> items);

        public async Task<IQueryable<DOOH.Server.Models.DOOHDB.AdboardStatus>> GetAdboardStatuses(Query query = null)
        {
            var items = Context.AdboardStatuses.AsQueryable();

            items = items.Include(i => i.Adboard);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAdboardStatusesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAdboardStatusGet(DOOH.Server.Models.DOOHDB.AdboardStatus item);
        partial void OnGetAdboardStatusByAdboardId(ref IQueryable<DOOH.Server.Models.DOOHDB.AdboardStatus> items);


        public async Task<DOOH.Server.Models.DOOHDB.AdboardStatus> GetAdboardStatusByAdboardId(int adboardid)
        {
            var items = Context.AdboardStatuses
                              .AsNoTracking()
                              .Where(i => i.AdboardId == adboardid);

            items = items.Include(i => i.Adboard);
 
            OnGetAdboardStatusByAdboardId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAdboardStatusGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAdboardStatusCreated(DOOH.Server.Models.DOOHDB.AdboardStatus item);
        partial void OnAfterAdboardStatusCreated(DOOH.Server.Models.DOOHDB.AdboardStatus item);

        public async Task<DOOH.Server.Models.DOOHDB.AdboardStatus> CreateAdboardStatus(DOOH.Server.Models.DOOHDB.AdboardStatus adboardstatus)
        {
            OnAdboardStatusCreated(adboardstatus);

            var existingItem = Context.AdboardStatuses
                              .Where(i => i.AdboardId == adboardstatus.AdboardId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AdboardStatuses.Add(adboardstatus);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(adboardstatus).State = EntityState.Detached;
                throw;
            }

            OnAfterAdboardStatusCreated(adboardstatus);

            return adboardstatus;
        }

        public async Task<DOOH.Server.Models.DOOHDB.AdboardStatus> CancelAdboardStatusChanges(DOOH.Server.Models.DOOHDB.AdboardStatus item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAdboardStatusUpdated(DOOH.Server.Models.DOOHDB.AdboardStatus item);
        partial void OnAfterAdboardStatusUpdated(DOOH.Server.Models.DOOHDB.AdboardStatus item);

        public async Task<DOOH.Server.Models.DOOHDB.AdboardStatus> UpdateAdboardStatus(int adboardid, DOOH.Server.Models.DOOHDB.AdboardStatus adboardstatus)
        {
            OnAdboardStatusUpdated(adboardstatus);

            var itemToUpdate = Context.AdboardStatuses
                              .Where(i => i.AdboardId == adboardstatus.AdboardId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(adboardstatus);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAdboardStatusUpdated(adboardstatus);

            return adboardstatus;
        }

        partial void OnAdboardStatusDeleted(DOOH.Server.Models.DOOHDB.AdboardStatus item);
        partial void OnAfterAdboardStatusDeleted(DOOH.Server.Models.DOOHDB.AdboardStatus item);

        public async Task<DOOH.Server.Models.DOOHDB.AdboardStatus> DeleteAdboardStatus(int adboardid)
        {
            var itemToDelete = Context.AdboardStatuses
                              .Where(i => i.AdboardId == adboardid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAdboardStatusDeleted(itemToDelete);


            Context.AdboardStatuses.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAdboardStatusDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAdboardWifisToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/adboardwifis/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/adboardwifis/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAdboardWifisToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/adboardwifis/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/adboardwifis/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAdboardWifisRead(ref IQueryable<DOOH.Server.Models.DOOHDB.AdboardWifi> items);

        public async Task<IQueryable<DOOH.Server.Models.DOOHDB.AdboardWifi>> GetAdboardWifis(Query query = null)
        {
            var items = Context.AdboardWifis.AsQueryable();

            items = items.Include(i => i.Adboard);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAdboardWifisRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAdboardWifiGet(DOOH.Server.Models.DOOHDB.AdboardWifi item);
        partial void OnGetAdboardWifiByAdboardId(ref IQueryable<DOOH.Server.Models.DOOHDB.AdboardWifi> items);


        public async Task<DOOH.Server.Models.DOOHDB.AdboardWifi> GetAdboardWifiByAdboardId(int adboardid)
        {
            var items = Context.AdboardWifis
                              .AsNoTracking()
                              .Where(i => i.AdboardId == adboardid);

            items = items.Include(i => i.Adboard);
 
            OnGetAdboardWifiByAdboardId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAdboardWifiGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAdboardWifiCreated(DOOH.Server.Models.DOOHDB.AdboardWifi item);
        partial void OnAfterAdboardWifiCreated(DOOH.Server.Models.DOOHDB.AdboardWifi item);

        public async Task<DOOH.Server.Models.DOOHDB.AdboardWifi> CreateAdboardWifi(DOOH.Server.Models.DOOHDB.AdboardWifi adboardwifi)
        {
            OnAdboardWifiCreated(adboardwifi);

            var existingItem = Context.AdboardWifis
                              .Where(i => i.AdboardId == adboardwifi.AdboardId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.AdboardWifis.Add(adboardwifi);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(adboardwifi).State = EntityState.Detached;
                throw;
            }

            OnAfterAdboardWifiCreated(adboardwifi);

            return adboardwifi;
        }

        public async Task<DOOH.Server.Models.DOOHDB.AdboardWifi> CancelAdboardWifiChanges(DOOH.Server.Models.DOOHDB.AdboardWifi item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAdboardWifiUpdated(DOOH.Server.Models.DOOHDB.AdboardWifi item);
        partial void OnAfterAdboardWifiUpdated(DOOH.Server.Models.DOOHDB.AdboardWifi item);

        public async Task<DOOH.Server.Models.DOOHDB.AdboardWifi> UpdateAdboardWifi(int adboardid, DOOH.Server.Models.DOOHDB.AdboardWifi adboardwifi)
        {
            OnAdboardWifiUpdated(adboardwifi);

            var itemToUpdate = Context.AdboardWifis
                              .Where(i => i.AdboardId == adboardwifi.AdboardId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(adboardwifi);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAdboardWifiUpdated(adboardwifi);

            return adboardwifi;
        }

        partial void OnAdboardWifiDeleted(DOOH.Server.Models.DOOHDB.AdboardWifi item);
        partial void OnAfterAdboardWifiDeleted(DOOH.Server.Models.DOOHDB.AdboardWifi item);

        public async Task<DOOH.Server.Models.DOOHDB.AdboardWifi> DeleteAdboardWifi(int adboardid)
        {
            var itemToDelete = Context.AdboardWifis
                              .Where(i => i.AdboardId == adboardid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAdboardWifiDeleted(itemToDelete);


            Context.AdboardWifis.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAdboardWifiDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAdvertisementsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/advertisements/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/advertisements/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAdvertisementsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/advertisements/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/advertisements/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAdvertisementsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Advertisement> items);

        public async Task<IQueryable<DOOH.Server.Models.DOOHDB.Advertisement>> GetAdvertisements(Query query = null)
        {
            var items = Context.Advertisements.AsQueryable();

            items = items.Include(i => i.Campaign);
            items = items.Include(i => i.Upload);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAdvertisementsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAdvertisementGet(DOOH.Server.Models.DOOHDB.Advertisement item);
        partial void OnGetAdvertisementByAdvertisementId(ref IQueryable<DOOH.Server.Models.DOOHDB.Advertisement> items);


        public async Task<DOOH.Server.Models.DOOHDB.Advertisement> GetAdvertisementByAdvertisementId(int advertisementid)
        {
            var items = Context.Advertisements
                              .AsNoTracking()
                              .Where(i => i.AdvertisementId == advertisementid);

            items = items.Include(i => i.Campaign);
            items = items.Include(i => i.Upload);
 
            OnGetAdvertisementByAdvertisementId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAdvertisementGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAdvertisementCreated(DOOH.Server.Models.DOOHDB.Advertisement item);
        partial void OnAfterAdvertisementCreated(DOOH.Server.Models.DOOHDB.Advertisement item);

        public async Task<DOOH.Server.Models.DOOHDB.Advertisement> CreateAdvertisement(DOOH.Server.Models.DOOHDB.Advertisement advertisement)
        {
            OnAdvertisementCreated(advertisement);

            var existingItem = Context.Advertisements
                              .Where(i => i.AdvertisementId == advertisement.AdvertisementId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Advertisements.Add(advertisement);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(advertisement).State = EntityState.Detached;
                throw;
            }

            OnAfterAdvertisementCreated(advertisement);

            return advertisement;
        }

        public async Task<DOOH.Server.Models.DOOHDB.Advertisement> CancelAdvertisementChanges(DOOH.Server.Models.DOOHDB.Advertisement item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAdvertisementUpdated(DOOH.Server.Models.DOOHDB.Advertisement item);
        partial void OnAfterAdvertisementUpdated(DOOH.Server.Models.DOOHDB.Advertisement item);

        public async Task<DOOH.Server.Models.DOOHDB.Advertisement> UpdateAdvertisement(int advertisementid, DOOH.Server.Models.DOOHDB.Advertisement advertisement)
        {
            OnAdvertisementUpdated(advertisement);

            var itemToUpdate = Context.Advertisements
                              .Where(i => i.AdvertisementId == advertisement.AdvertisementId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(advertisement);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAdvertisementUpdated(advertisement);

            return advertisement;
        }

        partial void OnAdvertisementDeleted(DOOH.Server.Models.DOOHDB.Advertisement item);
        partial void OnAfterAdvertisementDeleted(DOOH.Server.Models.DOOHDB.Advertisement item);

        public async Task<DOOH.Server.Models.DOOHDB.Advertisement> DeleteAdvertisement(int advertisementid)
        {
            var itemToDelete = Context.Advertisements
                              .Where(i => i.AdvertisementId == advertisementid)
                              .Include(i => i.Analytics)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAdvertisementDeleted(itemToDelete);


            Context.Advertisements.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAdvertisementDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportAnalyticsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/analytics/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/analytics/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAnalyticsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/analytics/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/analytics/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAnalyticsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Analytic> items);

        public async Task<IQueryable<DOOH.Server.Models.DOOHDB.Analytic>> GetAnalytics(Query query = null)
        {
            var items = Context.Analytics.AsQueryable();

            items = items.Include(i => i.Adboard);
            items = items.Include(i => i.Advertisement);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnAnalyticsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAnalyticGet(DOOH.Server.Models.DOOHDB.Analytic item);
        partial void OnGetAnalyticByAnalyticId(ref IQueryable<DOOH.Server.Models.DOOHDB.Analytic> items);


        public async Task<DOOH.Server.Models.DOOHDB.Analytic> GetAnalyticByAnalyticId(int analyticid)
        {
            var items = Context.Analytics
                              .AsNoTracking()
                              .Where(i => i.AnalyticId == analyticid);

            items = items.Include(i => i.Adboard);
            items = items.Include(i => i.Advertisement);
 
            OnGetAnalyticByAnalyticId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnAnalyticGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnAnalyticCreated(DOOH.Server.Models.DOOHDB.Analytic item);
        partial void OnAfterAnalyticCreated(DOOH.Server.Models.DOOHDB.Analytic item);

        public async Task<DOOH.Server.Models.DOOHDB.Analytic> CreateAnalytic(DOOH.Server.Models.DOOHDB.Analytic analytic)
        {
            OnAnalyticCreated(analytic);

            var existingItem = Context.Analytics
                              .Where(i => i.AnalyticId == analytic.AnalyticId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Analytics.Add(analytic);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(analytic).State = EntityState.Detached;
                throw;
            }

            OnAfterAnalyticCreated(analytic);

            return analytic;
        }

        public async Task<DOOH.Server.Models.DOOHDB.Analytic> CancelAnalyticChanges(DOOH.Server.Models.DOOHDB.Analytic item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAnalyticUpdated(DOOH.Server.Models.DOOHDB.Analytic item);
        partial void OnAfterAnalyticUpdated(DOOH.Server.Models.DOOHDB.Analytic item);

        public async Task<DOOH.Server.Models.DOOHDB.Analytic> UpdateAnalytic(int analyticid, DOOH.Server.Models.DOOHDB.Analytic analytic)
        {
            OnAnalyticUpdated(analytic);

            var itemToUpdate = Context.Analytics
                              .Where(i => i.AnalyticId == analytic.AnalyticId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(analytic);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAnalyticUpdated(analytic);

            return analytic;
        }

        partial void OnAnalyticDeleted(DOOH.Server.Models.DOOHDB.Analytic item);
        partial void OnAfterAnalyticDeleted(DOOH.Server.Models.DOOHDB.Analytic item);

        public async Task<DOOH.Server.Models.DOOHDB.Analytic> DeleteAnalytic(int analyticid)
        {
            var itemToDelete = Context.Analytics
                              .Where(i => i.AnalyticId == analyticid)
                              .Include(i => i.Billings)
                              .Include(i => i.Earnings)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAnalyticDeleted(itemToDelete);


            Context.Analytics.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAnalyticDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportBillingsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/billings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/billings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportBillingsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/billings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/billings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnBillingsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Billing> items);

        public async Task<IQueryable<DOOH.Server.Models.DOOHDB.Billing>> GetBillings(Query query = null)
        {
            var items = Context.Billings.AsQueryable();

            items = items.Include(i => i.Analytic);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnBillingsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnBillingGet(DOOH.Server.Models.DOOHDB.Billing item);
        partial void OnGetBillingByBillingId(ref IQueryable<DOOH.Server.Models.DOOHDB.Billing> items);


        public async Task<DOOH.Server.Models.DOOHDB.Billing> GetBillingByBillingId(int billingid)
        {
            var items = Context.Billings
                              .AsNoTracking()
                              .Where(i => i.BillingId == billingid);

            items = items.Include(i => i.Analytic);
 
            OnGetBillingByBillingId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnBillingGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnBillingCreated(DOOH.Server.Models.DOOHDB.Billing item);
        partial void OnAfterBillingCreated(DOOH.Server.Models.DOOHDB.Billing item);

        public async Task<DOOH.Server.Models.DOOHDB.Billing> CreateBilling(DOOH.Server.Models.DOOHDB.Billing billing)
        {
            OnBillingCreated(billing);

            var existingItem = Context.Billings
                              .Where(i => i.BillingId == billing.BillingId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Billings.Add(billing);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(billing).State = EntityState.Detached;
                throw;
            }

            OnAfterBillingCreated(billing);

            return billing;
        }

        public async Task<DOOH.Server.Models.DOOHDB.Billing> CancelBillingChanges(DOOH.Server.Models.DOOHDB.Billing item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnBillingUpdated(DOOH.Server.Models.DOOHDB.Billing item);
        partial void OnAfterBillingUpdated(DOOH.Server.Models.DOOHDB.Billing item);

        public async Task<DOOH.Server.Models.DOOHDB.Billing> UpdateBilling(int billingid, DOOH.Server.Models.DOOHDB.Billing billing)
        {
            OnBillingUpdated(billing);

            var itemToUpdate = Context.Billings
                              .Where(i => i.BillingId == billing.BillingId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(billing);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterBillingUpdated(billing);

            return billing;
        }

        partial void OnBillingDeleted(DOOH.Server.Models.DOOHDB.Billing item);
        partial void OnAfterBillingDeleted(DOOH.Server.Models.DOOHDB.Billing item);

        public async Task<DOOH.Server.Models.DOOHDB.Billing> DeleteBilling(int billingid)
        {
            var itemToDelete = Context.Billings
                              .Where(i => i.BillingId == billingid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnBillingDeleted(itemToDelete);


            Context.Billings.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterBillingDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportBrandsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/brands/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/brands/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportBrandsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/brands/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/brands/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnBrandsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Brand> items);

        public async Task<IQueryable<DOOH.Server.Models.DOOHDB.Brand>> GetBrands(Query query = null)
        {
            var items = Context.Brands.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnBrandsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnBrandGet(DOOH.Server.Models.DOOHDB.Brand item);
        partial void OnGetBrandByBrandId(ref IQueryable<DOOH.Server.Models.DOOHDB.Brand> items);


        public async Task<DOOH.Server.Models.DOOHDB.Brand> GetBrandByBrandId(int brandid)
        {
            var items = Context.Brands
                              .AsNoTracking()
                              .Where(i => i.BrandId == brandid);

 
            OnGetBrandByBrandId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnBrandGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnBrandCreated(DOOH.Server.Models.DOOHDB.Brand item);
        partial void OnAfterBrandCreated(DOOH.Server.Models.DOOHDB.Brand item);

        public async Task<DOOH.Server.Models.DOOHDB.Brand> CreateBrand(DOOH.Server.Models.DOOHDB.Brand brand)
        {
            OnBrandCreated(brand);

            var existingItem = Context.Brands
                              .Where(i => i.BrandId == brand.BrandId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Brands.Add(brand);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(brand).State = EntityState.Detached;
                throw;
            }

            OnAfterBrandCreated(brand);

            return brand;
        }

        public async Task<DOOH.Server.Models.DOOHDB.Brand> CancelBrandChanges(DOOH.Server.Models.DOOHDB.Brand item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnBrandUpdated(DOOH.Server.Models.DOOHDB.Brand item);
        partial void OnAfterBrandUpdated(DOOH.Server.Models.DOOHDB.Brand item);

        public async Task<DOOH.Server.Models.DOOHDB.Brand> UpdateBrand(int brandid, DOOH.Server.Models.DOOHDB.Brand brand)
        {
            OnBrandUpdated(brand);

            var itemToUpdate = Context.Brands
                              .Where(i => i.BrandId == brand.BrandId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(brand);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterBrandUpdated(brand);

            return brand;
        }

        partial void OnBrandDeleted(DOOH.Server.Models.DOOHDB.Brand item);
        partial void OnAfterBrandDeleted(DOOH.Server.Models.DOOHDB.Brand item);

        public async Task<DOOH.Server.Models.DOOHDB.Brand> DeleteBrand(int brandid)
        {
            var itemToDelete = Context.Brands
                              .Where(i => i.BrandId == brandid)
                              .Include(i => i.Displays)
                              .Include(i => i.Motherboards)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnBrandDeleted(itemToDelete);


            Context.Brands.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterBrandDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportCampaignsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/campaigns/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/campaigns/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCampaignsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/campaigns/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/campaigns/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCampaignsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Campaign> items);

        public async Task<IQueryable<DOOH.Server.Models.DOOHDB.Campaign>> GetCampaigns(Query query = null)
        {
            var items = Context.Campaigns.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnCampaignsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCampaignGet(DOOH.Server.Models.DOOHDB.Campaign item);
        partial void OnGetCampaignByCampaignId(ref IQueryable<DOOH.Server.Models.DOOHDB.Campaign> items);


        public async Task<DOOH.Server.Models.DOOHDB.Campaign> GetCampaignByCampaignId(int campaignid)
        {
            var items = Context.Campaigns
                              .AsNoTracking()
                              .Where(i => i.CampaignId == campaignid);

 
            OnGetCampaignByCampaignId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnCampaignGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnCampaignCreated(DOOH.Server.Models.DOOHDB.Campaign item);
        partial void OnAfterCampaignCreated(DOOH.Server.Models.DOOHDB.Campaign item);

        public async Task<DOOH.Server.Models.DOOHDB.Campaign> CreateCampaign(DOOH.Server.Models.DOOHDB.Campaign campaign)
        {
            OnCampaignCreated(campaign);

            var existingItem = Context.Campaigns
                              .Where(i => i.CampaignId == campaign.CampaignId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Campaigns.Add(campaign);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(campaign).State = EntityState.Detached;
                throw;
            }

            OnAfterCampaignCreated(campaign);

            return campaign;
        }

        public async Task<DOOH.Server.Models.DOOHDB.Campaign> CancelCampaignChanges(DOOH.Server.Models.DOOHDB.Campaign item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCampaignUpdated(DOOH.Server.Models.DOOHDB.Campaign item);
        partial void OnAfterCampaignUpdated(DOOH.Server.Models.DOOHDB.Campaign item);

        public async Task<DOOH.Server.Models.DOOHDB.Campaign> UpdateCampaign(int campaignid, DOOH.Server.Models.DOOHDB.Campaign campaign)
        {
            OnCampaignUpdated(campaign);

            var itemToUpdate = Context.Campaigns
                              .Where(i => i.CampaignId == campaign.CampaignId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(campaign);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterCampaignUpdated(campaign);

            return campaign;
        }

        partial void OnCampaignDeleted(DOOH.Server.Models.DOOHDB.Campaign item);
        partial void OnAfterCampaignDeleted(DOOH.Server.Models.DOOHDB.Campaign item);

        public async Task<DOOH.Server.Models.DOOHDB.Campaign> DeleteCampaign(int campaignid)
        {
            var itemToDelete = Context.Campaigns
                              .Where(i => i.CampaignId == campaignid)
                              .Include(i => i.Advertisements)
                              .Include(i => i.CampaignAdboards)
                              .Include(i => i.Schedules)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCampaignDeleted(itemToDelete);


            Context.Campaigns.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCampaignDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportCampaignAdboardsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/campaignadboards/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/campaignadboards/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCampaignAdboardsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/campaignadboards/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/campaignadboards/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCampaignAdboardsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.CampaignAdboard> items);

        public async Task<IQueryable<DOOH.Server.Models.DOOHDB.CampaignAdboard>> GetCampaignAdboards(Query query = null)
        {
            var items = Context.CampaignAdboards.AsQueryable();

            items = items.Include(i => i.Adboard);
            items = items.Include(i => i.Campaign);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnCampaignAdboardsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCampaignAdboardGet(DOOH.Server.Models.DOOHDB.CampaignAdboard item);
        partial void OnGetCampaignAdboardByCampaignIdAndAdboardId(ref IQueryable<DOOH.Server.Models.DOOHDB.CampaignAdboard> items);


        public async Task<DOOH.Server.Models.DOOHDB.CampaignAdboard> GetCampaignAdboardByCampaignIdAndAdboardId(int campaignid, int adboardid)
        {
            var items = Context.CampaignAdboards
                              .AsNoTracking()
                              .Where(i => i.CampaignId == campaignid && i.AdboardId == adboardid);

            items = items.Include(i => i.Adboard);
            items = items.Include(i => i.Campaign);
 
            OnGetCampaignAdboardByCampaignIdAndAdboardId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnCampaignAdboardGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnCampaignAdboardCreated(DOOH.Server.Models.DOOHDB.CampaignAdboard item);
        partial void OnAfterCampaignAdboardCreated(DOOH.Server.Models.DOOHDB.CampaignAdboard item);

        public async Task<DOOH.Server.Models.DOOHDB.CampaignAdboard> CreateCampaignAdboard(DOOH.Server.Models.DOOHDB.CampaignAdboard campaignadboard)
        {
            OnCampaignAdboardCreated(campaignadboard);

            var existingItem = Context.CampaignAdboards
                              .Where(i => i.CampaignId == campaignadboard.CampaignId && i.AdboardId == campaignadboard.AdboardId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.CampaignAdboards.Add(campaignadboard);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(campaignadboard).State = EntityState.Detached;
                throw;
            }

            OnAfterCampaignAdboardCreated(campaignadboard);

            return campaignadboard;
        }

        public async Task<DOOH.Server.Models.DOOHDB.CampaignAdboard> CancelCampaignAdboardChanges(DOOH.Server.Models.DOOHDB.CampaignAdboard item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCampaignAdboardUpdated(DOOH.Server.Models.DOOHDB.CampaignAdboard item);
        partial void OnAfterCampaignAdboardUpdated(DOOH.Server.Models.DOOHDB.CampaignAdboard item);

        public async Task<DOOH.Server.Models.DOOHDB.CampaignAdboard> UpdateCampaignAdboard(int campaignid, int adboardid, DOOH.Server.Models.DOOHDB.CampaignAdboard campaignadboard)
        {
            OnCampaignAdboardUpdated(campaignadboard);

            var itemToUpdate = Context.CampaignAdboards
                              .Where(i => i.CampaignId == campaignadboard.CampaignId && i.AdboardId == campaignadboard.AdboardId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(campaignadboard);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterCampaignAdboardUpdated(campaignadboard);

            return campaignadboard;
        }

        partial void OnCampaignAdboardDeleted(DOOH.Server.Models.DOOHDB.CampaignAdboard item);
        partial void OnAfterCampaignAdboardDeleted(DOOH.Server.Models.DOOHDB.CampaignAdboard item);

        public async Task<DOOH.Server.Models.DOOHDB.CampaignAdboard> DeleteCampaignAdboard(int campaignid, int adboardid)
        {
            var itemToDelete = Context.CampaignAdboards
                              .Where(i => i.CampaignId == campaignid && i.AdboardId == adboardid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCampaignAdboardDeleted(itemToDelete);


            Context.CampaignAdboards.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCampaignAdboardDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportCampaignCriteriaToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/campaigncriteria/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/campaigncriteria/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCampaignCriteriaToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/campaigncriteria/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/campaigncriteria/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCampaignCriteriaRead(ref IQueryable<DOOH.Server.Models.DOOHDB.CampaignCriterion> items);

        public async Task<IQueryable<DOOH.Server.Models.DOOHDB.CampaignCriterion>> GetCampaignCriteria(Query query = null)
        {
            var items = Context.CampaignCriteria.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnCampaignCriteriaRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCampaignCriterionGet(DOOH.Server.Models.DOOHDB.CampaignCriterion item);
        partial void OnGetCampaignCriterionByCampaignCriteriaId(ref IQueryable<DOOH.Server.Models.DOOHDB.CampaignCriterion> items);


        public async Task<DOOH.Server.Models.DOOHDB.CampaignCriterion> GetCampaignCriterionByCampaignCriteriaId(int campaigncriteriaid)
        {
            var items = Context.CampaignCriteria
                              .AsNoTracking()
                              .Where(i => i.CampaignCriteriaId == campaigncriteriaid);

 
            OnGetCampaignCriterionByCampaignCriteriaId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnCampaignCriterionGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnCampaignCriterionCreated(DOOH.Server.Models.DOOHDB.CampaignCriterion item);
        partial void OnAfterCampaignCriterionCreated(DOOH.Server.Models.DOOHDB.CampaignCriterion item);

        public async Task<DOOH.Server.Models.DOOHDB.CampaignCriterion> CreateCampaignCriterion(DOOH.Server.Models.DOOHDB.CampaignCriterion campaigncriterion)
        {
            OnCampaignCriterionCreated(campaigncriterion);

            var existingItem = Context.CampaignCriteria
                              .Where(i => i.CampaignCriteriaId == campaigncriterion.CampaignCriteriaId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.CampaignCriteria.Add(campaigncriterion);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(campaigncriterion).State = EntityState.Detached;
                throw;
            }

            OnAfterCampaignCriterionCreated(campaigncriterion);

            return campaigncriterion;
        }

        public async Task<DOOH.Server.Models.DOOHDB.CampaignCriterion> CancelCampaignCriterionChanges(DOOH.Server.Models.DOOHDB.CampaignCriterion item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCampaignCriterionUpdated(DOOH.Server.Models.DOOHDB.CampaignCriterion item);
        partial void OnAfterCampaignCriterionUpdated(DOOH.Server.Models.DOOHDB.CampaignCriterion item);

        public async Task<DOOH.Server.Models.DOOHDB.CampaignCriterion> UpdateCampaignCriterion(int campaigncriteriaid, DOOH.Server.Models.DOOHDB.CampaignCriterion campaigncriterion)
        {
            OnCampaignCriterionUpdated(campaigncriterion);

            var itemToUpdate = Context.CampaignCriteria
                              .Where(i => i.CampaignCriteriaId == campaigncriterion.CampaignCriteriaId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(campaigncriterion);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterCampaignCriterionUpdated(campaigncriterion);

            return campaigncriterion;
        }

        partial void OnCampaignCriterionDeleted(DOOH.Server.Models.DOOHDB.CampaignCriterion item);
        partial void OnAfterCampaignCriterionDeleted(DOOH.Server.Models.DOOHDB.CampaignCriterion item);

        public async Task<DOOH.Server.Models.DOOHDB.CampaignCriterion> DeleteCampaignCriterion(int campaigncriteriaid)
        {
            var itemToDelete = Context.CampaignCriteria
                              .Where(i => i.CampaignCriteriaId == campaigncriteriaid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCampaignCriterionDeleted(itemToDelete);


            Context.CampaignCriteria.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCampaignCriterionDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportCategoriesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/categories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/categories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCategoriesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/categories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/categories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCategoriesRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Category> items);

        public async Task<IQueryable<DOOH.Server.Models.DOOHDB.Category>> GetCategories(Query query = null)
        {
            var items = Context.Categories.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnCategoriesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCategoryGet(DOOH.Server.Models.DOOHDB.Category item);
        partial void OnGetCategoryByCategoryId(ref IQueryable<DOOH.Server.Models.DOOHDB.Category> items);


        public async Task<DOOH.Server.Models.DOOHDB.Category> GetCategoryByCategoryId(int categoryid)
        {
            var items = Context.Categories
                              .AsNoTracking()
                              .Where(i => i.CategoryId == categoryid);

 
            OnGetCategoryByCategoryId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnCategoryGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnCategoryCreated(DOOH.Server.Models.DOOHDB.Category item);
        partial void OnAfterCategoryCreated(DOOH.Server.Models.DOOHDB.Category item);

        public async Task<DOOH.Server.Models.DOOHDB.Category> CreateCategory(DOOH.Server.Models.DOOHDB.Category category)
        {
            OnCategoryCreated(category);

            var existingItem = Context.Categories
                              .Where(i => i.CategoryId == category.CategoryId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Categories.Add(category);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(category).State = EntityState.Detached;
                throw;
            }

            OnAfterCategoryCreated(category);

            return category;
        }

        public async Task<DOOH.Server.Models.DOOHDB.Category> CancelCategoryChanges(DOOH.Server.Models.DOOHDB.Category item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCategoryUpdated(DOOH.Server.Models.DOOHDB.Category item);
        partial void OnAfterCategoryUpdated(DOOH.Server.Models.DOOHDB.Category item);

        public async Task<DOOH.Server.Models.DOOHDB.Category> UpdateCategory(int categoryid, DOOH.Server.Models.DOOHDB.Category category)
        {
            OnCategoryUpdated(category);

            var itemToUpdate = Context.Categories
                              .Where(i => i.CategoryId == category.CategoryId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(category);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterCategoryUpdated(category);

            return category;
        }

        partial void OnCategoryDeleted(DOOH.Server.Models.DOOHDB.Category item);
        partial void OnAfterCategoryDeleted(DOOH.Server.Models.DOOHDB.Category item);

        public async Task<DOOH.Server.Models.DOOHDB.Category> DeleteCategory(int categoryid)
        {
            var itemToDelete = Context.Categories
                              .Where(i => i.CategoryId == categoryid)
                              .Include(i => i.Adboards)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCategoryDeleted(itemToDelete);


            Context.Categories.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCategoryDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportCompaniesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/companies/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/companies/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCompaniesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/companies/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/companies/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCompaniesRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Company> items);

        public async Task<IQueryable<DOOH.Server.Models.DOOHDB.Company>> GetCompanies(Query query = null)
        {
            var items = Context.Companies.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnCompaniesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCompanyGet(DOOH.Server.Models.DOOHDB.Company item);
        partial void OnGetCompanyByKey(ref IQueryable<DOOH.Server.Models.DOOHDB.Company> items);


        public async Task<DOOH.Server.Models.DOOHDB.Company> GetCompanyByKey(string key)
        {
            var items = Context.Companies
                              .AsNoTracking()
                              .Where(i => i.Key == key);

 
            OnGetCompanyByKey(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnCompanyGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnCompanyCreated(DOOH.Server.Models.DOOHDB.Company item);
        partial void OnAfterCompanyCreated(DOOH.Server.Models.DOOHDB.Company item);

        public async Task<DOOH.Server.Models.DOOHDB.Company> CreateCompany(DOOH.Server.Models.DOOHDB.Company company)
        {
            OnCompanyCreated(company);

            var existingItem = Context.Companies
                              .Where(i => i.Key == company.Key)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Companies.Add(company);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(company).State = EntityState.Detached;
                throw;
            }

            OnAfterCompanyCreated(company);

            return company;
        }

        public async Task<DOOH.Server.Models.DOOHDB.Company> CancelCompanyChanges(DOOH.Server.Models.DOOHDB.Company item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnCompanyUpdated(DOOH.Server.Models.DOOHDB.Company item);
        partial void OnAfterCompanyUpdated(DOOH.Server.Models.DOOHDB.Company item);

        public async Task<DOOH.Server.Models.DOOHDB.Company> UpdateCompany(string key, DOOH.Server.Models.DOOHDB.Company company)
        {
            OnCompanyUpdated(company);

            var itemToUpdate = Context.Companies
                              .Where(i => i.Key == company.Key)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(company);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterCompanyUpdated(company);

            return company;
        }

        partial void OnCompanyDeleted(DOOH.Server.Models.DOOHDB.Company item);
        partial void OnAfterCompanyDeleted(DOOH.Server.Models.DOOHDB.Company item);

        public async Task<DOOH.Server.Models.DOOHDB.Company> DeleteCompany(string key)
        {
            var itemToDelete = Context.Companies
                              .Where(i => i.Key == key)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCompanyDeleted(itemToDelete);


            Context.Companies.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCompanyDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportDisplaysToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/displays/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/displays/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportDisplaysToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/displays/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/displays/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnDisplaysRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Display> items);

        public async Task<IQueryable<DOOH.Server.Models.DOOHDB.Display>> GetDisplays(Query query = null)
        {
            var items = Context.Displays.AsQueryable();

            items = items.Include(i => i.Brand);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnDisplaysRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnDisplayGet(DOOH.Server.Models.DOOHDB.Display item);
        partial void OnGetDisplayByDisplayId(ref IQueryable<DOOH.Server.Models.DOOHDB.Display> items);


        public async Task<DOOH.Server.Models.DOOHDB.Display> GetDisplayByDisplayId(int displayid)
        {
            var items = Context.Displays
                              .AsNoTracking()
                              .Where(i => i.DisplayId == displayid);

            items = items.Include(i => i.Brand);
 
            OnGetDisplayByDisplayId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnDisplayGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnDisplayCreated(DOOH.Server.Models.DOOHDB.Display item);
        partial void OnAfterDisplayCreated(DOOH.Server.Models.DOOHDB.Display item);

        public async Task<DOOH.Server.Models.DOOHDB.Display> CreateDisplay(DOOH.Server.Models.DOOHDB.Display display)
        {
            OnDisplayCreated(display);

            var existingItem = Context.Displays
                              .Where(i => i.DisplayId == display.DisplayId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Displays.Add(display);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(display).State = EntityState.Detached;
                throw;
            }

            OnAfterDisplayCreated(display);

            return display;
        }

        public async Task<DOOH.Server.Models.DOOHDB.Display> CancelDisplayChanges(DOOH.Server.Models.DOOHDB.Display item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnDisplayUpdated(DOOH.Server.Models.DOOHDB.Display item);
        partial void OnAfterDisplayUpdated(DOOH.Server.Models.DOOHDB.Display item);

        public async Task<DOOH.Server.Models.DOOHDB.Display> UpdateDisplay(int displayid, DOOH.Server.Models.DOOHDB.Display display)
        {
            OnDisplayUpdated(display);

            var itemToUpdate = Context.Displays
                              .Where(i => i.DisplayId == display.DisplayId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(display);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterDisplayUpdated(display);

            return display;
        }

        partial void OnDisplayDeleted(DOOH.Server.Models.DOOHDB.Display item);
        partial void OnAfterDisplayDeleted(DOOH.Server.Models.DOOHDB.Display item);

        public async Task<DOOH.Server.Models.DOOHDB.Display> DeleteDisplay(int displayid)
        {
            var itemToDelete = Context.Displays
                              .Where(i => i.DisplayId == displayid)
                              .Include(i => i.Adboards)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnDisplayDeleted(itemToDelete);


            Context.Displays.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterDisplayDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportEarningsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/earnings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/earnings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportEarningsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/earnings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/earnings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnEarningsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Earning> items);

        public async Task<IQueryable<DOOH.Server.Models.DOOHDB.Earning>> GetEarnings(Query query = null)
        {
            var items = Context.Earnings.AsQueryable();

            items = items.Include(i => i.Adboard);
            items = items.Include(i => i.Analytic);
            items = items.Include(i => i.Provider);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnEarningsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnEarningGet(DOOH.Server.Models.DOOHDB.Earning item);
        partial void OnGetEarningByEarningId(ref IQueryable<DOOH.Server.Models.DOOHDB.Earning> items);


        public async Task<DOOH.Server.Models.DOOHDB.Earning> GetEarningByEarningId(int earningid)
        {
            var items = Context.Earnings
                              .AsNoTracking()
                              .Where(i => i.EarningId == earningid);

            items = items.Include(i => i.Adboard);
            items = items.Include(i => i.Analytic);
            items = items.Include(i => i.Provider);
 
            OnGetEarningByEarningId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnEarningGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnEarningCreated(DOOH.Server.Models.DOOHDB.Earning item);
        partial void OnAfterEarningCreated(DOOH.Server.Models.DOOHDB.Earning item);

        public async Task<DOOH.Server.Models.DOOHDB.Earning> CreateEarning(DOOH.Server.Models.DOOHDB.Earning earning)
        {
            OnEarningCreated(earning);

            var existingItem = Context.Earnings
                              .Where(i => i.EarningId == earning.EarningId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Earnings.Add(earning);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(earning).State = EntityState.Detached;
                throw;
            }

            OnAfterEarningCreated(earning);

            return earning;
        }

        public async Task<DOOH.Server.Models.DOOHDB.Earning> CancelEarningChanges(DOOH.Server.Models.DOOHDB.Earning item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnEarningUpdated(DOOH.Server.Models.DOOHDB.Earning item);
        partial void OnAfterEarningUpdated(DOOH.Server.Models.DOOHDB.Earning item);

        public async Task<DOOH.Server.Models.DOOHDB.Earning> UpdateEarning(int earningid, DOOH.Server.Models.DOOHDB.Earning earning)
        {
            OnEarningUpdated(earning);

            var itemToUpdate = Context.Earnings
                              .Where(i => i.EarningId == earning.EarningId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(earning);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterEarningUpdated(earning);

            return earning;
        }

        partial void OnEarningDeleted(DOOH.Server.Models.DOOHDB.Earning item);
        partial void OnAfterEarningDeleted(DOOH.Server.Models.DOOHDB.Earning item);

        public async Task<DOOH.Server.Models.DOOHDB.Earning> DeleteEarning(int earningid)
        {
            var itemToDelete = Context.Earnings
                              .Where(i => i.EarningId == earningid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnEarningDeleted(itemToDelete);


            Context.Earnings.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterEarningDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportFaqsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/faqs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/faqs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportFaqsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/faqs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/faqs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnFaqsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Faq> items);

        public async Task<IQueryable<DOOH.Server.Models.DOOHDB.Faq>> GetFaqs(Query query = null)
        {
            var items = Context.Faqs.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnFaqsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnFaqGet(DOOH.Server.Models.DOOHDB.Faq item);
        partial void OnGetFaqByFaqId(ref IQueryable<DOOH.Server.Models.DOOHDB.Faq> items);


        public async Task<DOOH.Server.Models.DOOHDB.Faq> GetFaqByFaqId(int faqid)
        {
            var items = Context.Faqs
                              .AsNoTracking()
                              .Where(i => i.FaqId == faqid);

 
            OnGetFaqByFaqId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnFaqGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnFaqCreated(DOOH.Server.Models.DOOHDB.Faq item);
        partial void OnAfterFaqCreated(DOOH.Server.Models.DOOHDB.Faq item);

        public async Task<DOOH.Server.Models.DOOHDB.Faq> CreateFaq(DOOH.Server.Models.DOOHDB.Faq faq)
        {
            OnFaqCreated(faq);

            var existingItem = Context.Faqs
                              .Where(i => i.FaqId == faq.FaqId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Faqs.Add(faq);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(faq).State = EntityState.Detached;
                throw;
            }

            OnAfterFaqCreated(faq);

            return faq;
        }

        public async Task<DOOH.Server.Models.DOOHDB.Faq> CancelFaqChanges(DOOH.Server.Models.DOOHDB.Faq item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnFaqUpdated(DOOH.Server.Models.DOOHDB.Faq item);
        partial void OnAfterFaqUpdated(DOOH.Server.Models.DOOHDB.Faq item);

        public async Task<DOOH.Server.Models.DOOHDB.Faq> UpdateFaq(int faqid, DOOH.Server.Models.DOOHDB.Faq faq)
        {
            OnFaqUpdated(faq);

            var itemToUpdate = Context.Faqs
                              .Where(i => i.FaqId == faq.FaqId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(faq);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterFaqUpdated(faq);

            return faq;
        }

        partial void OnFaqDeleted(DOOH.Server.Models.DOOHDB.Faq item);
        partial void OnAfterFaqDeleted(DOOH.Server.Models.DOOHDB.Faq item);

        public async Task<DOOH.Server.Models.DOOHDB.Faq> DeleteFaq(int faqid)
        {
            var itemToDelete = Context.Faqs
                              .Where(i => i.FaqId == faqid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnFaqDeleted(itemToDelete);


            Context.Faqs.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterFaqDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportMotherboardsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/motherboards/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/motherboards/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportMotherboardsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/motherboards/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/motherboards/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnMotherboardsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Motherboard> items);

        public async Task<IQueryable<DOOH.Server.Models.DOOHDB.Motherboard>> GetMotherboards(Query query = null)
        {
            var items = Context.Motherboards.AsQueryable();

            items = items.Include(i => i.Brand);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnMotherboardsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnMotherboardGet(DOOH.Server.Models.DOOHDB.Motherboard item);
        partial void OnGetMotherboardByMotherboardId(ref IQueryable<DOOH.Server.Models.DOOHDB.Motherboard> items);


        public async Task<DOOH.Server.Models.DOOHDB.Motherboard> GetMotherboardByMotherboardId(int motherboardid)
        {
            var items = Context.Motherboards
                              .AsNoTracking()
                              .Where(i => i.MotherboardId == motherboardid);

            items = items.Include(i => i.Brand);
 
            OnGetMotherboardByMotherboardId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnMotherboardGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnMotherboardCreated(DOOH.Server.Models.DOOHDB.Motherboard item);
        partial void OnAfterMotherboardCreated(DOOH.Server.Models.DOOHDB.Motherboard item);

        public async Task<DOOH.Server.Models.DOOHDB.Motherboard> CreateMotherboard(DOOH.Server.Models.DOOHDB.Motherboard motherboard)
        {
            OnMotherboardCreated(motherboard);

            var existingItem = Context.Motherboards
                              .Where(i => i.MotherboardId == motherboard.MotherboardId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Motherboards.Add(motherboard);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(motherboard).State = EntityState.Detached;
                throw;
            }

            OnAfterMotherboardCreated(motherboard);

            return motherboard;
        }

        public async Task<DOOH.Server.Models.DOOHDB.Motherboard> CancelMotherboardChanges(DOOH.Server.Models.DOOHDB.Motherboard item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnMotherboardUpdated(DOOH.Server.Models.DOOHDB.Motherboard item);
        partial void OnAfterMotherboardUpdated(DOOH.Server.Models.DOOHDB.Motherboard item);

        public async Task<DOOH.Server.Models.DOOHDB.Motherboard> UpdateMotherboard(int motherboardid, DOOH.Server.Models.DOOHDB.Motherboard motherboard)
        {
            OnMotherboardUpdated(motherboard);

            var itemToUpdate = Context.Motherboards
                              .Where(i => i.MotherboardId == motherboard.MotherboardId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(motherboard);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterMotherboardUpdated(motherboard);

            return motherboard;
        }

        partial void OnMotherboardDeleted(DOOH.Server.Models.DOOHDB.Motherboard item);
        partial void OnAfterMotherboardDeleted(DOOH.Server.Models.DOOHDB.Motherboard item);

        public async Task<DOOH.Server.Models.DOOHDB.Motherboard> DeleteMotherboard(int motherboardid)
        {
            var itemToDelete = Context.Motherboards
                              .Where(i => i.MotherboardId == motherboardid)
                              .Include(i => i.Adboards)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnMotherboardDeleted(itemToDelete);


            Context.Motherboards.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterMotherboardDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportPagesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/pages/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/pages/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportPagesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/pages/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/pages/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnPagesRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Page> items);

        public async Task<IQueryable<DOOH.Server.Models.DOOHDB.Page>> GetPages(Query query = null)
        {
            var items = Context.Pages.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnPagesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnPageGet(DOOH.Server.Models.DOOHDB.Page item);
        partial void OnGetPageBySlag(ref IQueryable<DOOH.Server.Models.DOOHDB.Page> items);


        public async Task<DOOH.Server.Models.DOOHDB.Page> GetPageBySlag(string slag)
        {
            var items = Context.Pages
                              .AsNoTracking()
                              .Where(i => i.Slag == slag);

 
            OnGetPageBySlag(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnPageGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnPageCreated(DOOH.Server.Models.DOOHDB.Page item);
        partial void OnAfterPageCreated(DOOH.Server.Models.DOOHDB.Page item);

        public async Task<DOOH.Server.Models.DOOHDB.Page> CreatePage(DOOH.Server.Models.DOOHDB.Page page)
        {
            OnPageCreated(page);

            var existingItem = Context.Pages
                              .Where(i => i.Slag == page.Slag)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Pages.Add(page);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(page).State = EntityState.Detached;
                throw;
            }

            OnAfterPageCreated(page);

            return page;
        }

        public async Task<DOOH.Server.Models.DOOHDB.Page> CancelPageChanges(DOOH.Server.Models.DOOHDB.Page item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnPageUpdated(DOOH.Server.Models.DOOHDB.Page item);
        partial void OnAfterPageUpdated(DOOH.Server.Models.DOOHDB.Page item);

        public async Task<DOOH.Server.Models.DOOHDB.Page> UpdatePage(string slag, DOOH.Server.Models.DOOHDB.Page page)
        {
            OnPageUpdated(page);

            var itemToUpdate = Context.Pages
                              .Where(i => i.Slag == page.Slag)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(page);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterPageUpdated(page);

            return page;
        }

        partial void OnPageDeleted(DOOH.Server.Models.DOOHDB.Page item);
        partial void OnAfterPageDeleted(DOOH.Server.Models.DOOHDB.Page item);

        public async Task<DOOH.Server.Models.DOOHDB.Page> DeletePage(string slag)
        {
            var itemToDelete = Context.Pages
                              .Where(i => i.Slag == slag)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnPageDeleted(itemToDelete);


            Context.Pages.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterPageDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportProvidersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/providers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/providers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportProvidersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/providers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/providers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnProvidersRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Provider> items);

        public async Task<IQueryable<DOOH.Server.Models.DOOHDB.Provider>> GetProviders(Query query = null)
        {
            var items = Context.Providers.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnProvidersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnProviderGet(DOOH.Server.Models.DOOHDB.Provider item);
        partial void OnGetProviderByProviderId(ref IQueryable<DOOH.Server.Models.DOOHDB.Provider> items);


        public async Task<DOOH.Server.Models.DOOHDB.Provider> GetProviderByProviderId(int providerid)
        {
            var items = Context.Providers
                              .AsNoTracking()
                              .Where(i => i.ProviderId == providerid);

 
            OnGetProviderByProviderId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnProviderGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnProviderCreated(DOOH.Server.Models.DOOHDB.Provider item);
        partial void OnAfterProviderCreated(DOOH.Server.Models.DOOHDB.Provider item);

        public async Task<DOOH.Server.Models.DOOHDB.Provider> CreateProvider(DOOH.Server.Models.DOOHDB.Provider provider)
        {
            OnProviderCreated(provider);

            var existingItem = Context.Providers
                              .Where(i => i.ProviderId == provider.ProviderId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Providers.Add(provider);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(provider).State = EntityState.Detached;
                throw;
            }

            OnAfterProviderCreated(provider);

            return provider;
        }

        public async Task<DOOH.Server.Models.DOOHDB.Provider> CancelProviderChanges(DOOH.Server.Models.DOOHDB.Provider item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnProviderUpdated(DOOH.Server.Models.DOOHDB.Provider item);
        partial void OnAfterProviderUpdated(DOOH.Server.Models.DOOHDB.Provider item);

        public async Task<DOOH.Server.Models.DOOHDB.Provider> UpdateProvider(int providerid, DOOH.Server.Models.DOOHDB.Provider provider)
        {
            OnProviderUpdated(provider);

            var itemToUpdate = Context.Providers
                              .Where(i => i.ProviderId == provider.ProviderId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(provider);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterProviderUpdated(provider);

            return provider;
        }

        partial void OnProviderDeleted(DOOH.Server.Models.DOOHDB.Provider item);
        partial void OnAfterProviderDeleted(DOOH.Server.Models.DOOHDB.Provider item);

        public async Task<DOOH.Server.Models.DOOHDB.Provider> DeleteProvider(int providerid)
        {
            var itemToDelete = Context.Providers
                              .Where(i => i.ProviderId == providerid)
                              .Include(i => i.Adboards)
                              .Include(i => i.Earnings)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnProviderDeleted(itemToDelete);


            Context.Providers.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterProviderDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportSchedulesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/schedules/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/schedules/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSchedulesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/schedules/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/schedules/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSchedulesRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Schedule> items);

        public async Task<IQueryable<DOOH.Server.Models.DOOHDB.Schedule>> GetSchedules(Query query = null)
        {
            var items = Context.Schedules.AsQueryable();

            items = items.Include(i => i.Campaign);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnSchedulesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnScheduleGet(DOOH.Server.Models.DOOHDB.Schedule item);
        partial void OnGetScheduleByScheduleId(ref IQueryable<DOOH.Server.Models.DOOHDB.Schedule> items);


        public async Task<DOOH.Server.Models.DOOHDB.Schedule> GetScheduleByScheduleId(int scheduleid)
        {
            var items = Context.Schedules
                              .AsNoTracking()
                              .Where(i => i.ScheduleId == scheduleid);

            items = items.Include(i => i.Campaign);
 
            OnGetScheduleByScheduleId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnScheduleGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnScheduleCreated(DOOH.Server.Models.DOOHDB.Schedule item);
        partial void OnAfterScheduleCreated(DOOH.Server.Models.DOOHDB.Schedule item);

        public async Task<DOOH.Server.Models.DOOHDB.Schedule> CreateSchedule(DOOH.Server.Models.DOOHDB.Schedule schedule)
        {
            OnScheduleCreated(schedule);

            var existingItem = Context.Schedules
                              .Where(i => i.ScheduleId == schedule.ScheduleId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Schedules.Add(schedule);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(schedule).State = EntityState.Detached;
                throw;
            }

            OnAfterScheduleCreated(schedule);

            return schedule;
        }

        public async Task<DOOH.Server.Models.DOOHDB.Schedule> CancelScheduleChanges(DOOH.Server.Models.DOOHDB.Schedule item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnScheduleUpdated(DOOH.Server.Models.DOOHDB.Schedule item);
        partial void OnAfterScheduleUpdated(DOOH.Server.Models.DOOHDB.Schedule item);

        public async Task<DOOH.Server.Models.DOOHDB.Schedule> UpdateSchedule(int scheduleid, DOOH.Server.Models.DOOHDB.Schedule schedule)
        {
            OnScheduleUpdated(schedule);

            var itemToUpdate = Context.Schedules
                              .Where(i => i.ScheduleId == schedule.ScheduleId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(schedule);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterScheduleUpdated(schedule);

            return schedule;
        }

        partial void OnScheduleDeleted(DOOH.Server.Models.DOOHDB.Schedule item);
        partial void OnAfterScheduleDeleted(DOOH.Server.Models.DOOHDB.Schedule item);

        public async Task<DOOH.Server.Models.DOOHDB.Schedule> DeleteSchedule(int scheduleid)
        {
            var itemToDelete = Context.Schedules
                              .Where(i => i.ScheduleId == scheduleid)
                              .Include(i => i.ScheduleAdboards)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnScheduleDeleted(itemToDelete);


            Context.Schedules.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterScheduleDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportScheduleAdboardsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/scheduleadboards/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/scheduleadboards/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportScheduleAdboardsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/scheduleadboards/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/scheduleadboards/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnScheduleAdboardsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.ScheduleAdboard> items);

        public async Task<IQueryable<DOOH.Server.Models.DOOHDB.ScheduleAdboard>> GetScheduleAdboards(Query query = null)
        {
            var items = Context.ScheduleAdboards.AsQueryable();

            items = items.Include(i => i.Adboard);
            items = items.Include(i => i.Schedule);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnScheduleAdboardsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnScheduleAdboardGet(DOOH.Server.Models.DOOHDB.ScheduleAdboard item);
        partial void OnGetScheduleAdboardByScheduleIdAndAdboardId(ref IQueryable<DOOH.Server.Models.DOOHDB.ScheduleAdboard> items);


        public async Task<DOOH.Server.Models.DOOHDB.ScheduleAdboard> GetScheduleAdboardByScheduleIdAndAdboardId(int scheduleid, int adboardid)
        {
            var items = Context.ScheduleAdboards
                              .AsNoTracking()
                              .Where(i => i.ScheduleId == scheduleid && i.AdboardId == adboardid);

            items = items.Include(i => i.Adboard);
            items = items.Include(i => i.Schedule);
 
            OnGetScheduleAdboardByScheduleIdAndAdboardId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnScheduleAdboardGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnScheduleAdboardCreated(DOOH.Server.Models.DOOHDB.ScheduleAdboard item);
        partial void OnAfterScheduleAdboardCreated(DOOH.Server.Models.DOOHDB.ScheduleAdboard item);

        public async Task<DOOH.Server.Models.DOOHDB.ScheduleAdboard> CreateScheduleAdboard(DOOH.Server.Models.DOOHDB.ScheduleAdboard scheduleadboard)
        {
            OnScheduleAdboardCreated(scheduleadboard);

            var existingItem = Context.ScheduleAdboards
                              .Where(i => i.ScheduleId == scheduleadboard.ScheduleId && i.AdboardId == scheduleadboard.AdboardId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.ScheduleAdboards.Add(scheduleadboard);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(scheduleadboard).State = EntityState.Detached;
                throw;
            }

            OnAfterScheduleAdboardCreated(scheduleadboard);

            return scheduleadboard;
        }

        public async Task<DOOH.Server.Models.DOOHDB.ScheduleAdboard> CancelScheduleAdboardChanges(DOOH.Server.Models.DOOHDB.ScheduleAdboard item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnScheduleAdboardUpdated(DOOH.Server.Models.DOOHDB.ScheduleAdboard item);
        partial void OnAfterScheduleAdboardUpdated(DOOH.Server.Models.DOOHDB.ScheduleAdboard item);

        public async Task<DOOH.Server.Models.DOOHDB.ScheduleAdboard> UpdateScheduleAdboard(int scheduleid, int adboardid, DOOH.Server.Models.DOOHDB.ScheduleAdboard scheduleadboard)
        {
            OnScheduleAdboardUpdated(scheduleadboard);

            var itemToUpdate = Context.ScheduleAdboards
                              .Where(i => i.ScheduleId == scheduleadboard.ScheduleId && i.AdboardId == scheduleadboard.AdboardId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(scheduleadboard);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterScheduleAdboardUpdated(scheduleadboard);

            return scheduleadboard;
        }

        partial void OnScheduleAdboardDeleted(DOOH.Server.Models.DOOHDB.ScheduleAdboard item);
        partial void OnAfterScheduleAdboardDeleted(DOOH.Server.Models.DOOHDB.ScheduleAdboard item);

        public async Task<DOOH.Server.Models.DOOHDB.ScheduleAdboard> DeleteScheduleAdboard(int scheduleid, int adboardid)
        {
            var itemToDelete = Context.ScheduleAdboards
                              .Where(i => i.ScheduleId == scheduleid && i.AdboardId == adboardid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnScheduleAdboardDeleted(itemToDelete);


            Context.ScheduleAdboards.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterScheduleAdboardDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTaxesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/taxes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/taxes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTaxesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/taxes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/taxes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTaxesRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Tax> items);

        public async Task<IQueryable<DOOH.Server.Models.DOOHDB.Tax>> GetTaxes(Query query = null)
        {
            var items = Context.Taxes.AsQueryable();

            items = items.Include(i => i.Tax1);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnTaxesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTaxGet(DOOH.Server.Models.DOOHDB.Tax item);
        partial void OnGetTaxByTaxId(ref IQueryable<DOOH.Server.Models.DOOHDB.Tax> items);


        public async Task<DOOH.Server.Models.DOOHDB.Tax> GetTaxByTaxId(int taxid)
        {
            var items = Context.Taxes
                              .AsNoTracking()
                              .Where(i => i.TaxId == taxid);

            items = items.Include(i => i.Tax1);
 
            OnGetTaxByTaxId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnTaxGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTaxCreated(DOOH.Server.Models.DOOHDB.Tax item);
        partial void OnAfterTaxCreated(DOOH.Server.Models.DOOHDB.Tax item);

        public async Task<DOOH.Server.Models.DOOHDB.Tax> CreateTax(DOOH.Server.Models.DOOHDB.Tax tax)
        {
            OnTaxCreated(tax);

            var existingItem = Context.Taxes
                              .Where(i => i.TaxId == tax.TaxId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Taxes.Add(tax);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(tax).State = EntityState.Detached;
                throw;
            }

            OnAfterTaxCreated(tax);

            return tax;
        }

        public async Task<DOOH.Server.Models.DOOHDB.Tax> CancelTaxChanges(DOOH.Server.Models.DOOHDB.Tax item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTaxUpdated(DOOH.Server.Models.DOOHDB.Tax item);
        partial void OnAfterTaxUpdated(DOOH.Server.Models.DOOHDB.Tax item);

        public async Task<DOOH.Server.Models.DOOHDB.Tax> UpdateTax(int taxid, DOOH.Server.Models.DOOHDB.Tax tax)
        {
            OnTaxUpdated(tax);

            var itemToUpdate = Context.Taxes
                              .Where(i => i.TaxId == tax.TaxId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(tax);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTaxUpdated(tax);

            return tax;
        }

        partial void OnTaxDeleted(DOOH.Server.Models.DOOHDB.Tax item);
        partial void OnAfterTaxDeleted(DOOH.Server.Models.DOOHDB.Tax item);

        public async Task<DOOH.Server.Models.DOOHDB.Tax> DeleteTax(int taxid)
        {
            var itemToDelete = Context.Taxes
                              .Where(i => i.TaxId == taxid)
                              .Include(i => i.Taxes1)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTaxDeleted(itemToDelete);


            Context.Taxes.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTaxDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportUploadsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/uploads/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/uploads/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportUploadsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/uploads/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/uploads/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnUploadsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.Upload> items);

        public async Task<IQueryable<DOOH.Server.Models.DOOHDB.Upload>> GetUploads(Query query = null)
        {
            var items = Context.Uploads.AsQueryable();

            items = items.Include(i => i.UserInformation);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnUploadsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnUploadGet(DOOH.Server.Models.DOOHDB.Upload item);
        partial void OnGetUploadByKey(ref IQueryable<DOOH.Server.Models.DOOHDB.Upload> items);


        public async Task<DOOH.Server.Models.DOOHDB.Upload> GetUploadByKey(string key)
        {
            var items = Context.Uploads
                              .AsNoTracking()
                              .Where(i => i.Key == key);

            items = items.Include(i => i.UserInformation);
 
            OnGetUploadByKey(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnUploadGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnUploadCreated(DOOH.Server.Models.DOOHDB.Upload item);
        partial void OnAfterUploadCreated(DOOH.Server.Models.DOOHDB.Upload item);

        public async Task<DOOH.Server.Models.DOOHDB.Upload> CreateUpload(DOOH.Server.Models.DOOHDB.Upload upload)
        {
            OnUploadCreated(upload);

            var existingItem = Context.Uploads
                              .Where(i => i.Key == upload.Key)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Uploads.Add(upload);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(upload).State = EntityState.Detached;
                throw;
            }

            OnAfterUploadCreated(upload);

            return upload;
        }

        public async Task<DOOH.Server.Models.DOOHDB.Upload> CancelUploadChanges(DOOH.Server.Models.DOOHDB.Upload item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnUploadUpdated(DOOH.Server.Models.DOOHDB.Upload item);
        partial void OnAfterUploadUpdated(DOOH.Server.Models.DOOHDB.Upload item);

        public async Task<DOOH.Server.Models.DOOHDB.Upload> UpdateUpload(string key, DOOH.Server.Models.DOOHDB.Upload upload)
        {
            OnUploadUpdated(upload);

            var itemToUpdate = Context.Uploads
                              .Where(i => i.Key == upload.Key)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(upload);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterUploadUpdated(upload);

            return upload;
        }

        partial void OnUploadDeleted(DOOH.Server.Models.DOOHDB.Upload item);
        partial void OnAfterUploadDeleted(DOOH.Server.Models.DOOHDB.Upload item);

        public async Task<DOOH.Server.Models.DOOHDB.Upload> DeleteUpload(string key)
        {
            var itemToDelete = Context.Uploads
                              .Where(i => i.Key == key)
                              .Include(i => i.Advertisements)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnUploadDeleted(itemToDelete);


            Context.Uploads.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterUploadDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportUserInformationsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/userinformations/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/userinformations/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportUserInformationsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/doohdb/userinformations/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/doohdb/userinformations/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnUserInformationsRead(ref IQueryable<DOOH.Server.Models.DOOHDB.UserInformation> items);

        public async Task<IQueryable<DOOH.Server.Models.DOOHDB.UserInformation>> GetUserInformations(Query query = null)
        {
            var items = Context.UserInformations.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnUserInformationsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnUserInformationGet(DOOH.Server.Models.DOOHDB.UserInformation item);
        partial void OnGetUserInformationByUserId(ref IQueryable<DOOH.Server.Models.DOOHDB.UserInformation> items);


        public async Task<DOOH.Server.Models.DOOHDB.UserInformation> GetUserInformationByUserId(string userid)
        {
            var items = Context.UserInformations
                              .AsNoTracking()
                              .Where(i => i.UserId == userid);

 
            OnGetUserInformationByUserId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnUserInformationGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnUserInformationCreated(DOOH.Server.Models.DOOHDB.UserInformation item);
        partial void OnAfterUserInformationCreated(DOOH.Server.Models.DOOHDB.UserInformation item);

        public async Task<DOOH.Server.Models.DOOHDB.UserInformation> CreateUserInformation(DOOH.Server.Models.DOOHDB.UserInformation userinformation)
        {
            OnUserInformationCreated(userinformation);

            var existingItem = Context.UserInformations
                              .Where(i => i.UserId == userinformation.UserId)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.UserInformations.Add(userinformation);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(userinformation).State = EntityState.Detached;
                throw;
            }

            OnAfterUserInformationCreated(userinformation);

            return userinformation;
        }

        public async Task<DOOH.Server.Models.DOOHDB.UserInformation> CancelUserInformationChanges(DOOH.Server.Models.DOOHDB.UserInformation item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnUserInformationUpdated(DOOH.Server.Models.DOOHDB.UserInformation item);
        partial void OnAfterUserInformationUpdated(DOOH.Server.Models.DOOHDB.UserInformation item);

        public async Task<DOOH.Server.Models.DOOHDB.UserInformation> UpdateUserInformation(string userid, DOOH.Server.Models.DOOHDB.UserInformation userinformation)
        {
            OnUserInformationUpdated(userinformation);

            var itemToUpdate = Context.UserInformations
                              .Where(i => i.UserId == userinformation.UserId)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(userinformation);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterUserInformationUpdated(userinformation);

            return userinformation;
        }

        partial void OnUserInformationDeleted(DOOH.Server.Models.DOOHDB.UserInformation item);
        partial void OnAfterUserInformationDeleted(DOOH.Server.Models.DOOHDB.UserInformation item);

        public async Task<DOOH.Server.Models.DOOHDB.UserInformation> DeleteUserInformation(string userid)
        {
            var itemToDelete = Context.UserInformations
                              .Where(i => i.UserId == userid)
                              .Include(i => i.Uploads)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnUserInformationDeleted(itemToDelete);


            Context.UserInformations.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterUserInformationDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}