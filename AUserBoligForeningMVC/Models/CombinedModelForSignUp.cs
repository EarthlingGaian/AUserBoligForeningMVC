using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AUserBoligForeningMVC.Models
{
    public class CombinedModelForSignUp
    {
        public Lejer Lejer { get; set; }
        public SelectList Lejers { get; set; }

        public SelectList Lejligheders { get; set; }


    }
}
