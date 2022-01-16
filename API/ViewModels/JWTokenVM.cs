using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.ViewModels
{
    public class JWTokenVM
    {
        public string status { get; set; }
        public string idToken { get; set; }
        public string message { get; set; }
    }
}
