using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace AUserBoligForeningMVC.Models
{
    public class Lejer
    {
        public int Id { get; set; }
        public string Fornavn { get; set; }
        public string Efternavn { get; set; }
        public string Adresse { get; set; }
        public int PostNr { get; set; }
        public string Email { get; set; }
        public string TlfNr { get; set; }
        public string By { get; set; }
        public int Alder { get; set; }
       
    }
}
