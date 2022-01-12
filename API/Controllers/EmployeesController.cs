using API.Models;
using API.Repository;
using API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class EmployeesController : BaseController<Employee, EmployeeRepository, string>
    {
        private readonly EmployeeRepository employeeRepository;
        public EmployeesController(EmployeeRepository employeeRepository) : base(employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        [HttpPost("Register")] //localhost/api/employee/register
        public ActionResult Register(RegisterVM registerVM)
        {
            try
            {
                var register = employeeRepository.Register(registerVM);
                if (register == 1)
                {
                    return Created("", new { status = "success", data = registerVM, message = "Successfully Registered" });
                }
                else
                {
                    return BadRequest(new { status = "failed", message = "unexpected error" });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { status = "failed", message = e });
            }
        }

        //[Authorize(Roles = "Director")]
        [HttpGet("Registered")]
        public ActionResult RegisteredData()
        {
            try
            {
                var registeredData = employeeRepository.GetRegisteredData();
                if (registeredData == null)
                {
                    return Ok(new { status = "success", data = "no data found" });
                }

                return Ok(new { status = "success", data = registeredData });
            }
            catch (Exception e)
            {
                return BadRequest(new { status = "failed", message = e });
            }

        }

        [Authorize(Roles = "Director")]
        [HttpGet("Registeredalt")]
        public ActionResult Coba()
        {
            try
            {
                var registeredData = employeeRepository.GetRegisteredDataAlt();
                if (registeredData == null)
                {
                    return Ok(new { status = "success", data = "no data found" });
                }
                return Ok(new { status = "success", data = registeredData });
            }
            catch (Exception e)
            {
                return BadRequest(new { status = "failed", message = e });
            }

        }

        [HttpGet("TestCORS")]
        public ActionResult TestCORS()
        {
            return Ok("Test CORS Berhasil");
        }

        [HttpGet("GenderStat")]
        public ActionResult GenderStat()
        {
            try
            {
                var data = employeeRepository.GetGenderStat();
                if (data == null)
                {
                    return Ok(new { status = "success", data = "no data found" });
                }

                return Ok(new { status = "success", data = data });
            }
            catch (Exception e)
            {
                return BadRequest(new { status = "failed", message = e });
            }
        }
    }
}
