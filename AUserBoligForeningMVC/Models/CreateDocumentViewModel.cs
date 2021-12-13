using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUserBoligForeningMVC.Models
{
    public class CreateDocumentViewModel
    {
        public int Id { get; set; }

        public string Navn { get; set; }

        public string Adresse { get; set; }
        public string By { get; set; }

        public int PostNr { get; set; }



        public DateTime MoveInDate { get; set; }


        public DateTime RegisteredDate { get; set; }
        public string Created { get; set; }
        public string DueDate { get; set; }

        public int Husleje { get; set; }


    }
}
