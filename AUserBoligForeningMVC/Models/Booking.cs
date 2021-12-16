using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AUserBoligForeningMVC.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Title { get; set; }
        public string Calendar { get; set; }
        public string CurrentUserMail { get; set; }
    }
}
