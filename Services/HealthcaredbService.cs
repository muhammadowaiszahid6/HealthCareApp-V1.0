using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

using HealthcareApp.Data;

namespace HealthcareApp
{
    public partial class healthcaredbService
    {
        healthcaredbContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly healthcaredbContext context;
        private readonly NavigationManager navigationManager;

        public healthcaredbService(healthcaredbContext context, NavigationManager navigationManager)
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


        public async Task ExportOrganizationsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/healthcaredb/organizations/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/healthcaredb/organizations/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportOrganizationsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/healthcaredb/organizations/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/healthcaredb/organizations/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnOrganizationsRead(ref IQueryable<HealthcareApp.Models.healthcaredb.Organization> items);

        public async Task<IQueryable<HealthcareApp.Models.healthcaredb.Organization>> GetOrganizations(Query query = null)
        {
            var items = Context.Organizations.AsQueryable();


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

            OnOrganizationsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnOrganizationGet(HealthcareApp.Models.healthcaredb.Organization item);
        partial void OnGetOrganizationById(ref IQueryable<HealthcareApp.Models.healthcaredb.Organization> items);


        public async Task<HealthcareApp.Models.healthcaredb.Organization> GetOrganizationById(int id)
        {
            var items = Context.Organizations
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetOrganizationById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnOrganizationGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnOrganizationCreated(HealthcareApp.Models.healthcaredb.Organization item);
        partial void OnAfterOrganizationCreated(HealthcareApp.Models.healthcaredb.Organization item);

        public async Task<HealthcareApp.Models.healthcaredb.Organization> CreateOrganization(HealthcareApp.Models.healthcaredb.Organization organization)
        {
            OnOrganizationCreated(organization);

            var existingItem = Context.Organizations
                              .Where(i => i.Id == organization.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Organizations.Add(organization);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(organization).State = EntityState.Detached;
                throw;
            }

            OnAfterOrganizationCreated(organization);

            return organization;
        }

        public async Task<HealthcareApp.Models.healthcaredb.Organization> CancelOrganizationChanges(HealthcareApp.Models.healthcaredb.Organization item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnOrganizationUpdated(HealthcareApp.Models.healthcaredb.Organization item);
        partial void OnAfterOrganizationUpdated(HealthcareApp.Models.healthcaredb.Organization item);

        public async Task<HealthcareApp.Models.healthcaredb.Organization> UpdateOrganization(int id, HealthcareApp.Models.healthcaredb.Organization organization)
        {
            OnOrganizationUpdated(organization);

            var itemToUpdate = Context.Organizations
                              .Where(i => i.Id == organization.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(organization);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterOrganizationUpdated(organization);

            return organization;
        }

        partial void OnOrganizationDeleted(HealthcareApp.Models.healthcaredb.Organization item);
        partial void OnAfterOrganizationDeleted(HealthcareApp.Models.healthcaredb.Organization item);

        public async Task<HealthcareApp.Models.healthcaredb.Organization> DeleteOrganization(int id)
        {
            var itemToDelete = Context.Organizations
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnOrganizationDeleted(itemToDelete);


            Context.Organizations.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterOrganizationDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportSpecialitiesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/healthcaredb/specialities/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/healthcaredb/specialities/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSpecialitiesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/healthcaredb/specialities/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/healthcaredb/specialities/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSpecialitiesRead(ref IQueryable<HealthcareApp.Models.healthcaredb.Speciality> items);

        public async Task<IQueryable<HealthcareApp.Models.healthcaredb.Speciality>> GetSpecialities(Query query = null)
        {
            var items = Context.Specialities.AsQueryable();


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

            OnSpecialitiesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnSpecialityGet(HealthcareApp.Models.healthcaredb.Speciality item);
        partial void OnGetSpecialityById(ref IQueryable<HealthcareApp.Models.healthcaredb.Speciality> items);


        public async Task<HealthcareApp.Models.healthcaredb.Speciality> GetSpecialityById(int id)
        {
            var items = Context.Specialities
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetSpecialityById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnSpecialityGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnSpecialityCreated(HealthcareApp.Models.healthcaredb.Speciality item);
        partial void OnAfterSpecialityCreated(HealthcareApp.Models.healthcaredb.Speciality item);

        public async Task<HealthcareApp.Models.healthcaredb.Speciality> CreateSpeciality(HealthcareApp.Models.healthcaredb.Speciality speciality)
        {
            OnSpecialityCreated(speciality);

            var existingItem = Context.Specialities
                              .Where(i => i.Id == speciality.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Specialities.Add(speciality);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(speciality).State = EntityState.Detached;
                throw;
            }

            OnAfterSpecialityCreated(speciality);

            return speciality;
        }

        public async Task<HealthcareApp.Models.healthcaredb.Speciality> CancelSpecialityChanges(HealthcareApp.Models.healthcaredb.Speciality item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnSpecialityUpdated(HealthcareApp.Models.healthcaredb.Speciality item);
        partial void OnAfterSpecialityUpdated(HealthcareApp.Models.healthcaredb.Speciality item);

        public async Task<HealthcareApp.Models.healthcaredb.Speciality> UpdateSpeciality(int id, HealthcareApp.Models.healthcaredb.Speciality speciality)
        {
            OnSpecialityUpdated(speciality);

            var itemToUpdate = Context.Specialities
                              .Where(i => i.Id == speciality.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(speciality);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterSpecialityUpdated(speciality);

            return speciality;
        }

        partial void OnSpecialityDeleted(HealthcareApp.Models.healthcaredb.Speciality item);
        partial void OnAfterSpecialityDeleted(HealthcareApp.Models.healthcaredb.Speciality item);

        public async Task<HealthcareApp.Models.healthcaredb.Speciality> DeleteSpeciality(int id)
        {
            var itemToDelete = Context.Specialities
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnSpecialityDeleted(itemToDelete);


            Context.Specialities.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterSpecialityDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportWithdrawsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/healthcaredb/withdraws/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/healthcaredb/withdraws/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportWithdrawsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/healthcaredb/withdraws/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/healthcaredb/withdraws/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnWithdrawsRead(ref IQueryable<HealthcareApp.Models.healthcaredb.Withdraw> items);

        public async Task<IQueryable<HealthcareApp.Models.healthcaredb.Withdraw>> GetWithdraws(Query query = null)
        {
            var items = Context.Withdraws.AsQueryable();


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

            OnWithdrawsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnWithdrawGet(HealthcareApp.Models.healthcaredb.Withdraw item);
        partial void OnGetWithdrawById(ref IQueryable<HealthcareApp.Models.healthcaredb.Withdraw> items);


        public async Task<HealthcareApp.Models.healthcaredb.Withdraw> GetWithdrawById(int id)
        {
            var items = Context.Withdraws
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetWithdrawById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnWithdrawGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnWithdrawCreated(HealthcareApp.Models.healthcaredb.Withdraw item);
        partial void OnAfterWithdrawCreated(HealthcareApp.Models.healthcaredb.Withdraw item);

        public async Task<HealthcareApp.Models.healthcaredb.Withdraw> CreateWithdraw(HealthcareApp.Models.healthcaredb.Withdraw withdraw)
        {
            OnWithdrawCreated(withdraw);

            var existingItem = Context.Withdraws
                              .Where(i => i.Id == withdraw.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Withdraws.Add(withdraw);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(withdraw).State = EntityState.Detached;
                throw;
            }

            OnAfterWithdrawCreated(withdraw);

            return withdraw;
        }

        public async Task<HealthcareApp.Models.healthcaredb.Withdraw> CancelWithdrawChanges(HealthcareApp.Models.healthcaredb.Withdraw item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnWithdrawUpdated(HealthcareApp.Models.healthcaredb.Withdraw item);
        partial void OnAfterWithdrawUpdated(HealthcareApp.Models.healthcaredb.Withdraw item);

        public async Task<HealthcareApp.Models.healthcaredb.Withdraw> UpdateWithdraw(int id, HealthcareApp.Models.healthcaredb.Withdraw withdraw)
        {
            OnWithdrawUpdated(withdraw);

            var itemToUpdate = Context.Withdraws
                              .Where(i => i.Id == withdraw.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(withdraw);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterWithdrawUpdated(withdraw);

            return withdraw;
        }

        partial void OnWithdrawDeleted(HealthcareApp.Models.healthcaredb.Withdraw item);
        partial void OnAfterWithdrawDeleted(HealthcareApp.Models.healthcaredb.Withdraw item);

        public async Task<HealthcareApp.Models.healthcaredb.Withdraw> DeleteWithdraw(int id)
        {
            var itemToDelete = Context.Withdraws
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnWithdrawDeleted(itemToDelete);


            Context.Withdraws.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterWithdrawDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}