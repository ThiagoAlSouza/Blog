using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Blog.Extensions;

public static class ModelStateExtension
{
    public static List<string> GetErrors(this ModelStateDictionary modelState)
    {
        List<string> results = new List<string>();

        foreach (var item in modelState.Values)
            results.AddRange(item.Errors.Select(x => x.ErrorMessage));

        return results;
    }
}
