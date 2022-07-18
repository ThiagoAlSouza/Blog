namespace Blog.Models;

public class Tag
{
    #region Properties

    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }

    public List<Post> Posts { get; set; }

    #endregion
}