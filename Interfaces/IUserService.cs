using firstProject.Models;

namespace firstProject.Interfaces;

public interface IUserService:IService<User>{
    public User GetByuserName(string userName);
    
}