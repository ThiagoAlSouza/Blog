using System.Collections.Generic;

namespace Blog.Models
{
    public class Role
    {
        #region Properties

        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }

        public IList<User> Users { get; set; }

        #endregion
    }
}