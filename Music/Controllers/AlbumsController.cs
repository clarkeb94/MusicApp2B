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
    public class AlbumsController : Controller
    {
        private MusicContext db = new MusicContext();


        // GET: Albums
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.AlbumNameSortParam = string.IsNullOrEmpty(sortOrder) ? "album_name" : "";
            ViewBag.ArtistNameSortParam = sortOrder == "artist_name" ? "name_artist" : "artist_name";
            ViewBag.GenreNameSortParam = sortOrder == "genre_name" ? "name_genre" : "genre_name";
            ViewBag.PriceSortParam = sortOrder == "price_amnt" ? "amnt_price" : "price_amnt";
            ViewBag.LikesSortParam = sortOrder == "likes_amnt" ? "amnt_likes" : "likes_amnt";
            var albums = from a in db.Albums.Include(a => a.Artist).Include(a => a.Genre) select a;
            if (!string.IsNullOrEmpty(searchString))
            {
                albums = albums.Where(a => a.Title.ToUpper().Contains(searchString.ToUpper()) || a.Genre.Name.ToUpper().Contains(searchString.ToUpper()) || a.Artist.Name.ToUpper().Contains(searchString.ToUpper()));

            }
            switch (sortOrder)
            {
                case "album_name":
                    albums = albums.OrderByDescending(a => a.Title);
                    break;
                case "artist_name":
                    albums = albums.OrderBy(a => a.Artist.Name);
                    break;
                case "name_artist":
                    albums = albums.OrderByDescending(a => a.Artist.Name);
                    break;
                case "genre_name":
                    albums = albums.OrderBy(a => a.Genre.Name);
                    break;
                case "name_genre":
                    albums = albums.OrderByDescending(a => a.Genre.Name);
                    break;
                case "price_amnt":
                    albums = albums.OrderBy(a => a.Price);
                    break;
                case "amnt_price":
                    albums = albums.OrderByDescending(a => a.Price);
                    break;
                case "likes_amnt":
                    albums = albums.OrderBy(a => a.Likes);
                    break;
                case "amnt_likes":
                    albums = albums.OrderByDescending(a => a.Likes);
                    break;
                default:
                    albums = albums.OrderBy(a => a.Title);
                    break;
            }


            return View(albums.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                Album album = db.Albums.Include(a => a.Artist).Include(a => a.Genre).Where(a => a.AlbumID == id).Single();

                if (album == null)
                {
                    return HttpNotFound();
                }
                return View(album);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Likes(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var albums = db.Albums.SingleOrDefault(a => a.AlbumID == id);
            albums.Likes++;
            db.SaveChanges();
            return RedirectToAction("Index", db.Albums.ToList());

        }

        public ActionResult ShowSomeAlbums(int id)
        {
            var albums = db.Albums
                .Include(a => a.Artist)
                .Include(a => a.Genre)
                .Where(a => a.GenreID == id);
            return View(albums.ToList());
        }

        // GET: Albums/Details/5
  

        public ActionResult Recommendations(int? id)
        {
            var albums = db.Albums
                .Include(a => a.Artist)
                .Where(a => a.GenreID == id);
            return View(albums.ToList());
        }

        // GET: Albums/Create
        public ActionResult Create()
        {
            ViewBag.ArtistID = new SelectList(db.Artists, "ArtistID", "Name");
            ViewBag.GenreID = new SelectList(db.Genres.OrderByDescending(g => g.Name), "GenreID", "Name");
            return View();
        }

        // POST: Albums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Album album)
        {
            var artistCheck = db.Artists.Any(ac => ac.Name.Equals(album.Artist.Name));
            var genreCheck = db.Genres.Any(ac => ac.Name.Equals(album.Genre.Name));
           

            if (artistCheck == true && album.ArtistID == 0)
            {
               
                ModelState.AddModelError("ArtistError", "Artist already exisits");
            }


            if (genreCheck == true && album.GenreID == 0)
            {
               
                ModelState.AddModelError("GenreError", "Genre already exisits");
            }

            if (album.GenreID != 0)
            {

                album.Genre = db.Genres.FirstOrDefault(m => m.GenreID == album.GenreID);
            }

            if (album.ArtistID != 0)
            {

                album.Artist = db.Artists.FirstOrDefault(m => m.ArtistID == album.ArtistID);
            }

            if (album.GenreID == 0)
            {
                ModelState.Remove("GenreID");
            }

            if (album.ArtistID == 0)
            {
                ModelState.Remove("ArtistID");
            }


            if (ModelState.IsValid)
            {
                db.Albums.Add(album);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArtistID = new SelectList(db.Artists, "ArtistID", "Name", album.ArtistID);
            ViewBag.GenreID = new SelectList(db.Genres, "GenreID", "Name", album.GenreID);
            return View(album);
        }

        // GET: Albums/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArtistID = new SelectList(db.Artists, "ArtistID", "Name", album.ArtistID);
            ViewBag.GenreID = new SelectList(db.Genres, "GenreID", "Name", album.GenreID);
            return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AlbumID,Title,GenreID,Price,ArtistID")] Album album)
        {
            if (ModelState.IsValid)
            {
                db.Entry(album).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArtistID = new SelectList(db.Artists, "ArtistID", "Name", album.ArtistID);
            ViewBag.GenreID = new SelectList(db.Genres, "GenreID", "Name", album.GenreID);
            return View(album);
        }

        // GET: Albums/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Album album = db.Albums.Include(a => a.Artist)
                .Include(a => a.Genre).Where(a => a.AlbumID == id).First();

            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Album album = db.Albums.Find(id);
            db.Albums.Remove(album);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
