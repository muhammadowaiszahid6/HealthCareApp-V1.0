using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthcareApp.Models.healthcaredb
{
    [Table("speciality")]
    public partial class Speciality
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("Speciality")]
        public string Speciality1 { get; set; }

        public string Qualifier { get; set; }

        public int? MinAge { get; set; }

        public int? MaxAge { get; set; }

        public bool? BoardCertified { get; set; }

        public string SpecialistType { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public bool? AcceptingNewPatients { get; set; }

    }
}