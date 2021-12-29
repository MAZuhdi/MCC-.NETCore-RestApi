﻿using API.Models;
using API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountRolesController : BaseController<AccountRole, AccountRoleRepository, string>
    {
        private AccountRoleRepository accountRoleRepository;
        public AccountRolesController(AccountRoleRepository accountRoleRepository) : base(accountRoleRepository)
        {
            this.accountRoleRepository = accountRoleRepository;
        }
    }
}
