using Blog.Models;

namespace Blog.Interfaces;

public interface IRepositoryCategory
{
    #region Methods

    Task<IEnumerable<Category>> GetAll();
    Task<Category> GetById(int id);
    Task Save(Category category);
    Task Update(Category category);
    Task Delete(int id);

    #endregion
}