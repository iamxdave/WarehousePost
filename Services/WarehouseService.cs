using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw5.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace cw5.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IConfiguration _configuration;
        public WarehouseService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<int> OrderRegistration(Order order)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            using(var command = new SqlCommand())
            {

                await connection.OpenAsync();
                var transaction = await connection.BeginTransactionAsync() as SqlTransaction;

                try
                {
                    command.Connection = connection;
                    command.Transaction = transaction;
            
                    command.CommandText = "update \"order\" set fulfilledat = @1 where idorder = @2";
                    command.Parameters.AddWithValue("@1", DateTime.Now);
                    command.Parameters.AddWithValue("@2", order.IdOrder);
                    await command.ExecuteNonQueryAsync();


                    command.Parameters.Clear();
                    command.CommandText = "select price from Product where idproduct = @1";
                    command.Parameters.AddWithValue("@1", order.IdProduct);
                    var price = double.Parse((await command.ExecuteScalarAsync()).ToString());
                    await command.ExecuteNonQueryAsync();

                    command.Parameters.Clear();
                    command.CommandText = "insert into Product_Warehouse values (@1, @2, @3, @4, @5, @6)";
                    command.Parameters.AddWithValue("@1", order.IdWarehouse);
                    command.Parameters.AddWithValue("@2", order.IdProduct);
                    command.Parameters.AddWithValue("@3", order.IdOrder);
                    command.Parameters.AddWithValue("@4", order.Amount);
                    command.Parameters.AddWithValue("@5", order.Amount * price);
                    command.Parameters.AddWithValue("@6", DateTime.Now);
                    await command.ExecuteNonQueryAsync();

                    command.Parameters.Clear();
                    command.CommandText = "select IdOrder from Product_Warehouse WHERE IdProduct = @1 AND IdWarehouse = @2 AND Amount = @3";
                    command.Parameters.AddWithValue("@1", order.IdProduct);
                    command.Parameters.AddWithValue("@2", order.IdWarehouse);
                    command.Parameters.AddWithValue("@3", order.Amount);
                    
                    var id = int.Parse((await command.ExecuteScalarAsync()).ToString());
                    await transaction.CommitAsync();

                    return id;

                }catch(Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                } 
            }
        }
        
        public async Task<bool> DoesProductExist(int productId)
        {

            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            using (var command = new SqlCommand())
            {

                command.Connection = connection;
                command.CommandText = "select 1 from product where idproduct = @1";
                command.Parameters.AddWithValue("@1", productId);

                await connection.OpenAsync();

                var dataReader = await command.ExecuteReaderAsync();

                return dataReader.HasRows;
            }
        }


        public async Task<bool> DoesWarehouseExist(int warehouseId)
        {

            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "select 1 from warehouse where idwarehouse = @1";
                command.Parameters.AddWithValue("@1", warehouseId);

                await connection.OpenAsync();

                var dataReader = await command.ExecuteReaderAsync();

                return dataReader.HasRows;
            }
        }

        public async Task<bool> DoesOrderExist(int productId, int amount, DateTime createdDate)
        {

            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "select createdat from \"order\" where idproduct = @1 and amount = @2";
                command.Parameters.AddWithValue("@1", productId);
                command.Parameters.AddWithValue("@2", amount);
                await connection.OpenAsync();

                DateTime createdAt = (DateTime)await command.ExecuteScalarAsync();
                
                if (createdAt < DateTime.Now)
                {

                    command.CommandText = "select 1 from \"order\" where idproduct = @1 AND amount = @2";

                    var dataReader = await command.ExecuteReaderAsync();
                
                    return dataReader.HasRows;

                } else return false;
            }
        }

        public async Task<bool> HasBeenFullfiled(int orderId)
        {

            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "select 1 from product_warehouse where idorder = @1";
                command.Parameters.AddWithValue("@1", orderId);


                await connection.OpenAsync();

                var dataReader = await command.ExecuteReaderAsync();
                return dataReader.HasRows;
            }
        }


        public async Task<int> GetOrderId(int productId, int amount)
        {

            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "select idorder from \"order\" where idproduct = @1 and amount = @2";
                command.Parameters.AddWithValue("@1", productId);
                command.Parameters.AddWithValue("@2", amount);

                await connection.OpenAsync();
                
                var id = int.Parse((await command.ExecuteScalarAsync()).ToString());
                
                return id;
            }
        }
    }
}