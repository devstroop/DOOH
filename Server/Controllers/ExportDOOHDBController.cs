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

        [HttpGet("/export/DOOHDB/adboardstatuses/csv")]
        [HttpGet("/export/DOOHDB/adboardstatuses/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdboardStatusesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAdboardStatuses(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/adboardstatuses/excel")]
        [HttpGet("/export/DOOHDB/adboardstatuses/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAdboardStatusesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAdboardStatuses(), Request.Query, false), fileName);
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

        [HttpGet("/export/DOOHDB/campaigncriteria/csv")]
        [HttpGet("/export/DOOHDB/campaigncriteria/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCampaignCriteriaToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCampaignCriteria(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/campaigncriteria/excel")]
        [HttpGet("/export/DOOHDB/campaigncriteria/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCampaignCriteriaToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCampaignCriteria(), Request.Query, false), fileName);
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

        [HttpGet("/export/DOOHDB/schedules/csv")]
        [HttpGet("/export/DOOHDB/schedules/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSchedulesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetSchedules(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/schedules/excel")]
        [HttpGet("/export/DOOHDB/schedules/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSchedulesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetSchedules(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/scheduleadboards/csv")]
        [HttpGet("/export/DOOHDB/scheduleadboards/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportScheduleAdboardsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetScheduleAdboards(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/scheduleadboards/excel")]
        [HttpGet("/export/DOOHDB/scheduleadboards/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportScheduleAdboardsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetScheduleAdboards(), Request.Query, false), fileName);
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

        [HttpGet("/export/DOOHDB/uploads/csv")]
        [HttpGet("/export/DOOHDB/uploads/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportUploadsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetUploads(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/uploads/excel")]
        [HttpGet("/export/DOOHDB/uploads/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportUploadsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetUploads(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/userinformations/csv")]
        [HttpGet("/export/DOOHDB/userinformations/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportUserInformationsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetUserInformations(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/userinformations/excel")]
        [HttpGet("/export/DOOHDB/userinformations/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportUserInformationsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetUserInformations(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/scheduleadvertisements/csv")]
        [HttpGet("/export/DOOHDB/scheduleadvertisements/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportScheduleAdvertisementsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetScheduleAdvertisements(), Request.Query, false), fileName);
        }

        [HttpGet("/export/DOOHDB/scheduleadvertisements/excel")]
        [HttpGet("/export/DOOHDB/scheduleadvertisements/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportScheduleAdvertisementsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetScheduleAdvertisements(), Request.Query, false), fileName);
        }
    }
}
