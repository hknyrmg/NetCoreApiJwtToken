using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TokenBasedAuth_NetCore.Entities;
using TokenBasedAuth_NetCore.Providers;
using TokenBasedAuth_NetCore.Repository;
using TokenBasedAuth_NetCore.UnitofWork;
using TokenBasedAuth_NetCore.Utils;

namespace TokenBasedAuth_NetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly IGenericRepository<Customer> _genericRepository;
        private readonly ICacheProvider _cacheProvider;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerController(
          IUnitOfWork unitOfWork,
          ICacheProvider cacheProvider)
        {
            _unitOfWork = unitOfWork;
            _genericRepository = _unitOfWork.GetRepository<Customer>();
            _cacheProvider = cacheProvider;

        }
        [HttpGet]
        public List<Customer> GetAll()
        {
            List<Customer> customers;


            List<Customer> customerList = _cacheProvider.GetFromCache<List<Customer>>(CacheKeys.customerList);
            if (customerList != null)
            {
                return customerList;
            }

            customers = _unitOfWork.GetRepository<Customer>().GetAll().ToList();
            _cacheProvider.SetCache<List<Customer>>(CacheKeys.customerList, customers, DateTimeOffset.Now.AddDays(1));

            return customers;
        }
        [HttpDelete("{id}")]
        public void Delete()
        {
            _genericRepository.Delete(3);
            _unitOfWork.SaveChanges();
        }
        [HttpPost("Add")]
        public Customer AddCustomer([FromBody] Customer customer)
        {
            _genericRepository.Add(customer);
            _unitOfWork.SaveChanges();
            return customer;
        }

        
    }
}