using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TokenBasedAuth_NetCore.Entities;
using TokenBasedAuth_NetCore.Providers;
using TokenBasedAuth_NetCore.Repository;
using TokenBasedAuth_NetCore.Services;
using TokenBasedAuth_NetCore.UnitofWork;
using TokenBasedAuth_NetCore.Utils;

namespace TokenBasedAuth_NetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public IActionResult Authenticate([FromBody] LoginModel loginModel)
        {
            var response = _userService.Authenticate(loginModel);
            if (response == null)
                return BadRequest("Username or password is wrong.");

            return Ok(response);

        }
        [Authorize]
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
        //[HttpGet]
        //[Route("get-all-users")]
        //public IQueryable<User> GetAllUsers()
        //{
        //    return _genericRepository.GetAll();
        //}


        [HttpPost("Add")]
        public User AddUser([FromBody] User user)
        {
            user.Password = HashUtil.GetSha256FromString(user.Password);

            _genericRepository.Add(user);
            _unitOfWork.SaveChanges();
            return user;
        }


    }
}
