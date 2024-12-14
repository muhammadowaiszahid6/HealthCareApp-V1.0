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
    public partial class Withdraws
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

        protected IEnumerable<HealthcareApp.Models.healthcaredb.Withdraw> withdraws;

        protected RadzenDataGrid<HealthcareApp.Models.healthcaredb.Withdraw> grid0;
        protected override async Task OnInitializedAsync()
        {
            withdraws = await healthcaredbService.GetWithdraws();
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddWithdraw>("Add Withdraw", null);
            await grid0.Reload();
        }

        protected async Task EditRow(HealthcareApp.Models.healthcaredb.Withdraw args)
        {
            await DialogService.OpenAsync<EditWithdraw>("Edit Withdraw", new Dictionary<string, object> { {"Id", args.Id} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, HealthcareApp.Models.healthcaredb.Withdraw withdraw)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await healthcaredbService.DeleteWithdraw(withdraw.Id);

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
                    Detail = $"Unable to delete Withdraw" 
                });
            }
        }
    }
}