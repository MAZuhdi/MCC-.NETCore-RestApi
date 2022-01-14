using API.Models;
using API.ViewModels;
using Client.Base.Controllers;
using Client.Models;
using Client.Repositories.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Controllers
{
    //[Authorize]
    public class EmployeesController : BaseController<Employee, EmployeeRepository, string>
    {
        private EmployeeRepository repository;
        public EmployeesController(EmployeeRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetRegistered()
        {
            var result = await repository.GetRegistered();
            return Json(result);
        }

        [HttpGet("Employees/GetRegistered/{NIK}")]
        public async Task<JsonResult> GetRegisteredByNIK(string NIK)
        {
            var result = await repository.GetRegisteredByNIK(NIK);
            return Json(result);
        }

        [HttpPost("Employees/Register")]
        public JsonResult Register(RegisterVM entity)
        {
            var result = repository.Register(entity);
            return Json(result);
        }
    }
}
