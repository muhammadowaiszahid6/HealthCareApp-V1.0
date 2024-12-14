using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthcareApp.Models.healthcaredb
{
    [Table("withdraw")]
    public partial class Withdraw
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public DateTime? DOB { get; set; }

        public string NPI { get; set; }

        public string SSN { get; set; }

        public string Ethnicity { get; set; }

        public string CAQHNumber { get; set; }

        public DateTime? WithdrawDate { get; set; }

        public string WithdrawReason { get; set; }

    }
}