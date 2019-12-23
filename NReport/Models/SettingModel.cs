using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace NReport
{
    public class SettingModel
    {
        [Display(Name = "Server Name:")]
        [Required(ErrorMessage = "Server Name is required")]
        public string ServerName { get; set; }

        [Display(Name = "Data Base:")]
        [Required(ErrorMessage = "Data Base is required")]
        public string DatabaseName { get; set; }

        [Display(Name = "User Name:")]
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }

        [Display(Name = "Password:")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

    }
}