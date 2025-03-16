using HRMS.UI.Helper;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.UI.Controllers
{
    public class BaseController : Controller
    {
        public readonly ApiService _apiService;
        public BaseController(ApiService apiService)
        {
            _apiService = apiService;
        }
    }
}