using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        private readonly IUnitOfWork _unitOfWork;

        public UserController(
          IUnitOfWork unitOfWork,
          ICacheProvider cacheProvider,
          IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _genericRepository = _unitOfWork.GetRepository<User>();
            _cacheProvider = cacheProvider;
            _userService = userService;

        }
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
        //[HttpGet]
        //[Route("get-all-users")]
        //public IQueryable<User> GetAllUsers()
        //{
        //    return _genericRepository.GetAll();
        //}


        [HttpPost("Add")]
        public IActionResult AddUser([FromBody] User user)
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


    }
}
