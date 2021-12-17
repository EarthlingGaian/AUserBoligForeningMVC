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
       
        [Required(ErrorMessage = "Du skal have et fornavn")]
        [ConcurrencyCheck]
        public string Fornavn { get; set; }

        [Required(ErrorMessage = "Du skal have et efternavn")]
        [ConcurrencyCheck]
        public string Efternavn { get; set; }
        public string Adresse { get; set; }
        public int PostNr { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "Du skal have et telefon nummer")]
        [ConcurrencyCheck]
        public string TlfNr { get; set; }
        public string By { get; set; }

        [Required(ErrorMessage = "Du skal have en alder")]
        [ConcurrencyCheck]
        public int Alder { get; set; }
       
    }
}
