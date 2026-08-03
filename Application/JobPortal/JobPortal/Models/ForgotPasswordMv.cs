using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobPortal.Models
{
    public class ForgotPasswordMv
    {
        [DataType(DataType.EmailAddress)]

        public string EmailAddress { get; set; }
    }
}