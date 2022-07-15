using System.Collections.Generic;

namespace Blog.Models
{
    public class Category
    {
        #region Properties

        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        
        public IList<Post> Posts { get; set; }

        #endregion
    }
}