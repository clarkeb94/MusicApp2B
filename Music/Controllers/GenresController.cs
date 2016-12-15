using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Music.Models;

namespace Music.Controllers
{
    public class GenresController : Controller
    {
        private MusicContext db = new MusicContext();
        // GET: Genre
        public ActionResult Index()
        {
            var something = db.Genres.ToList();
            return View(something);
        }
        // GET: Genre/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Genres/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GenreID, Name")] Genre genre)
        {
            if (db.Genres.Any(ac => ac.Name.Equals(genre.Name)))
            {
                ModelState.AddModelError("GenreError", "Genre already exists" );
               
                
                return View("Create");


            }
            else if (ModelState.IsValid)
            {
                ViewBag.NameError = false;
                db.Genres.Add(genre);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(genre);
        }











        [HttpGet]
        public ActionResult BrowseByGenre(int? id)
        {


           

            if (id != null)
            {

                var genre = db.Genres.Where(a => a.GenreID == id);
                ViewBag.header = genre.First().Name;
                var albums = db.Albums
                    .Include(a => a.Artist)
                    .Include(a => a.Genre)
                    .Where(a => a.GenreID == id);
                return View("BrowseByGenre", albums.ToList());
            }
            else
            {
                return View();
            }
        }

        
        







    }
    }

