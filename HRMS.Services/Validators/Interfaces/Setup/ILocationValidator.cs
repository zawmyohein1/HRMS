using HRMS.Model.Responses;
using HRMS.Models.Views.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Services.Validators.Interfaces.Setup
{
    public interface ILocationValidator
    {
        Task<ResponseModel<LocationModel>> Validate(LocationModel model);
    }
}
