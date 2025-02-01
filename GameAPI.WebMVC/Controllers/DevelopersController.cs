using GameAPI.ConsumeAPI;
using GameAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameAPI.WebMVC.Controllers
{
    [Authorize]
    public class DevelopersController : Controller
    {

        private readonly string apiUrl;

        public DevelopersController(IConfiguration configuration)
        {
            apiUrl = configuration["ApiURL"] + "Developers";
        }

        // GET: DevelopersController
        public ActionResult Index()
        {
            try
            {
                var data = Crud<DeveloperResponseDTO>.Read_All(apiUrl);
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading developers: {ex.Message}";
                return View(Array.Empty<DeveloperResponseDTO>());
            }
        }

        // GET: DevelopersController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var data = Crud<DeveloperResponseDTO>.Read_ById(apiUrl, id);
                if (data == null)
                {
                    return NotFound();
                }
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading developer details: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: DevelopersController/Create
        public ActionResult Create()
        {
            return View(new CreateDeveloperDTO());
        }

        // POST: DevelopersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateDeveloperDTO developer)
        {
            if (!ModelState.IsValid)
            {
                return View(developer);
            }

            try
            {
                var result = Crud<CreateDeveloperDTO>.Create(apiUrl, developer);
                if (result != null)
                {
                    TempData["Success"] = "Developer created successfully";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Failed to create developer");
                return View(developer);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error creating developer: {ex.Message}");
                return View(developer);
            }
        }

        // GET: DevelopersController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var developer = Crud<DeveloperResponseDTO>.Read_ById(apiUrl, id);
                if (developer == null)
                {
                    return NotFound();
                }

                var updateDto = new UpdateDeveloperDTO
                {
                    Name = developer.Name,
                    Location = developer.Location
                };
                return View(updateDto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading developer: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: DevelopersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UpdateDeveloperDTO developer)
        {
            if (!ModelState.IsValid)
            {
                return View(developer);
            }

            try
            {
                var success = Crud<UpdateDeveloperDTO>.Update(apiUrl, id, developer);
                if (success)
                {
                    TempData["Success"] = "Developer updated successfully";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Failed to update developer");
                return View(developer);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error updating developer: {ex.Message}");
                return View(developer);
            }
        }

        // GET: DevelopersController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var developer = Crud<DeveloperResponseDTO>.Read_ById(apiUrl, id);
                if (developer == null)
                {
                    return NotFound();
                }
                return View(developer);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading developer: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: DevelopersController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var success = Crud<DeveloperDTO>.Delete(apiUrl, id);
                if (success)
                {
                    TempData["Success"] = "Developer deleted successfully";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Error"] = "Failed to delete developer";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error deleting developer: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
