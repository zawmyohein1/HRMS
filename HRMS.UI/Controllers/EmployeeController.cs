using Microsoft.AspNetCore.Mvc;
using HRMS.Models.View;
using HRMS.Models.Requests;
using HRMS.UI.Controllers;
using HRMS.UI.Helper;
using HRMS.Common.Messages;
using HRMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EmployeeController : BaseController
{
    private static readonly string endPoint = "employee";

    public EmployeeController(ApiService apiService) : base(apiService)
    {
    }

    //  GET: Employee/Index
    public async Task<IActionResult> Index()
    {
        var response = await _apiService.GetSingleAsync<EmployeeDTOPage>(endPoint);

        if (response == null || response.StatusCode == 0 || response?.Data == null)
        {
            TempData["ErrorMessage"] = Messages.DataRetrievalFailed;
            return View(new EmployeeDTOPage());
        }

        return View(response.Data);
    }

    [HttpGet]
    public async Task<IActionResult> Filter(int employeeId, string name, string gender, int? departmentId, string status, DateTime? startDate, DateTime? endDate)
    {
        try
        {
            var request = new EmployeeFilterRequest
            {
                EmployeeId = employeeId,
                Name = name,
                Gender = gender,
                DepartmentId = departmentId,
                Status = status,
                StartDate = startDate,
                EndDate = endDate
            };

            var response = await _apiService.PostAsyncT2<List<EmployeeModel>>($"{endPoint}/Filter", request);

            if (response == null || response.StatusCode != 200 || response?.Data == null)
            {
                return BadRequest(new { message = response?.Message ?? Messages.DataRetrievalFailed });
            }

            return PartialView("_EmployeeTablePartial", response.Data);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = Messages.DataFilterFailed, error = ex.Message });
        }
    }

    //  GET: Employee/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View("Form", new EmployeeModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EmployeeModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Form", model);
        }

        var response = await _apiService.PostAsync<EmployeeModel>(endPoint, model);
        if (response != null && (response.StatusCode == 200 || response.StatusCode == 201))
        {
            TempData["SuccessMessage"] = Messages.DataSaveSuccess;
            return RedirectToAction("Index");
        }

        TempData["ErrorMessage"] = response?.Message ?? Messages.DataSaveFailed;
        return View("Form", model);
    }

    //  GET: Employee/Edit/{id}
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var response = await _apiService.GetSingleAsync<EmployeeModel>($"{endPoint}/{id}");

        if (response == null || response.StatusCode != 200 || response.Data == null)
        {
            TempData["ErrorMessage"] = response?.Message ?? Messages.DataRetrievalFailed;
            return RedirectToAction("Index");
        }

        return View("Form", response.Data);
    }

    //  PUT: Employee/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EmployeeModel employee)
    {
        if (!ModelState.IsValid)
        {
            return View("Form", employee);
        }

        var response = await _apiService.PutAsync<EmployeeModel>($"{endPoint}/{employee.Id}", employee);
        if (response != null && response.StatusCode == 200)
        {
            TempData["SuccessMessage"] = Messages.DataUpdateSuccess;
            return RedirectToAction("Index");
        }

        TempData["ErrorMessage"] = response?.Message ?? Messages.DataUpdateFailed;
        return View("Form", employee);
    }

    //  DELETE: Employee/Delete
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _apiService.DeleteAsync<EmployeeModel>($"{endPoint}/{id}");

        if (response == null || response.StatusCode != 200)
        {
            return Json(new { success = false, message = response?.Message ?? Messages.DataDeletionFailed });
        }

        return Json(new { success = true, message = Messages.DataDeleteSuccess });
    }
}
