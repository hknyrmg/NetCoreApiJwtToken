using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TokenBasedAuth_NetCore.Entities;
using TokenBasedAuth_NetCore.Models;
using TokenBasedAuth_NetCore.Providers;
using TokenBasedAuth_NetCore.Repository;
using TokenBasedAuth_NetCore.UnitofWork;
using TokenBasedAuth_NetCore.Utils;
using Dapper;
using System.Data;

namespace TokenBasedAuth_NetCore.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;

        private readonly IGenericRepository<User> _genericRepository;
        private readonly ICacheProvider _cacheProvider;
        private readonly IDapperService _dapperService;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork,
          ICacheProvider cacheProvider, IOptions<AppSettings> appSettings,
           IDapperService dapperService)
        {
            _appSettings = appSettings.Value;
            _unitOfWork = unitOfWork;
            _genericRepository = _unitOfWork.GetRepository<User>();
            _cacheProvider = cacheProvider;
            _dapperService = dapperService;
        }
        public TokenModel Authenticate(Entities.LoginModel loginModel)
        {
            //var paramUserName = new SqlParameter()
            //{
            //    ParameterName = "@UserName",
            //    SqlDbType = System.Data.SqlDbType.VarChar,
            //    Direction = System.Data.ParameterDirection.Input,
            //    Size = int.MaxValue,
            //    Value = loginModel.UserName
            //};
            //var paramPassword = new SqlParameter()
            //{
            //    ParameterName = "@Password",
            //    SqlDbType = System.Data.SqlDbType.VarChar,
            //    Direction = System.Data.ParameterDirection.Input,
            //    Size = int.MaxValue,
            //    Value = HashUtil.GetSha256FromString(loginModel.Password)
            //};
            //var user = _genericRepository.getEntityFromQuery("GetUser @UserName, @Password",
            //     new[] { paramUserName, paramPassword }).FirstOrDefault();


            var dbparams = new DynamicParameters();
            dbparams.Add("UserName", loginModel.UserName, DbType.String);
            dbparams.Add("Password", HashUtil.GetSha256FromString(loginModel.Password), DbType.String);
     
            var user = _dapperService.Get<User>("GetUser"
                , dbparams,
                commandType: CommandType.StoredProcedure);


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
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.Trim())}),
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
