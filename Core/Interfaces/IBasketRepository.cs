
namespace Core.Interfaces
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(string baskedId);
        Task<CustomerBasket> UpdateBaskedAsync(CustomerBasket basked);
        Task<bool> DeleteBaskedAsync(string baskedId);
    }
}