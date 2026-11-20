using AppDev_MidtermExam_.Models;

namespace AppDev_MidtermExam_.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Products>> GetAllAsync();
        Task<Products?> GetByIdAsync(int id);

        Task<int> CreateAsync(Products product);
    }
}
