using System.ComponentModel.DataAnnotations;

namespace ToDoListService.Validators;

/// <summary>
///     Checks that one of fields from request body matches data provided in route
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
public class MatchesRouteValueAttribute : ValidationAttribute
{
    public string ControllerName { get; set; }
    public string ActionName { get; set; }
    public string RouteFieldName { get; set; }

    public MatchesRouteValueAttribute(string controllerName, string actionName, string routeFieldName)
    {
        ControllerName = controllerName;
        ActionName = actionName;
        RouteFieldName = routeFieldName;
        
        var defaultErrorMessage = "The field {0} does not match route value of " + RouteFieldName;
        ErrorMessage ??= defaultErrorMessage;
    }


    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var httpContextAccessor = validationContext.GetRequiredService<IHttpContextAccessor>();
        var routeValuesDict = httpContextAccessor.HttpContext?.Request.RouteValues;
        var controllerName = routeValuesDict?["controller"]?.ToString();
        var actionName = routeValuesDict?["action"]?.ToString();
        var idFromRoute = routeValuesDict?[RouteFieldName]?.ToString();
        if (!ControllerName.Equals(controllerName) || !ActionName.Equals(actionName) || idFromRoute == null)
            return ValidationResult.Success;
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            return new ValidationResult($"{validationContext.DisplayName} is required.");

        return !idFromRoute.Equals(value.ToString())
            ? new ValidationResult(FormatErrorMessage(validationContext.DisplayName))
            : ValidationResult.Success;
    }
}