using Microsoft.AspNetCore.Mvc;
using firstProject.Models;
using firstProject.Interfaces;
using System.Text.Json;

namespace firstProject.Services;

public class ShoesServiceConst : IService<Shoes>
{
    List<Shoes> shoes { get; }
    private static string fileName="shoes.json";
    private string filePath;
    public ShoesServiceConst(IHostEnvironment env)
    {
        filePath=Path.Combine(env.ContentRootPath,"Data",fileName);
        using(var jsonFile =File.OpenText(filePath))
        {
            shoes=JsonSerializer.Deserialize<List<Shoes>>(jsonFile.ReadToEnd(),new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive=true
            });
        }

    }
    private void saveToFile(){
        File.WriteAllText(filePath,JsonSerializer.Serialize(shoes));
    }

    public List<Shoes> GetAll()=>shoes;

    public Shoes Get(int code) =>shoes.FirstOrDefault(s=>s.Code==code);

    public void Insert(Shoes newShoes)
    {
        if (newShoes == null)
            return;
        int maxCode = shoes.Max(s => s.Code);
        newShoes.Code = maxCode + 1;
        shoes.Add(newShoes);
        saveToFile();
    }


    public void Update( Shoes newShoes)
    {
        if (newShoes == null )
        {
            return;
        }

        var shoe = shoes.FirstOrDefault(s => s.Code == newShoes.Code);
        if (shoe == null)
            return;

        shoe.Size = newShoes.Size;
        shoe.Company = newShoes.Company;
        shoe.Color = newShoes.Color;
        saveToFile();
    }

    public void Delete(int Code)
    {
        var shoe =Get(Code);
        if (shoes is null)
            return;
        shoes.Remove(shoe);
        saveToFile();
    }


    // public int Count =>shoes.Count();
}

public static class ShoeUtilities
{
    public static void AddShoeConst(this IServiceCollection services)
    {
        services.AddSingleton<IService<Shoes>, ShoesServiceConst>();
    }
}