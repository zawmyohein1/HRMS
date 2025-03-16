using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Models.Enums
{
    public class Enum
    {
        public enum Gender
        {
            Male,
            Female,
            Other
        }

        public enum EmployeeStatus
        {
            Active,
            Inactive,
            Terminated,
            OnLeave
        }
    }
}
