using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AUserBoligForeningMVC.Models
{
    public class Lejer
    {
        public int Id { get; set; }

        [ConcurrencyCheck]
        public string Fornavn { get; set; }
        [ConcurrencyCheck]
        public string Efternavn { get; set; }
        public string Adresse { get; set; }
        public int PostNr { get; set; }
        public string Email { get; set; }
        [ConcurrencyCheck]
        public string TlfNr { get; set; }
        public string By { get; set; }
        [ConcurrencyCheck]
        public int Alder { get; set; }
       
    }
}
