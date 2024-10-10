using KALS.Domain.Entities;

namespace KALS.Repository.Interface;

public interface IUserRepository: IGenericRepository<User>
{
    Task<User> GetUserByIdAsync(Guid id);
    
    Task<User> GetUserByUsernameOrPhoneNumber(string usernameOrPhoneNumber);
    
    Task<ICollection<User>> GetAllUsers();
    Task<User> GetUserByPhoneNumber(string phoneNumber);
}