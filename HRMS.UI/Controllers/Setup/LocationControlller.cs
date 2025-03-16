using HRMS.Common.Messages;
using HRMS.Models.Views.Setup;
using HRMS.UI.Helper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.UI.Controllers.Setup
{
    public class LocationController : BaseController
    {
        private static readonly string endPoint = "location";

        public LocationController(ApiService apiService) : base(apiService)
        {
        }

        // GET: Setup/Location (Index)
        public async Task<IActionResult> Index()
        {
            var response = await _apiService.GetAsync<LocationModel>(endPoint);

            if (response == null || response.StatusCode == 0 || response.Data == null)
            {
                TempData["ErrorMessage"] = Messages.DataRetrievalFailed;
                return View("~/Views/Setup/Location/Index.cshtml", new List<LocationModel>());
            }

            return View("~/Views/Setup/Location/Index.cshtml", response.Data);
        }

        // GET: Setup/Location/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View("~/Views/Setup/Location/Form.cshtml", new LocationModel());
        }

        // POST: Setup/Location/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LocationModel location)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Setup/Location/Form.cshtml", location);
            }

            var response = await _apiService.PostAsync<LocationModel>(endPoint, location);
            if (response != null && (response.StatusCode == 200 || response.StatusCode == 201))
            {
                TempData["SuccessMessage"] = Messages.DataSaveSuccess;
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = response?.Message ?? Messages.DataCreationFailed;
            Console.WriteLine($"Error Creating Location: {response?.Message}");
            return View("~/Views/Setup/Location/Form.cshtml", location);
        }

        // GET: Setup/Location/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _apiService.GetSingleAsync<LocationModel>($"{endPoint}/{id}");
            if (response == null || response.StatusCode != 200 || response.Data == null)
            {
                TempData["ErrorMessage"] = response?.Message ?? Messages.DataRetrievalFailed;
                return RedirectToAction("Index");
            }

            return View("~/Views/Setup/Location/Form.cshtml", response.Data);
        }

        // PUT: Setup/Location/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LocationModel location)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Setup/Location/Form.cshtml", location);
            }

            var response = await _apiService.PutAsync<LocationModel>($"{endPoint}/{location.Id}", location);
            if (response != null && response.StatusCode == 200)
            {
                TempData["SuccessMessage"] = Messages.DataUpdateSuccess;
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = response?.Message ?? Messages.DataUpdateFailed;
            Console.WriteLine($"Error Updating Location: {response?.Message}");
            return View("~/Views/Setup/Location/Form.cshtml", location);
        }

        // DELETE: Setup/Location/Delete/{id}
        [HttpPost]  // Changed from [HttpDelete] to [HttpPost] for CSRF protection
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiService.DeleteAsync<LocationModel>($"{endPoint}/{id}");

            if (response == null || response.StatusCode != 200)
            {
                return Json(new { success = false, message = response?.Message ?? Messages.DataDeletionFailed });
            }

            return Json(new { success = true, message = Messages.DataDeleteSuccess });
        }
    }
}
