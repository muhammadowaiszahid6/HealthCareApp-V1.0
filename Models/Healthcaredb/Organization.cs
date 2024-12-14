using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthcareApp.Models.healthcaredb
{
    [Table("organization")]
    public partial class Organization
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string OrganizationName { get; set; }

        public string NPI { get; set; }

        public string AdminName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Extension { get; set; }

        public string Fax { get; set; }

        public bool? DelegatedCredentialing { get; set; }

        public bool? RequestDelegatedCredentialing { get; set; }

        public bool? DoNotContact { get; set; }

        public DateTime? NextAuditDate { get; set; }

        public DateTime? TerminationDate { get; set; }

        public string TerminationReason { get; set; }

    }
}