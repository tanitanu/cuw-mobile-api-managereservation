using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class UpdateMemberContact : RequestBase
    {
        /// <summary>
        /// Address detail
        /// </summary>

        public Address Address { get; set; }

        /// <summary>
        /// Phone detail
        /// </summary>

        public Phone Phone { get; set; }

        /// <summary>
        /// Email detail
        /// </summary>

        public Email Email { get; set; }
    }
}
