using Azure.Core;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string Nit { get; set; }
        public string Description { get; set; }
        public string Observation { get; set; }
        public string UrlDelivery { get; set; }
        public int? IdUsuario { get; set; }

        public ICollection<Request> Requests { get; set; }

        public List<ApplicationUser> ToList()
        {
            return new();
        }
    }
}
