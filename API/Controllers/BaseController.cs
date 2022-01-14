using API.Repository.Interface;
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
    public class BaseController<Entity, Repository, Key> : ControllerBase 
        where Entity : class
        where Repository : IRepository<Entity, Key>
    {
        private readonly Repository repository;

        public BaseController(Repository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                var result = repository.Get();
                if (result.Count() == 0)
                {
                    return Ok(new { status = "success", data = "null", message = "no data found" });
                }
                //return Ok(new { status = "success", data = result, message = "all of data" });
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { status = "failed", message = e });
            }
        }

        [HttpGet("{key}")]
        public ActionResult Get(Key key)
        {
            try
            {
                var result = repository.Get(key);
                if (result == null)
                {
                    return NotFound(new
                    {
                        status = "success",
                        data = "null",
                        message = "no data found"
                    });
                }
                return Ok(new
                {
                    status = "success",
                    data = result,
                    message = "Detailed data"
                });
            }
            catch (Exception e)
            {
                return BadRequest(new { status = "failed", message = e });
            }
        }

        [HttpPost]
        public ActionResult Insert(Entity entity)
        {
            try
            {
                var insert = repository.Insert(entity);
                if (insert > 0)
                {
                    return Created("", new{status = "success",data = entity,message = "record created"});
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

        [HttpPut]
        public ActionResult Update(Entity entity)
        {
            try
            {
                var update = repository.Update(entity);
                if (update > 0)
                {
                    return Ok(new { status = "success", data = entity, message = "record updated" });
                }
                else
                {
                    return BadRequest(new { status = "failed", message = "bad request" });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { status = "failed", message = e });
            }
        }

        [HttpDelete("{key}")]
        public ActionResult Delete(Key key)
        {
            try
            {
                var delete = repository.Delete(key);
                if (delete == 2)
                {
                    return NotFound(new
                    {
                        status = "failed",
                        message = "Record not found"
                    });
                }
                if (delete > 0)
                {
                    return Ok(new
                    {
                        status = "success",
                        message = "record successfuly deleted"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        status = "failed",
                        message = "bad request"
                    });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    status = "failed",
                    message = "bad request " + e
                });
            }
        }
    }
}

