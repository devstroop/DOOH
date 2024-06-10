using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using DOOH.Server.Data;

namespace DOOH.Server.Controllers
{
    public partial class ExportDOOHDBController : ExportController
    {
        private readonly DOOHDBContext context;
        private readonly DOOHDBService service;

        public ExportDOOHDBController(DOOHDBContext context, DOOHDBService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/DOOHDB/adboards/csv")]
        [HttpGet("/export/DOOHDB/adboards/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdboardsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAdboards(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/adboards/excel")]
        [HttpGet("/export/DOOHDB/adboards/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdboardsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAdboards(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/adboardimages/csv")]
        [HttpGet("/export/DOOHDB/adboardimages/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdboardImagesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAdboardImages(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/adboardimages/excel")]
        [HttpGet("/export/DOOHDB/adboardimages/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdboardImagesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAdboardImages(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/adboardnetworks/csv")]
        [HttpGet("/export/DOOHDB/adboardnetworks/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdboardNetworksToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAdboardNetworks(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/adboardnetworks/excel")]
        [HttpGet("/export/DOOHDB/adboardnetworks/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdboardNetworksToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAdboardNetworks(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/adboardwifis/csv")]
        [HttpGet("/export/DOOHDB/adboardwifis/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdboardWifisToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAdboardWifis(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/adboardwifis/excel")]
        [HttpGet("/export/DOOHDB/adboardwifis/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdboardWifisToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAdboardWifis(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/advertisements/csv")]
        [HttpGet("/export/DOOHDB/advertisements/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdvertisementsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAdvertisements(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/advertisements/excel")]
        [HttpGet("/export/DOOHDB/advertisements/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdvertisementsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAdvertisements(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/analytics/csv")]
        [HttpGet("/export/DOOHDB/analytics/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAnalyticsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAnalytics(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/analytics/excel")]
        [HttpGet("/export/DOOHDB/analytics/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAnalyticsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAnalytics(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/attachments/csv")]
        [HttpGet("/export/DOOHDB/attachments/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAttachmentsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAttachments(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/attachments/excel")]
        [HttpGet("/export/DOOHDB/attachments/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAttachmentsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAttachments(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/billings/csv")]
        [HttpGet("/export/DOOHDB/billings/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBillingsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetBillings(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/billings/excel")]
        [HttpGet("/export/DOOHDB/billings/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBillingsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetBillings(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/brands/csv")]
        [HttpGet("/export/DOOHDB/brands/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBrandsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetBrands(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/brands/excel")]
        [HttpGet("/export/DOOHDB/brands/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBrandsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetBrands(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/campaigns/csv")]
        [HttpGet("/export/DOOHDB/campaigns/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCampaignsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCampaigns(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/campaigns/excel")]
        [HttpGet("/export/DOOHDB/campaigns/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCampaignsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCampaigns(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/campaignadboards/csv")]
        [HttpGet("/export/DOOHDB/campaignadboards/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCampaignAdboardsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCampaignAdboards(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/campaignadboards/excel")]
        [HttpGet("/export/DOOHDB/campaignadboards/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCampaignAdboardsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCampaignAdboards(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/categories/csv")]
        [HttpGet("/export/DOOHDB/categories/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCategoriesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCategories(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/categories/excel")]
        [HttpGet("/export/DOOHDB/categories/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCategoriesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCategories(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/cities/csv")]
        [HttpGet("/export/DOOHDB/cities/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCitiesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCities(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/cities/excel")]
        [HttpGet("/export/DOOHDB/cities/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCitiesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCities(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/companies/csv")]
        [HttpGet("/export/DOOHDB/companies/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCompaniesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCompanies(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/companies/excel")]
        [HttpGet("/export/DOOHDB/companies/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCompaniesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCompanies(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/countries/csv")]
        [HttpGet("/export/DOOHDB/countries/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCountriesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCountries(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/countries/excel")]
        [HttpGet("/export/DOOHDB/countries/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCountriesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCountries(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/displays/csv")]
        [HttpGet("/export/DOOHDB/displays/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportDisplaysToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetDisplays(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/displays/excel")]
        [HttpGet("/export/DOOHDB/displays/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportDisplaysToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetDisplays(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/earnings/csv")]
        [HttpGet("/export/DOOHDB/earnings/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportEarningsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetEarnings(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/earnings/excel")]
        [HttpGet("/export/DOOHDB/earnings/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportEarningsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetEarnings(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/faqs/csv")]
        [HttpGet("/export/DOOHDB/faqs/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportFaqsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetFaqs(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/faqs/excel")]
        [HttpGet("/export/DOOHDB/faqs/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportFaqsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetFaqs(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/motherboards/csv")]
        [HttpGet("/export/DOOHDB/motherboards/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMotherboardsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetMotherboards(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/motherboards/excel")]
        [HttpGet("/export/DOOHDB/motherboards/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMotherboardsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetMotherboards(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/pages/csv")]
        [HttpGet("/export/DOOHDB/pages/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportPagesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetPages(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/pages/excel")]
        [HttpGet("/export/DOOHDB/pages/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportPagesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetPages(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/providers/csv")]
        [HttpGet("/export/DOOHDB/providers/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportProvidersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetProviders(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/providers/excel")]
        [HttpGet("/export/DOOHDB/providers/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportProvidersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetProviders(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/states/csv")]
        [HttpGet("/export/DOOHDB/states/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStatesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStates(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/states/excel")]
        [HttpGet("/export/DOOHDB/states/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStatesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStates(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/statuses/csv")]
        [HttpGet("/export/DOOHDB/statuses/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStatusesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStatuses(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/statuses/excel")]
        [HttpGet("/export/DOOHDB/statuses/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStatusesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStatuses(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/taxes/csv")]
        [HttpGet("/export/DOOHDB/taxes/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTaxesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTaxes(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/taxes/excel")]
        [HttpGet("/export/DOOHDB/taxes/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTaxesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTaxes(), Request.Query, false), fileName);
        }
    }
}
