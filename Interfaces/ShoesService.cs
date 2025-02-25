using Microsoft.AspNetCore.Mvc;
using firstProject.Models;

namespace firstProject.Interfaces;

public interface IShoesService
{
    List<Shoes> Get();

    Shoes Get(int code);

    int Insert(Shoes newShoes);

    bool Update(int Code, Shoes newShoes);

    bool Delete(int Code);
}