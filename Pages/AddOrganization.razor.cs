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
    public partial class AddOrganization
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

        protected override async Task OnInitializedAsync()
        {
            organization = new HealthcareApp.Models.healthcaredb.Organization();
        }
        protected bool errorVisible;
        protected HealthcareApp.Models.healthcaredb.Organization organization;

        protected async Task FormSubmit()
        {
            try
            {
                await healthcaredbService.CreateOrganization(organization);
                DialogService.Close(organization);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}