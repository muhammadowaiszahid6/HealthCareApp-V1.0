using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace HealthcareApp.Pages
{
    public partial class Specialities
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        public healthcaredbService healthcaredbService { get; set; }

        protected IEnumerable<HealthcareApp.Models.healthcaredb.Speciality> specialities;

        protected RadzenDataGrid<HealthcareApp.Models.healthcaredb.Speciality> grid0;
        protected override async Task OnInitializedAsync()
        {
            specialities = await healthcaredbService.GetSpecialities();
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddSpeciality>("Add Speciality", null);
            await grid0.Reload();
        }

        protected async Task EditRow(HealthcareApp.Models.healthcaredb.Speciality args)
        {
            await DialogService.OpenAsync<EditSpeciality>("Edit Speciality", new Dictionary<string, object> { {"Id", args.Id} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, HealthcareApp.Models.healthcaredb.Speciality speciality)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await healthcaredbService.DeleteSpeciality(speciality.Id);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                { 
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error", 
                    Detail = $"Unable to delete Speciality" 
                });
            }
        }
    }
}