using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels;

public class RegisterUserViewModel
{
    [Required]
    public string Name { get; set; }
    [Required]
    [EmailAddress]
    public string email { get; set; }
    [Required]
    public string senha { get; set; }
}
