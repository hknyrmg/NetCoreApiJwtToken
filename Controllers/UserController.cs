using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TokenBasedAuth_NetCore.Entities;
using TokenBasedAuth_NetCore.Models;
using TokenBasedAuth_NetCore.Providers;
using TokenBasedAuth_NetCore.Repository;
using TokenBasedAuth_NetCore.Services;
using TokenBasedAuth_NetCore.UnitofWork;
using TokenBasedAuth_NetCore.Utils;
using Dapper;
using System.Data;
using System.Collections;
using TokenBasedAuth_NetCore.Entities.Enums;
using Microsoft.AspNetCore.Cors;

namespace TokenBasedAuth_NetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IGenericRepository<User> _genericRepository;

        private readonly ICacheProvider _cacheProvider;
        private readonly IUserService _userService;
        private readonly IDapperService _dapperService;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(
          IUnitOfWork unitOfWork,
          ICacheProvider cacheProvider,
          IUserService userService,
          IDapperService dapperService)
        {
            _unitOfWork = unitOfWork;
            _genericRepository = _unitOfWork.GetRepository<User>();
            _cacheProvider = cacheProvider;
            _userService = userService;
            _dapperService = dapperService;

        }

        [DisableCors]


        [HttpPost("authenticate")]
        [AllowAnonymous]
        public IActionResult Authenticate([FromBody] LoginModel loginModel)
        {
            var response = _userService.Authenticate(loginModel);
            if (response == null)
                return BadRequest("Username or password is wrong.");

            ResponseModel<TokenModel> responseModel = new ResponseModel<TokenModel>()
            {
                Status = HttpStatusCode.OK,
                Message = "Succesfully Logined",
                Result = response
            };

            return Ok(responseModel);

        }
        
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();

            if (users == null || users.Count() == 0)
                return BadRequest("Users Not Found");

            ResponseModel<IEnumerable<User>> responseModel = new ResponseModel<IEnumerable<User>>()
            {
                Status = HttpStatusCode.OK,
                Message = "Succesfully retrieved",
                Result = users
            };
            return Ok(responseModel);
        }


        [AllowAnonymous]

        [HttpPost("Add")]
        public IActionResult AddUserOld([FromBody] User user)
        {
            user.Password = HashUtil.GetSha256FromString(user.Password);

            _genericRepository.Add(user);
           var result = _unitOfWork.SaveChanges();
            if(result > 0)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest("Bad Request...");
            }
        }


        [AllowAnonymous]

        [HttpPost("AddDapper")]
        public async Task<IActionResult> AddDapperAsync([FromBody] User user)
        {
            user.Password = HashUtil.GetSha256FromString(user.Password);

            var dbparams = new DynamicParameters();
            dbparams.Add("UserName", user.UserName, DbType.String);
            dbparams.Add("Name", user.Name, DbType.String);
            dbparams.Add("SurName", user.SurName, DbType.String);
            dbparams.Add("Password", user.Password, DbType.String);
            dbparams.Add("Email", user.Email, DbType.String);
            dbparams.Add("Phone", user.Phone, DbType.String);
            dbparams.Add("Token", user.Token, DbType.String);
            dbparams.Add("Role", user.Role, DbType.String);

            dbparams.Add("StatementType", SpConstantKeys.SPStatementTypeInsert, DbType.String);

            var result = await Task.FromResult(_dapperService.Insert<int>("[UserDb].[dbo].[MasterCrudUser]"
                , dbparams,
                commandType: CommandType.StoredProcedure));
            if (result == 0)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest("Bad Request...");
            }
        }


        [Authorize(Roles = Role.Admin)]

        [HttpGet("GetAllDapper")]
        public async Task<IActionResult> GetAllDapperAsync()
        {
       
         
            

            var dbparams = new DynamicParameters();
           

            var users = await Task.FromResult(_dapperService.GetAll<User>(SpConstantKeys.SelectAllUsers
                , dbparams,
                commandType: CommandType.StoredProcedure));
    


            if (users == null || users.Count() == 0)
                return BadRequest("Users Not Found");

            ResponseModel<IEnumerable<User>> responseModel = new ResponseModel<IEnumerable<User>>()
            {
                Status = HttpStatusCode.OK,
                Message = "Succesfully retrieved",
                Result = users
            };
            return Ok(responseModel);
        }
        // [AllowAnonymous]

        //[HttpPost("Add")]
        //public IActionResult AddUser([FromBody] User user)
        //{

        //    var paramId = new SqlParameter()
        //    {
        //        ParameterName = "@id",
        //        SqlDbType = System.Data.SqlDbType.VarChar,
        //        Direction = System.Data.ParameterDirection.Input,
        //        Value = 1
        //    };
        //    var paramUserName = new SqlParameter()
        //    {
        //        ParameterName = "@UserName",
        //        SqlDbType = System.Data.SqlDbType.VarChar,
        //        Direction = System.Data.ParameterDirection.Input,
        //        Value = user.UserName
        //    };
        //    var paramName = new SqlParameter()
        //    {
        //        ParameterName = "@Name",
        //        SqlDbType = System.Data.SqlDbType.VarChar,
        //        Direction = System.Data.ParameterDirection.Input,
        //        Value = user.Name
        //    };
        //    var paramSurName = new SqlParameter()
        //    {
        //        ParameterName = "@SurName",
        //        SqlDbType = System.Data.SqlDbType.VarChar,
        //        Direction = System.Data.ParameterDirection.Input,
        //        Value = user.SurName
        //    };
        //    var paramPassword = new SqlParameter()
        //    {
        //        ParameterName = "@Password",
        //        SqlDbType = System.Data.SqlDbType.VarChar,
        //        Direction = System.Data.ParameterDirection.Input,
        //        Value = HashUtil.GetSha256FromString(user.Password)
        //    };
        //    var paramEMail = new SqlParameter()
        //    {
        //        ParameterName = "@Email",
        //        SqlDbType = System.Data.SqlDbType.NVarChar,
        //        Direction = System.Data.ParameterDirection.Input,
        //        Value = user.Email
        //    };
        //    var paramPhone = new SqlParameter()
        //    {
        //        ParameterName = "@Phone",
        //        SqlDbType = System.Data.SqlDbType.NVarChar,
        //        Direction = System.Data.ParameterDirection.Input,
        //        Value = user.Phone
        //    };
        //    var paramLastLogon = new SqlParameter()
        //    {
        //        ParameterName = "@LastLogon",
        //        SqlDbType = System.Data.SqlDbType.DateTime,
        //        Direction = System.Data.ParameterDirection.Input,
        //        Value = user.LastLogon
        //    };
        //    var paramToken = new SqlParameter()
        //    {
        //        ParameterName = "@Token",
        //        SqlDbType = System.Data.SqlDbType.VarChar,
        //        Direction = System.Data.ParameterDirection.Input,
        //        Value = user.Token
        //    };
        //    var paramStatementType = new SqlParameter()
        //    {
        //        ParameterName = "@StatementType",
        //        SqlDbType = System.Data.SqlDbType.NVarChar,
        //        Direction = System.Data.ParameterDirection.Input,
        //        Value = ConstantKeys.SPStatementTypeInsert
        //    };
        //    int a = _genericRepository.executeSqlQuery(@"[UserDB].[dbo].[MasterCrudUsersSp] @id, @UserName, @Name,  @SurName,
        //   @Password, @Email, @Phone, @LastLogon, @Token, @StatementType",
        //   new[] {paramId, paramUserName, paramName, paramSurName,  paramPassword,
        //       paramEMail, paramPhone, paramLastLogon, paramToken, paramStatementType });
        //    if (a > 0)
        //    {
        //        return Ok(user);
        //    }
        //    else
        //    {
        //        return BadRequest("Bad Request...");
        //    }
        //}


    }
}
