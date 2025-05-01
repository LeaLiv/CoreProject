using firstProject.Interfaces;
using firstProject.Models;
using System.Text.Json;

namespace firstProject.Services;

public class UserService:IUserService
{
    List<User> users { get; }

    private static string fileName="users.json";
    private string filePath;
    public UserService(IHostEnvironment env)
    {
        filePath=Path.Combine(env.ContentRootPath,"Data",fileName);
        using(var jsonFile =File.OpenText(filePath))
        {
            users = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive=true
            });
        }

    }


    private void saveToFile(){
        File.WriteAllText(filePath,JsonSerializer.Serialize(users));
    }

    public List<User> GetAll()=>users;

    public User Get(int id) =>users.FirstOrDefault(s=>s.Id==id);

    public void Insert(User newUser)
    {
        if (newUser == null)
            return;
        int maxId = users.Max(s => s.Id);
        newUser.Id = maxId + 1;
        users.Add(newUser);
        saveToFile();
    }

    public void Update(User newUser)
    {
        if(newUser==null)
        {
            return;
        }
        var user=users.FirstOrDefault(s=>s.Id==newUser.Id);
        if(user==null)
            return;
        user.Name=newUser.Name;
        user.Email=newUser.Email;   
        user.Role=newUser.Role;
        saveToFile();
    }

    public void Delete(int Id)
    {
       var user=Get(Id);
         if(user==null)
                return;
          users.Remove(user);
          saveToFile();
    }
    public User GetByUserName(string userName){
        return users.FirstOrDefault(s=>s.UserNAme==userName);
    }
}
public static class UserUtilities
{
    public static void AddUserConst(this IServiceCollection services)
    {
        services.AddSingleton<IUserService,UserService>();
    }
}
