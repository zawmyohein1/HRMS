using HRMS.Common.Messages;
using HRMS.Models.Views.Setup;
using HRMS.UI.Helper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.UI.Controllers.Setup
{
    public class JobRoleController : BaseController
    {
        private static readonly string endPoint = "jobrole";

        public JobRoleController(ApiService apiService) : base(apiService)
        {
        }

        // GET: Setup/JobRole (Index)
        public async Task<IActionResult> Index()
        {
            var response = await _apiService.GetAsync<JobRoleModel>(endPoint);

            if (response == null || response.StatusCode == 0 || response.Data == null)
            {
                TempData["ErrorMessage"] = Messages.DataRetrievalFailed;
                return View("~/Views/Setup/JobRole/Index.cshtml", new List<JobRoleModel>());
            }

            return View("~/Views/Setup/JobRole/Index.cshtml", response.Data);
        }

        // GET: Setup/JobRole/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var response = await _apiService.GetSingleAsync<JobRoleModel>($"{endPoint}/create");

            if (response == null || response.StatusCode != 200 || response.Data == null)
            {
                TempData["ErrorMessage"] = response?.Message ?? Messages.DataRetrievalFailed;
                return RedirectToAction("Index");
            }

            return View("~/Views/Setup/JobRole/Form.cshtml", response.Data);
        }

        // POST: Setup/JobRole/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobRoleModel jobRole)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Setup/JobRole/Form.cshtml", jobRole);
            }

            var response = await _apiService.PostAsync<JobRoleModel>(endPoint, jobRole);
            if (response != null && (response.StatusCode == 200 || response.StatusCode == 201))
            {
                TempData["SuccessMessage"] = Messages.DataSaveSuccess;
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = response?.Message ?? Messages.DataCreationFailed;
            return View("~/Views/Setup/JobRole/Form.cshtml", response.Data);
        }

        // GET: Setup/JobRole/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _apiService.GetSingleAsync<JobRoleModel>($"{endPoint}/{id}");
            if (response == null || response.StatusCode != 200 || response.Data == null)
            {
                TempData["ErrorMessage"] = response?.Message ?? Messages.DataRetrievalFailed;
                return RedirectToAction("Index");
            }

            return View("~/Views/Setup/JobRole/Form.cshtml", response.Data);
        }

        // PUT: Setup/JobRole/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(JobRoleModel jobRole)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Setup/JobRole/Form.cshtml", jobRole);
            }

            var response = await _apiService.PutAsync<JobRoleModel>($"{endPoint}/{jobRole.Id}", jobRole);
            if (response != null && response.StatusCode == 200)
            {
                TempData["SuccessMessage"] = Messages.DataUpdateSuccess;
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = response?.Message ?? Messages.DataUpdateFailed;
            return View("~/Views/Setup/JobRole/Form.cshtml", jobRole);
        }

        // DELETE: Setup/JobRole/Delete/{id}
        [HttpPost]  // Changed from [HttpDelete] to [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiService.DeleteAsync<JobRoleModel>($"{endPoint}/{id}");

            if (response == null || response.StatusCode != 200)
            {
                return Json(new { success = false, message = response?.Message ?? Messages.DataDeletionFailed });
            }

            return Json(new { success = true, message = Messages.DataDeleteSuccess });
        }
    }
}
