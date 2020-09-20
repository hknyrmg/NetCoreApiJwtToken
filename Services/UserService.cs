using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TokenBasedAuth_NetCore.Entities;
using TokenBasedAuth_NetCore.Models;
using TokenBasedAuth_NetCore.Providers;
using TokenBasedAuth_NetCore.Repository;
using TokenBasedAuth_NetCore.UnitofWork;
using TokenBasedAuth_NetCore.Utilities.Helper;
using TokenBasedAuth_NetCore.Utils;

namespace TokenBasedAuth_NetCore.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;

        private readonly IGenericRepository<User> _genericRepository;
        private readonly ICacheProvider _cacheProvider;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork,
          ICacheProvider cacheProvider, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _unitOfWork = unitOfWork;
            _genericRepository = _unitOfWork.GetRepository<User>();
            _cacheProvider = cacheProvider;
        }
        public TokenModel Authenticate(Entities.LoginModel loginModel)
        {
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
            var user = _genericRepository.getEntityFromQuery("GetUser @UserName, @Password",
                 new[] { paramUserName, paramPassword }).FirstOrDefault();
            if (user == null) return null;

            //            var token = JwtGenerator.createJwtToken(user);
            var token = generateJwtToken(user);

            return new TokenModel(user, token);
            //return _genericRepository.getEntityFromQuery("GetUser @UserName, @Password",
            //     new[] { paramUserName, paramPassword }).FirstOrDefault();


        }
        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public IEnumerable<User> GetAll()
        {
            return _genericRepository.GetAll(); 
        }
        public User GetById(int id)
        {
            return _genericRepository.Find(id);
        }
    }
}
