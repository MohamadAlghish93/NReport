using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NReport.Models
{
    public class SettingModel
    {
        ///<summary>
        /// Gets or sets ServerName.
        ///</summary>
        public string ServerName { get; set; }

        ///<summary>
        /// Gets or sets DatabaseName.
        ///</summary>
        public string DatabaseName { get; set; }

        ///<summary>
        /// Gets or sets UserName.
        ///</summary>
        public string UserName { get; set; }

        ///<summary>
        /// Gets or sets Password.
        ///</summary>
        public string Password { get; set; }

        ///<summary>
        /// Gets or sets Password.
        ///</summary>
        public string tarasolURL { get; set; }
    }
}