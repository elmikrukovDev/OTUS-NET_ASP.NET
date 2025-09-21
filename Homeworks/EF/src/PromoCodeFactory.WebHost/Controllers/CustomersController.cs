using Castle.Core.Resource;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Repositories;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Клиенты
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController
        : ControllerBase
    {
        private readonly IRepository<Customer> _customerRepository;

        public CustomersController(ICustomerRepository repository)
        {
            _customerRepository = repository;
        }

        /// <summary>
        /// Получить всех покупателей
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<CustomerShortResponse>> GetCustomersAsync()
        {
            try
            {
                var customers = await _customerRepository.GetAllAsync();

                var customerModelList = customers.Select(c =>
                    new CustomerResponse()
                    {
                        Id = c.Id,
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        Email = c.Email,
                        Preferences = c.CustomerPreferences.Select(cp => 
                            new PreferenceResponse()
                            {
                                Id = cp.PreferenceId,
                                Description = cp.Preference.Description
                            }).ToList(),
                        PromoCodes = c.PromoCodes.Select(p =>
                            new PromoCodeShortResponse()
                            {
                                Id = p.Id,
                                ServiceInfo = p.ServiceInfo,
                                Code = p.Code,
                                BeginDate = p.BeginDate.ToShortDateString(),
                                EndDate = p.EndDate.ToShortDateString(),
                                PartnerName = p.PartnerName
                            }).ToList()
                    }).ToList();

                return customerModelList.Count == 0
                    ? NoContent()
                    : Ok(customerModelList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Получить покупателя по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetCustomerById")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(id);

                if (customer == null)
                    return NotFound();

                var customerModel = new CustomerResponse()
                {
                    Id = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    Preferences = customer.CustomerPreferences.Select(cp =>
                            new PreferenceResponse()
                            {
                                Id = cp.PreferenceId,
                                Description = cp.Preference.Description
                            }).ToList(),
                    PromoCodes = customer.PromoCodes.Select(p =>
                        new PromoCodeShortResponse()
                        {
                            Id = p.Id,
                            ServiceInfo = p.ServiceInfo,
                            Code = p.Code,
                            BeginDate = p.BeginDate.ToShortDateString(),
                            EndDate = p.EndDate.ToShortDateString(),
                            PartnerName = p.PartnerName
                        }).ToList()
                };

                return customerModel;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Создать покупателя
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                Guid customerId = Guid.NewGuid();
                var customer = new Customer()
                {
                    Id = customerId,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    CustomerPreferences = request.PreferenceIds.Select(pId =>
                        new CustomerPreference()
                        {
                            CustomerId = customerId,
                            PreferenceId = pId
                        }).ToList()
                };

                customer = await _customerRepository.CreateAsync(customer);

                var response = new CustomerShortResponse()
                {
                    Id = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email
                };

                return CreatedAtRoute("GetCustomerById", new { id = customer.Id }, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Обновить данные покупателя
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingCustomer = await _customerRepository.GetByIdAsync(id);
                if (existingCustomer == null)
                    return NotFound(0);

                var updatedCustomer = new Customer()
                {
                    Id = id,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    CustomerPreferences = request.PreferenceIds.Select(pId =>
                        new CustomerPreference()
                        {
                            CustomerId = id,
                            PreferenceId = pId
                        }).ToList(),
                    PromoCodes = existingCustomer.PromoCodes
                };

                updatedCustomer = await _customerRepository.UpdateAsync(updatedCustomer);

                var response = new CustomerShortResponse()
                {
                    Id = updatedCustomer.Id,
                    FirstName = updatedCustomer.FirstName,
                    LastName = updatedCustomer.LastName,
                    Email = updatedCustomer.Email
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Удалить покупателя по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(id);
                if (customer == null)
                    return NotFound();

                var result = await _customerRepository.DeleteAsync(id);
                if (result)
                    return Ok(result);
                return StatusCode(500, "Failed to delete customer");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}