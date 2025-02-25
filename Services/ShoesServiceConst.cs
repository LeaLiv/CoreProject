using Microsoft.AspNetCore.Mvc;
using firstProject.Models;
using firstProject.Interfaces;

namespace firstProject.Services;

public class ShoesServiceConst:IShoesService
{
    private List<Shoes> list;
    public ShoesServiceConst()
    {
        list = new List<Shoes>
        {
            new Shoes{ Code=1, Size=38 ,Company="Nike"  ,Color="black" },
            new Shoes{ Code=2, Size=35 ,Company="Adidas",Color="pink" },
            new Shoes{ Code=3, Size=45 ,Company="Nike" ,Color="Grey" }
        };
    }

    public List<Shoes> Get()
    {
        return list;
    }

    public Shoes Get(int code)
    {
        var shoes = list.FirstOrDefault(s => s.Code == code);
        return shoes;
    }

    public int Insert(Shoes newShoes)
    {
        if (newShoes == null)
            return -1;

        int maxCode = list.Max(s => s.Code);
        newShoes.Code = maxCode + 1;
        list.Add(newShoes);

        return newShoes.Code;
    }


    public bool Update(int Code, Shoes newShoes)
    {
        if (newShoes == null || newShoes.Code != Code)
        {
            return false;
        }

        var shoes = list.FirstOrDefault(s => s.Code == Code);
        if (shoes == null)
            return false;

        shoes.Size = newShoes.Size;
        shoes.Company = newShoes.Company;
        shoes.Color = newShoes.Color;
        return true;
    }

    public bool Delete(int Code)
    {
        var shoes = list.FirstOrDefault(s=>s.Code==Code);
        if (shoes == null)
            return false;

        var index = list.IndexOf(shoes);
        list.RemoveAt(index);

        return true;
    }
}

public static class ShoeUtilities
{
    public static void AddShoeConst(this IServiceCollection services)
    {
        services.AddSingleton<IShoesService,ShoesServiceConst>();
    }
}