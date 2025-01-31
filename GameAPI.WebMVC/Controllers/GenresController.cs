using GameAPI.ConsumeAPI;
using GameAPI.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameAPI.WebMVC.Controllers
{
    public class GenresController : Controller
    {
        private readonly string apiUrl;

        public GenresController(IConfiguration configuration)
        {
            apiUrl = configuration["ApiURL"] + "Genres";
        }

        // GET: GenresController
        public ActionResult Index()
        {
            try
            {
                var data = Crud<GenreResponseDTO>.Read_All(apiUrl);
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading genres: {ex.Message}";
                return View(Array.Empty<GenreDTO>());
            }
        }

        // GET: GenresController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var data = Crud<GenreResponseDTO>.Read_ById(apiUrl, id);
                if (data == null)
                {
                    return NotFound();
                }
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading genre details: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: GenresController/Create
        public ActionResult Create()
        {
            return View(new CreateGenreDTO());
        }

        // POST: GenresController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateGenreDTO genre)
        {
            if (!ModelState.IsValid)
            {
                return View(genre);
            }

            try
            {
                var result = Crud<CreateGenreDTO>.Create(apiUrl, genre);
                if (result != null)
                {
                    TempData["Success"] = "Genre created successfully";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Failed to create genre");
                return View(genre);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error creating genre: {ex.Message}");
                return View(genre);
            }
        }

        // GET: GenresController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var genre = Crud<GenreDTO>.Read_ById(apiUrl, id);
                if (genre == null)
                {
                    return NotFound();
                }

                var updateDto = new UpdateGenreDTO
                {
                    Name = genre.Name
                };
                return View(updateDto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading genre: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: GenresController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UpdateGenreDTO genre)
        {
            if (!ModelState.IsValid)
            {
                return View(genre);
            }

            try
            {
                var success = Crud<UpdateGenreDTO>.Update(apiUrl, id, genre);
                if (success)
                {
                    TempData["Success"] = "Genre updated successfully";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Failed to update genre");
                return View(genre);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error updating genre: {ex.Message}");
                return View(genre);
            }
        }

        // GET: GenresController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var genre = Crud<GenreDTO>.Read_ById(apiUrl, id);
                if (genre == null)
                {
                    return NotFound();
                }
                return View(genre);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading genre: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: GenresController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var success = Crud<GenreDTO>.Delete(apiUrl, id);
                if (success)
                {
                    TempData["Success"] = "Genre deleted successfully";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Error"] = "Failed to delete genre";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error deleting genre: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
