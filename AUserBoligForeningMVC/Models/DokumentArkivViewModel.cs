using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUserBoligForeningMVC.Models
{
    public class DokumentArkivViewModel
    {
        public int Id { get; set; }
        public IFormFile LejeKontrakt { get; set; }

        public IFormFile IndflytningsPapir { get; set; }
    }
}
