using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using NewFaceBook.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NewFaceBook.ViewModels
{
    public class UserCreateViewModel 
    {

        [Required]
        [MaxLength(30, ErrorMessage = "Name cannot exceed 30 characters")]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage = "This is not a real email")]
        [Display(Name = "Personal Email")]
        public string Email { get; set; }

        [Required]
        public Dept? Location { get; set; }

        public IFormFile Photo { get; set; }
    }
}
