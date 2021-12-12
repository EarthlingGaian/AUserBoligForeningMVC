using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUserBoligForeningMVC.Models
{
    public class DokumentArkiv
    {
        public int Id { get; set; }
        public string LejeKontrakt { get; set; }
        
        public string IndflytningsPapir { get; set; }

        public string BeboerMail { get; set; }


    }
}
