using HRMS.Common.Messages;
using HRMS.Models.Views.Setup;
using HRMS.UI.Helper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.UI.Controllers.Setup
{
    public class DepartmentController : BaseController
    {
        private static readonly string endPoint = "department";

        public DepartmentController(ApiService apiService) : base(apiService)
        {
        }

        // GET: Setup/Department (Index)
        public async Task<IActionResult> Index()
        {
            var response = await _apiService.GetAsync<DepartmentModel>(endPoint);

            if (response == null || response.StatusCode == 0 || response.Data == null)
            {
                TempData["ErrorMessage"] = Messages.DataRetrievalFailed;
                return View("~/Views/Setup/Department/Index.cshtml", new List<DepartmentModel>());
            }

            return View("~/Views/Setup/Department/Index.cshtml", response.Data);
        }

        // GET: Setup/Department/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var response = await _apiService.GetSingleAsync<DepartmentModel>($"{endPoint}/create");

            if (response == null || response.StatusCode != 200 || response.Data == null)
            {
                TempData["ErrorMessage"] = response?.Message ?? Messages.DataRetrievalFailed;
                return RedirectToAction("Index");
            }

            return View("~/Views/Setup/Department/Form.cshtml", response.Data);
        }

        // POST: Setup/Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentModel department)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Setup/Department/Form.cshtml", department);
            }

            var response = await _apiService.PostAsync<DepartmentModel>(endPoint, department);
            if (response != null && (response.StatusCode == 200 || response.StatusCode == 201))
            {
               
                TempData["SuccessMessage"] = Messages.DataSaveSuccess;
                return RedirectToAction("Index");
            }
            
            TempData["ErrorMessage"] = response?.Message ?? Messages.DataCreationFailed;
            return View("~/Views/Setup/Department/Form.cshtml", response.Data);
        }

        // GET: Setup/Department/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _apiService.GetSingleAsync<DepartmentModel>($"{endPoint}/{id}");
            if (response == null || response.StatusCode != 200 || response.Data == null)
            {
                TempData["ErrorMessage"] = response?.Message ?? Messages.DataRetrievalFailed;
                return RedirectToAction("Index");
            }

            return View("~/Views/Setup/Department/Form.cshtml", response.Data);
        }

        // PUT: Setup/Department/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepartmentModel department)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Setup/Department/Form.cshtml", department);
            }

            var response = await _apiService.PutAsync<DepartmentModel>($"{endPoint}/{department.Id}", department);
            if (response != null && response.StatusCode == 200)
            {
                TempData["SuccessMessage"] = Messages.DataUpdateSuccess;
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = response?.Message ?? Messages.DataUpdateFailed;
            return View("~/Views/Setup/Department/Form.cshtml", department);
        }

        // DELETE: Setup/Department/Delete/{id}
        [HttpPost]  // Change from [HttpDelete] to [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _apiService.DeleteAsync<DepartmentModel>($"{endPoint}/{id}");

            if (response == null || response.StatusCode != 200)
            {
                return Json(new { success = false, message = response?.Message ?? Messages.DataDeletionFailed });
            }

            return Json(new { success = true, message = Messages.DataDeleteSuccess });
        }
    }
}
