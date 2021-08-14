using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenBasedAuth_NetCore.Entities.Trivia;
using TokenBasedAuth_NetCore.Models;
using TokenBasedAuth_NetCore.Providers;
using TokenBasedAuth_NetCore.Repository;
using TokenBasedAuth_NetCore.UnitofWork;

namespace TokenBasedAuth_NetCore.Services.Trivia
{
    public class TriviaService : ITriviaService
    {
        private readonly AppSettings _appSettings;

        private readonly IGenericRepository<Category> _genericRepository;
        private readonly ICacheProvider _cacheProvider;
        private readonly IUnitOfWork _unitOfWork;
        public TriviaService(IUnitOfWork unitOfWork,
          ICacheProvider cacheProvider, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _unitOfWork = unitOfWork;
            _genericRepository = _unitOfWork.GetRepository<Category>();
            _cacheProvider = cacheProvider;
        }
        public IEnumerable<Category> GetAll()
        {
            //return _genericRepository.GetAll();
            return _genericRepository.getEntityFromQuery("GetTriviaCategories");
        }
    }
}
