using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using HealthcareApp.Data;

namespace HealthcareApp.Controllers
{
    public partial class ExporthealthcaredbController : ExportController
    {
        private readonly healthcaredbContext context;
        private readonly healthcaredbService service;

        public ExporthealthcaredbController(healthcaredbContext context, healthcaredbService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/healthcaredb/organizations/csv")]
        [HttpGet("/export/healthcaredb/organizations/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportOrganizationsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetOrganizations(), Request.Query), fileName);
        }

        [HttpGet("/export/healthcaredb/organizations/excel")]
        [HttpGet("/export/healthcaredb/organizations/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportOrganizationsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetOrganizations(), Request.Query), fileName);
        }

        [HttpGet("/export/healthcaredb/specialities/csv")]
        [HttpGet("/export/healthcaredb/specialities/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSpecialitiesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetSpecialities(), Request.Query), fileName);
        }

        [HttpGet("/export/healthcaredb/specialities/excel")]
        [HttpGet("/export/healthcaredb/specialities/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSpecialitiesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetSpecialities(), Request.Query), fileName);
        }

        [HttpGet("/export/healthcaredb/withdraws/csv")]
        [HttpGet("/export/healthcaredb/withdraws/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportWithdrawsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetWithdraws(), Request.Query), fileName);
        }

        [HttpGet("/export/healthcaredb/withdraws/excel")]
        [HttpGet("/export/healthcaredb/withdraws/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportWithdrawsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetWithdraws(), Request.Query), fileName);
        }
    }
}
