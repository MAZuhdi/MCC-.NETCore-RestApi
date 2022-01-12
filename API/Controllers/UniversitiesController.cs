using API.Models;
using API.Repository;
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
    public class UniversitiesController : BaseController<University, UniversityRepository, int>
    {
        private readonly UniversityRepository universityRepository;
        public UniversitiesController(UniversityRepository universityRepository) : base(universityRepository)
        {
            this.universityRepository = universityRepository;
        }

        [HttpGet("UniversityStat")]
        public ActionResult UniversityStat()
        {
            try
            {
                var data = universityRepository.GetUniversityStat();
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
