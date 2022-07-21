using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Accounts;

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
