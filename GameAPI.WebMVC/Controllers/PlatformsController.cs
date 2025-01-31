using GameAPI.ConsumeAPI;
using GameAPI.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameAPI.WebMVC.Controllers
{
    public class PlatformsController : Controller
    {

        private readonly string apiUrl;

        public PlatformsController(IConfiguration configuration)
        {
            apiUrl = configuration["ApiURL"] + "Platforms";
        }

        // GET: PlatformsController
        public ActionResult Index()
        {
            try
            {
                var data = Crud<PlatformResponseDTO>.Read_All(apiUrl);
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading platforms: {ex.Message}";
                return View(Array.Empty<PlatformResponseDTO>());
            }
        }

        // GET: PlatformsController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var data = Crud<PlatformResponseDTO>.Read_ById(apiUrl, id);
                if (data == null)
                {
                    return NotFound();
                }
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading platform details: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: PlatformsController/Create
        public ActionResult Create()
        {
            return View(new InputPlatformDTO());
        }

        // POST: PlatformsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InputPlatformDTO platform)
        {
            if (!ModelState.IsValid)
            {
                return View(platform);
            }

            try
            {
                var result = Crud<InputPlatformDTO>.Create(apiUrl, platform);
                if (result != null)
                {
                    TempData["Success"] = "Platform created successfully";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Failed to create platform");
                return View(platform);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error creating platform: {ex.Message}");
                return View(platform);
            }
        }

        // GET: PlatformsController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var platform = Crud<PlatformResponseDTO>.Read_ById(apiUrl, id);
                if (platform == null)
                {
                    return NotFound();
                }

                var updateDto = new InputPlatformDTO
                {
                    Name = platform.Name
                };
                return View(updateDto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading platform: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: PlatformsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, InputPlatformDTO platform)
        {
            if (!ModelState.IsValid)
            {
                return View(platform);
            }

            try
            {
                var success = Crud<InputPlatformDTO>.Update(apiUrl, id, platform);
                if (success)
                {
                    TempData["Success"] = "Platform updated successfully";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Failed to update platform");
                return View(platform);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error updating platform: {ex.Message}");
                return View(platform);
            }
        }

        // GET: PlatformsController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var platform = Crud<PlatformResponseDTO>.Read_ById(apiUrl, id);
                if (platform == null)
                {
                    return NotFound();
                }
                return View(platform);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading platform: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: PlatformsController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var success = Crud<PlatformDTO>.Delete(apiUrl, id);
                if (success)
                {
                    TempData["Success"] = "Platform deleted successfully";
                    return RedirectToAction(nameof(Index));
                }

                TempData["Error"] = "Failed to delete platform";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error deleting platform: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
