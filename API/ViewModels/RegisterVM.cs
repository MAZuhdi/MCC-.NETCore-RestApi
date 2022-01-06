using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using API.Models;

namespace API.ViewModels
{
    public class RegisterVM
    {
        //Data validation seperti [Required] bisa juga ditambahkan di sini
        public string NIK { get; set; }
        [Required]
        [MinLength(3)]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Phone]
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        [Range(0, 500000000)] //Harus pada rentang Rp0 hingg Rp500juta
        public Gender Gender { get; set; }
        public int Salary { get; set; }
        [EmailAddress] // Harus format email
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Degree { get; set; }
        public double GPA { get; set; }
        public int UniversityId { get; set; }
    }
}
