using TapiFramework.Entities.Interfaces;

namespace Blog.Models;

public class Category : IBaseEntity
{
    #region Properties

    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }

    public IList<Post> Posts { get; set; }

    #endregion
}
