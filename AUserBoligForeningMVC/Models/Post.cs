using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AUserBoligForeningMVC.Models
{
    public class Post
    {
        public int Id { get; set; }
        
        public string AuthorId { get; set; }
        
        public string AuthorName { get; set; }

        public DateTime Created { get; set; }

        [Required(ErrorMessage = "Skriv noget, før du kan poste det")]
        [MaxLength(150, ErrorMessage = "Maximum length for the post is 150 characters.")]
        public string PostContent { get; set; }

        public bool isAdmin { get; set; }

    }
}
