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
    public class ArtistsController : Controller
    {
        private MusicContext db = new MusicContext();
        // GET: Artists
        public ActionResult Index()
        {
           
            return View(db.Artists.ToList());
        }
        // GET: Albums/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Albums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArtistID, Name, Bio")] Artist artist)
        {
            if (db.Artists.Any(ac => ac.Name.Equals(artist.Name)))
            {
                ModelState.AddModelError("ArtistError", "Artist already exists");


                return View("Create");
            }
               else if (ModelState.IsValid)
            {
                db.Artists.Add(artist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(artist);
        }



        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                Artist artist = db.Artists.Where(a => a.ArtistID == id).Single();

                if (artist == null)
                {
                    return HttpNotFound();
                }
                return View(artist);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }



        [HttpGet]
        public ActionResult BrowseByArtist(int? id)
        {




            if (id != null)
            {

                var artist = db.Artists.Where(a => a.ArtistID == id);
                ViewBag.header = artist.First().Name;
                var albums = db.Albums
                    .Include(a => a.Artist)
                    .Include(a => a.Genre)
                    .Where(a => a.ArtistID == id);
                return View("BrowseByArtist", albums.ToList());
            }
            else
            {
                return View();
            }
        }


        // GET: Albums/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArtistID = new SelectList(db.Artists, "ArtistID", "Name", "Bio", artist.ArtistID);
            
            return View(artist);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArtistID, Name, Bio")] Artist artist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(artist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArtistID = new SelectList(db.Artists, "ArtistID", "Name", "Bio", artist.ArtistID);
          
            return View(artist);
        }





    }
}





    
