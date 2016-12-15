using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Music.Models
{
    public class MusicContext : DbContext
    {
      
    
        public MusicContext() : base("name=MusicContext")
        {
        }

        public DbSet<Music.Models.Album> Albums { get; set; }

        public DbSet<Music.Models.Artist> Artists { get; set; }

        public DbSet<Music.Models.Genre> Genres { get; set; }

        public DbSet<Music.Models.UserAccount> userAccount { get; set; }
    }
}
