using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GameAPI.ConsumeAPI;
using GameAPI.Models.DTO;

namespace GameAPI.WebMVC.Controllers
{
    public class GamesController : Controller
    {
        private readonly string apiUrl;
        private readonly string genresUrl;
        private readonly string developersUrl;
        private readonly string platformsUrl;

        public GamesController(IConfiguration configuration)
        {
            apiUrl = configuration["ApiURL"] + "Games";
            genresUrl = configuration["ApiURL"] + "Genres";
            developersUrl = configuration["ApiURL"] + "Developers";
            platformsUrl = configuration["ApiURL"] + "Platforms";
        }

        // GET: GamesController
        public ActionResult Index()
        {
            try
            {
                var data = Crud<GameResponseDTO>.Read_All(apiUrl);
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading games: {ex.Message}";
                return View(Array.Empty<GameResponseDTO>());
            }
        }

        // GET: GamesController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var data = Crud<GameResponseDTO>.Read_ById(apiUrl, id);
                if (data == null)
                {
                    return NotFound();
                }
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading game details: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: GamesController/Create
        public ActionResult Create()
        {
            try
            {
                // Get dropdown data
                ViewBag.Genres = Crud<GenreResponseDTO>.Read_All(genresUrl);
                ViewBag.Developers = Crud<DeveloperResponseDTO>.Read_All(developersUrl);
                ViewBag.Platforms = Crud<PlatformResponseDTO>.Read_All(platformsUrl);


                return View(new CreateGameDTO());
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading form data: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: GamesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateGameDTO game)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Genres = Crud<GenreResponseDTO>.Read_All(genresUrl);
                ViewBag.Developers = Crud<DeveloperResponseDTO>.Read_All(developersUrl);
                ViewBag.Platforms = Crud<PlatformResponseDTO>.Read_All(platformsUrl);
                return View(game);
            }

            try
            {
                var result = Crud<CreateGameDTO>.Create(apiUrl, game);
                if (result != null)
                {
                    TempData["Success"] = "Game created successfully";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Failed to create game");
                return View(game);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error creating game: {ex.Message}");
                ViewBag.Genres = Crud<GenreResponseDTO>.Read_All(genresUrl);
                ViewBag.Developers = Crud<DeveloperDTO>.Read_All(developersUrl);
                ViewBag.Platforms = Crud<PlatformDTO>.Read_All(platformsUrl);
                return View(game);
            }
        }

        // GET: GamesController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var game = Crud<GameResponseDTO>.Read_ById(apiUrl, id);
                if (game == null)
                {
                    return NotFound();
                }

                ViewBag.Genres = Crud<GenreResponseDTO>.Read_All(genresUrl);
                ViewBag.Developers = Crud<DeveloperResponseDTO>.Read_All(developersUrl);
                ViewBag.Platforms = Crud<PlatformResponseDTO>.Read_All(platformsUrl);

                var updateDto = new UpdateGameDTO
                {
                    Name = game.Name,
                    Description = game.Description,
                    ReleaseDate = game.ReleaseDate,
                    GenreId = game.Genre.Id,
                    DeveloperId = game.Developer.Id,
                    PlatformIds = game.Platforms.Select(p => p.Id).ToList()
                };
                return View(updateDto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading game: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: GamesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UpdateGameDTO game)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Genres = Crud<GenreResponseDTO>.Read_All(genresUrl);
                ViewBag.Developers = Crud<DeveloperResponseDTO>.Read_All(developersUrl);
                ViewBag.Platforms = Crud<PlatformResponseDTO>.Read_All(platformsUrl);
                return View(game);
            }

            try
            {
                var success = Crud<UpdateGameDTO>.Update(apiUrl, id, game);
                if (success)
                {
                    TempData["Success"] = "Game updated successfully";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Failed to update game");
                return View(game);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error updating game: {ex.Message}");
                ViewBag.Genres = Crud<GenreResponseDTO>.Read_All(genresUrl);
                ViewBag.Developers = Crud<DeveloperResponseDTO>.Read_All(developersUrl);
                ViewBag.Platforms = Crud<PlatformResponseDTO>.Read_All(platformsUrl);
                return View(game);
            }
        }

        // GET: GamesController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var game = Crud<GameResponseDTO>.Read_ById(apiUrl, id);
                if (game == null)
                {
                    return NotFound();
                }
                return View(game);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading game: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: GamesController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var success = Crud<GameResponseDTO>.Delete(apiUrl, id);
                if (success)
                {
                    TempData["Success"] = "Game deleted successfully";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Error"] = "Failed to delete game";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error deleting game: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
