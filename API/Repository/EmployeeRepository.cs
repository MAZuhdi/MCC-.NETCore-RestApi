using API.Context;
using API.Models;
using API.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace API.Repository
{
    public class EmployeeRepository : GeneralRepository<MyContext, Employee, string>
    {
        private readonly MyContext myContext;
        
        public EmployeeRepository(MyContext myContext) : base(myContext)
        {
            this.myContext = myContext;
        }


        public int Register(RegisterVM registerVM)
        {
            var checkEmail = myContext.Employees
                            .Where(e => e.Email == registerVM.Email)
                            .FirstOrDefault();
            if (checkEmail != null)
            {
                return 2; // Duplicate Email
            }

            var checkPhone = myContext.Employees
                            .Where(e => e.Phone == registerVM.Phone)
                            .FirstOrDefault();
            if (checkPhone != null)
            {
                return 3; // Duplicate Phone
            }

            int increment = myContext.Employees.ToList().Count;
            //variabel nik
            string formattedNIK = DateTime.Now.ToString("yyyy") + "0" + increment.ToString();
            int result;
            var employee = new Employee()
            {
                NIK = formattedNIK,
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Phone = registerVM.Phone,
                Email = registerVM.Email,
                Salary = registerVM.Salary,
                Gender = (Models.Gender)registerVM.Gender
            };
            try
            {
                myContext.Employees.Add(employee);
                result = myContext.SaveChanges();

            }
            catch (Exception)
            {
                return 4; //Employee add error
            }
            
            var account = new Account()
            {
                NIK = formattedNIK,
                Password = BCrypt.Net.BCrypt.HashPassword(registerVM.Password)
            };
            try
            {
                myContext.Accounts.Add(account);
                result = myContext.SaveChanges();
            }
            catch (Exception)
            {
                return 5; //Account add error
            }

            var education = new Education()
            {
                Degree = registerVM.Degree,
                GPA = registerVM.GPA,
                UniversityId = registerVM.UniversityId
            };
            try
            {
                myContext.Educations.Add(education);
                result = myContext.SaveChanges();
            }
            catch (Exception)
            {
                return 6; //Education add error
            }

            var profiling = new Profiling()
            {
                NIK = formattedNIK,
                EducationId = education.Id
            };
            try
            {
                myContext.Profilings.Add(profiling);
                result = myContext.SaveChanges();
            }
            catch (Exception)
            {
                return 7; //Profiling add error
            }

            var accountRole = new AccountRole()
            {
                AccountId = formattedNIK,
                RoleId = 1
            };

            try
            {
                myContext.AccountRoles.Add(accountRole);
                result = myContext.SaveChanges();
            }
            catch (Exception)
            {
                return 8; //Role add error
            }
            return result;
        }

        public IEnumerable GetRegisteredData()
        {
            var query = (from employee in myContext.Set<Employee>()
                        join account in myContext.Set<Account>()
                            on employee.NIK equals account.NIK
                        join profiling in myContext.Set<Profiling>()
                            on account.NIK equals profiling.NIK
                        join education in myContext.Set<Education>()
                            on profiling.EducationId equals education.Id
                        join university in myContext.Set<University>()
                            on education.UniversityId equals university.Id
                        select new {
                            FullName = employee.FirstName + " " + employee.LastName,
                            PhoneNumber = employee.Phone,
                            BirthDate = employee.BirthDate,
                            Salary = employee.Salary,
                            Email = employee.Email,
                            Gender = employee.Gender,
                            Degree = education.Degree,
                            GPA = education.GPA,
                            UniversityName = university.Name
                        });
            return query.ToList();
        }

        public IEnumerable GetRegisteredDataAlt()
        {
            var data = myContext.Employees
                .Include(employee => employee.Account)
                .ThenInclude(account => account.Profiling)
                .ThenInclude(profiling => profiling.Education)
                .ThenInclude(education => education.University)
                .ToList();
            return data;
        }
    }
}
