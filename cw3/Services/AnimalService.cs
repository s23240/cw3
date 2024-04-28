using System.Data.SqlClient;
using cw3.Models;
using cw3.Models.Dto;
using cw3.Services.Abstractions;

namespace cw3.Services;

public class AnimalService : IAnimalService
{
    private readonly IConfiguration _configuration;

    public AnimalService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<ICollection<Animal>> GetAnimals(string? orderBy)
    {
        orderBy ??= "name";

        switch (orderBy)
        {
            case "name": break;
            case "description": break;
            case "category": break;
            case "area": break;
            default: orderBy = "name"; break;
        }
        
        var results = new List<Animal>();
        
        using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            var command = connection.CreateCommand();
            command.CommandText = $"select * from animal order by {orderBy}";
            await connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())    
            {
                results.Add(new Animal
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.GetValue(2) == DBNull.Value ? null : reader.GetString(2),
                    Category = reader.GetString(3),
                    Area = reader.GetString(4),
                });
            }
        }

        return results;
    }

    public async Task<Animal> CreateAnimal(AnimalDto dto)
    {
        using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            var command = connection.CreateCommand();
            command.CommandText =
                $"insert into animal (id, name, description, category, area) values (@1, @2, @3, @4, @5)";
            command.Parameters.AddWithValue("@1", dto.Id);
            command.Parameters.AddWithValue("@2", dto.Name);
            command.Parameters.AddWithValue("@3", dto.Description);
            command.Parameters.AddWithValue("@4", dto.Category);
            command.Parameters.AddWithValue("@5", dto.Area);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        var result = await GetById(dto.Id);
        
        return result!;
    }

    public async Task<Animal?> GetById(int id)
    {
        using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            var command = connection.CreateCommand();
            command.CommandText = $"select id, name, description, category, area from animal where id = @1";
            command.Parameters.AddWithValue("@1", id);
            await connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();

            await reader.ReadAsync();

            return new Animal
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetValue(2) == DBNull.Value ? null : reader.GetString(2),
                Category = reader.GetString(3),
                Area = reader.GetString(4),
            };
        }
    }

    public async Task<Animal> Update(int id, UpdateDto dto)
    {
        using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            var command = connection.CreateCommand();
            command.CommandText = $"update animal set name = @2, description = @3, category = @4, area = @5 where id = @1";
            command.Parameters.AddWithValue("@1", id);
            command.Parameters.AddWithValue("@2", dto.Name);
            command.Parameters.AddWithValue("@3", dto.Description);
            command.Parameters.AddWithValue("@4", dto.Category);
            command.Parameters.AddWithValue("@5", dto.Area);
            await connection.OpenAsync();

            await command.ExecuteNonQueryAsync();

            return (await GetById(id))!;
        }
    }

    public async Task Delete(int id)
    {
        using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            var command = connection.CreateCommand();
            command.CommandText = $"delete from animal where id = @1";
            command.Parameters.AddWithValue("@1", id);
            await connection.OpenAsync();

            await command.ExecuteNonQueryAsync();
        }
    }
    
    public async Task<bool> Exists(int id)
    {
        using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            var command = connection.CreateCommand();
            command.CommandText = $"select id from animal where id = @1";
            command.Parameters.AddWithValue("@1", id);
            await connection.OpenAsync();
            if (await command.ExecuteScalarAsync() is not null)
            {
                return true;
            }
            return false;
        }
    }
}
