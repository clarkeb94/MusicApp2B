using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Music.Models
{
    public class Album
    {

        public int AlbumID { get; set; }
        [Required(ErrorMessage = "Album title is required")]
        public string Title { get; set; }

        public int GenreID { get; set; }
        // [Display(Name = "Genre")]
        public Genre Genre { get; set; }

        [Range(0.01, 100.0, ErrorMessage = "Not a correct price")]
        public decimal Price { get; set; }

        public int ArtistID { get; set; }
        //[Display(Name = "Artist")]
        public Artist Artist { get; set; }

        public int Likes { get; set; }

        public string Recommendations { get; set; }

     
       

    }
}