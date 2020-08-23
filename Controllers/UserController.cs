using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TokenBasedAuth_NetCore.Entities;
using TokenBasedAuth_NetCore.Providers;
using TokenBasedAuth_NetCore.Repository;
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
        private readonly IUnitOfWork _unitOfWork;

        public UserController(
          IUnitOfWork unitOfWork,
          ICacheProvider cacheProvider)
        {
            _unitOfWork = unitOfWork;
            _genericRepository = _unitOfWork.GetRepository<User>();
            _cacheProvider = cacheProvider;

        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet]
        [Route("{id}")]
        public ActionResult<User> Get(int id)
        {
            return _genericRepository.Find(id);
        }
        [HttpGet]
        [Route("get-all-users")]
        public IQueryable<User> GetAllUsers()
        {
            return _genericRepository.GetAll();
        }
        // POST api/values
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}
        [HttpPost]
        public ActionResult Post([FromBody] string value)
        {
            return Ok();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost("Add")]
        public User AddUser([FromBody] User user)
        {
            user.Password = HashUtil.GetSha256FromString(user.Password);

            _genericRepository.Add(user);
            _unitOfWork.SaveChanges();
            return user;
        }

        [HttpPost("GetUser")]
        public User GetUser([FromBody] LoginModel loginModel)
        {

            //or
            var paramUserName = new SqlParameter()
            {
                ParameterName = "@UserName",
                SqlDbType = System.Data.SqlDbType.VarChar,
                Direction = System.Data.ParameterDirection.Input,
                Size = int.MaxValue,
                Value = loginModel.UserName
            };
            var paramPassword = new SqlParameter()
            {
                ParameterName = "@Password",
                SqlDbType = System.Data.SqlDbType.VarChar,
                Direction = System.Data.ParameterDirection.Input,
                Size = int.MaxValue,
                Value = HashUtil.GetSha256FromString(loginModel.Password)
            };
            return _genericRepository.getEntityFromQuery("GetUser @UserName, @Password",
                 new[] { paramUserName, paramPassword }).FirstOrDefault();
        }
    }
}
