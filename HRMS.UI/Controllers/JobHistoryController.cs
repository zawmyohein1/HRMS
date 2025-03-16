using HRMS.Common.Messages;
using HRMS.Models.View;
using HRMS.UI.Controllers;
using HRMS.UI.Helper;
using Microsoft.AspNetCore.Mvc;

public class JobHistoryController : BaseController
{
    private static readonly string endPoint = "jobHistory";

    public JobHistoryController(ApiService apiService) : base(apiService)
    {
    }

    //  GET: JobHistory (Index)
    public async Task<IActionResult> Index()
    {
        var response = await _apiService.GetAsync<JobHistoryModel>(endPoint);

        if (response == null || response.StatusCode == 0 || response?.Data == null)
        {
            TempData["ErrorMessage"] = Messages.DataFilterFailed;
            return View(new List<JobHistoryModel>());
        }

        return View(response.Data);
    }

    //  GET: JobHistory/Create
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var response = await _apiService.GetSingleAsync<JobHistoryModel>($"{endPoint}/create");

        if (response == null || response.StatusCode != 200 || response.Data == null)
        {
            TempData["ErrorMessage"] = response?.Message ?? Messages.DataRetrievalFailed;
            return RedirectToAction("Index");
        }

        return View("Form", response.Data);
    }

    //  POST: JobHistory/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(JobHistoryModel jobHistory)
    {
        if (!ModelState.IsValid)
        {
            return View("Form", jobHistory);
        }

        var response = await _apiService.PostAsync<JobHistoryModel>(endPoint, jobHistory);
        if (response != null && (response.StatusCode == 200 || response.StatusCode == 201))
        {
            TempData["SuccessMessage"] = Messages.DataSaveSuccess;
            return RedirectToAction("Index");
        }

        TempData["ErrorMessage"] = response?.Message ?? Messages.DataSaveFailed;
        return View("Form", jobHistory);
    }

    //  GET: JobHistory/Edit/{id}
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var response = await _apiService.GetSingleAsync<JobHistoryModel>($"{endPoint}/{id}");

        if (response == null || response.StatusCode != 200 || response.Data == null)
        {
            TempData["ErrorMessage"] = response?.Message ?? Messages.DataRetrievalFailed;
            return RedirectToAction("Index");
        }

        return View("Form", response.Data);
    }

    //  PUT: JobHistory/Edit 
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(JobHistoryModel jobHistory)
    {
        if (!ModelState.IsValid)
        {
            return View("Form", jobHistory);
        }

        var response = await _apiService.PutAsync<JobHistoryModel>($"{endPoint}/{jobHistory.Id}", jobHistory);
        if (response != null && response.StatusCode == 200)
        {
            TempData["SuccessMessage"] = Messages.DataUpdateSuccess;
            return RedirectToAction("Index");
        }

        TempData["ErrorMessage"] = response?.Message ?? Messages.DataUpdateFailed;
        return View("Form", jobHistory);
    }

    //  DELETE: JobHistory/Delete/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _apiService.DeleteAsync<JobHistoryModel>($"{endPoint}/{id}");

        if (response == null || response.StatusCode != 200)
        {
            return Json(new { success = false, message = response?.Message ?? Messages.DataDeletionFailed });
        }

        return Json(new { success = true, message = Messages.DataDeleteSuccess });
    }
}
