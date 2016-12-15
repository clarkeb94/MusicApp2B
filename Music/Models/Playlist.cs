using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Music.Models
{
    public class Playlist
    {
        public int AlbumID { get; set; }
        [Required(ErrorMessage = "Album title is required")]
        public string Title { get; set; }

        public int GenreID { get; set; }
        // [Display(Name = "Genre")]
        public Genre Genre { get; set; }

        public int ArtistID { get; set; }
        //[Display(Name = "Artist")]
        public Artist Artist { get; set; }






    }
}