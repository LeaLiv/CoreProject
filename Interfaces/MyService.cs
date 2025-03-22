
namespace firstProject.Interfaces;

public interface IService<T>
{
    List<T> GetAll();

    T Get(int Id);

    void Insert(T newItem);

    void Update(T newItem);

    void Delete(int Id);
}