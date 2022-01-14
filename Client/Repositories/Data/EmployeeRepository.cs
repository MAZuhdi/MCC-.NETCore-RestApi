using API.Models;
using API.ViewModels;
using Client.Base.Urls;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace Client.Repositories.Data
{
    public class EmployeeRepository : GeneralRepository<Employee, string>
    {
        private readonly Address address;
        private readonly string request;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpClient httpClient;


        public EmployeeRepository(Address address, string request = "Employees/") : base(address, request)
        {
            this.address = address;
            this.request = request;
            _contextAccessor = new HttpContextAccessor();
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(address.link)
            };

            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", _contextAccessor.HttpContext.Session.GetString("JWToken"));
        }

        public async Task<List<RegisteredVM>> GetRegistered()
        {
            List<RegisteredVM> entities = new List<RegisteredVM>();

            using (var response = await httpClient.GetAsync(request + "Registered/")) //bikin URL target API
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entities = JsonConvert.DeserializeObject<List<RegisteredVM>>(apiResponse);
            }
            return entities;
        }

        public async Task<RegisteredVM> GetRegisteredByNIK(string nik)
        {
            RegisteredVM entities = new RegisteredVM();

            using (var response = await httpClient.GetAsync(request + "Registered/" + nik)) //bikin URL target API
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entities = JsonConvert.DeserializeObject<RegisteredVM>(apiResponse);
            }
            return entities;
        }

        public HttpStatusCode Register(RegisterVM entity)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            var result = httpClient.PostAsync(address.link + request + "Register/", content).Result;
            return result.StatusCode;
        }

        public HttpStatusCode DeleteRegistered(string id)
        {
            var result = httpClient.DeleteAsync(request + "Registered/" + id).Result;
            return result.StatusCode;
        }
    }
}
