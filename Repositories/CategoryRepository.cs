using Blog.Data;
using Blog.Interfaces;
using Blog.Models;
using TapiFramework.Repositories;
using TapiFramework.Repositories.Interfaces;

namespace Blog.Repositories;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(BlogDataContext context) : base(context)
    {
    }
}