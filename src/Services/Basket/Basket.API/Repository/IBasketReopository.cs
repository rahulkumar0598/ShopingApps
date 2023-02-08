using Basket.API.Entities;

namespace Basket.API.Repository
{
    public interface IBasketReopository
    {
        Task<ShoppingCart> GetBasket(string userName);
        Task<ShoppingCart> UpdateBasket(ShoppingCart basket);
        Task DeleteBasket(string userName);

    }
}
