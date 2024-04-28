using cw3.Models;
using cw3.Models.Dto;

namespace cw3.Services.Abstractions;

public interface IAnimalService
{
    Task<ICollection<Animal>> GetAnimals(string? orderBy);
    Task<Animal> CreateAnimal(AnimalDto dto);
    Task<Animal?> GetById(int id);
    Task<Animal> Update(int id, UpdateDto dto);
    Task Delete(int id);
    Task<bool> Exists(int id);

}