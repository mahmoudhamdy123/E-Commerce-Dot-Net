using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Interfaces;
using Infastructure;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }


        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string baskedId)
        {
            var basket = await _basketRepository.GetBasketAsync(baskedId);
            return Ok(basket ?? new CustomerBasket(baskedId));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasketById(CustomerBasket basket)
        {
            var updateBasket = await _basketRepository.UpdateBaskedAsync(basket);
            return Ok(updateBasket);
        }


        [HttpDelete]
        public async Task<ActionResult<CustomerBasket>> DeleteBasketById(string baskedId)
        {
            var deleted = await _basketRepository.DeleteBaskedAsync(baskedId);
            return Ok(deleted);
        }


    }
}