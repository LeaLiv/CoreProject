using Microsoft.AspNetCore.Mvc;
using firstProject.Models;

namespace firstProject.Interfaces;

public interface IShoesService
{
    List<Shoes> GetAll();

    Shoes Get(int code);

    void Insert(Shoes newShoes);

    void Update(Shoes newShoes);

    void Delete(int Code);
    int Count { get; }
}