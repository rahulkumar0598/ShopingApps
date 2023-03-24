using Basket.API.Entities;
using Basket.API.gRPCServices;
using Basket.API.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class BasketController:ControllerBase
    {
        private readonly IBasketReopository _basketReopository;
        private readonly DiscountgRPCService _discountgRPCService;

        public BasketController(IBasketReopository basketReopository,DiscountgRPCService discountgRPCService)
        {
            _basketReopository = basketReopository;
            _discountgRPCService = discountgRPCService;
        }
        [HttpGet("{userName}",Name ="GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket=await _basketReopository.GetBasket(userName);
            return Ok(basket?? new ShoppingCart(userName));
        }
        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart shoppingCart)
        {
            //TODO: communicate with discount gRPC
            //and calculate latest price of the product into shopping cart
            //consume Discount gRPC
            foreach (var item in shoppingCart.Items)
            {
                //item.ProductName;
                var coupon = await _discountgRPCService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }
            return Ok(await _basketReopository.UpdateBasket(shoppingCart));
        }

        [HttpDelete("{userName}",Name ="DeleteBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _basketReopository.DeleteBasket(userName);
            return Ok();
        }

    }
}
