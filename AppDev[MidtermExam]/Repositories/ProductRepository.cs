using AppDev_MidtermExam_.DTO;
using AppDev_MidtermExam_.Models;
using AppDev_MidtermExam_.Properties.Data;
using AppDev_MidtermExam_.Repositories.Interfaces;
using Dapper;

namespace AppDev_MidtermExam_.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public readonly DatabaseHelper _db;

        public ProductRepository(DatabaseHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Products>> GetAllAsync()
        {
            using var connection = _db.CreateConnection();

            var products = await connection.QueryAsync<Products>(
                "SELECT * FROM Products"
            );

            return products;
        }

        public async Task<Products?> GetByIdAsync(int id)
        {
            using var connection = _db.CreateConnection();

            var product = await connection.QueryFirstOrDefaultAsync<Products>(
                    "SELECT * FROM Products WHERE Id = @Id",
                    new { Id = id }
                );

            return product;
        }

        public async Task<int> CreateAsync(Products product)
        {
            using var connection = _db.CreateConnection();

            var rows = await connection.ExecuteAsync(
                    @"INSERT INTO Products
                    (ProductId, ProductName, ProductPrice, ProductQuantity, ProductCapabilities)
                    VALUES
                    (@ProductId, @ProductName, @ProductPrice, @ProductQuantity, @ProductCapabilities)",
                    product
                );
            return rows;
        }
    }
}
