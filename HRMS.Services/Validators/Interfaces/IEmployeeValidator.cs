using HRMS.Model.Responses;
using HRMS.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Services.Validators.Interfaces
{
    public interface IEmployeeValidator
    {
        Task<ResponseModel<EmployeeModel>> Validate(EmployeeModel model);
    }
}
