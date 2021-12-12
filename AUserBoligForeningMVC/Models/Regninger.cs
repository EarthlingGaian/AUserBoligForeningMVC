using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUserBoligForeningMVC.Models
{
    public class Regninger
    {
        public int Id { get; set; }
        public string Regning { get; set; }
        public string BeboerMail { get; set; }
        public string Date { get; set; }
        public string Calendar { get; set; }
    }
}
