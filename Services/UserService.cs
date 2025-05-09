using firstProject.Interfaces;
using firstProject.Models;
using System.Text.Json;

namespace firstProject.Services;

public class UserService : IUserService
{
    List<User> users { get; set;}

    private static string fileName = "users.json";
    private string filePath;
    public UserService(IHostEnvironment env)
    {
        filePath = Path.Combine(env.ContentRootPath, "Data", fileName);
        loadUsers();

    }
private void loadUsers(){
    using (var jsonFile = File.OpenText(filePath))
        {
            users = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd()
            , new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
}

    private async void saveToFile()
    {
        File.WriteAllText(filePath, JsonSerializer.Serialize(users));
    }

    public List<User> GetAll() => users;

    public User Get(int id)
    {
        loadUsers();
        User user = users.FirstOrDefault<User>(s => s.Id == id);
        // System.Console.WriteLine(users.for);
        // foreach (var item in users)
        // {
        //     System.Console.WriteLine(item.userName);
        // }
        Console.WriteLine(user.userName);
        return user;
    }


    public async void Insert(User newUser)
    {
        if (newUser == null)
            return;
        int maxId = users.Max(s => s.Id);
        newUser.Id = maxId + 1;
        users.Add(newUser);

        saveToFile();
        // foreach (var item in users)
        // {
        //     System.Console.WriteLine($"{item.Id}: {item.userName}");
        // }
    }

    public void Update(User newUser)
    {
        if (newUser == null)
        {
            return;
        }
        var user = users.FirstOrDefault(s => s.Id == newUser.Id);
        if (user == null)
            return;
        user.Name = newUser.Name;
        user.Email = newUser.Email;
        user.Role = newUser.Role;
        saveToFile();
    }

    public void Delete(int Id)
    {
        var user = Get(Id);
        if (user == null)
            return;
        users.Remove(user);
        saveToFile();
    }
    public User GetByuserName(string userName)
    {
        return users.FirstOrDefault(s => s.userName == userName);
    }
}
public static class UserUtilities
{
    public static void AddUserConst(this IServiceCollection services)
    {
        services.AddSingleton<IUserService, UserService>();
    }
}
