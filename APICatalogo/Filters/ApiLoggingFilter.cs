using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalogo.Filters;

public class ApiLoggingFilter : IActionFilter
{
    private readonly ILogger<ApiLoggingFilter> _logger;
    public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
    {
        _logger = logger;
    }

    //Executa antes da ação ser executada
    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("### Executando -> OnActionExecuting");
        _logger.LogInformation("###########################################");
        _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation($"ModelState : {context.ModelState.IsValid}");
        _logger.LogInformation("###########################################");
    }

    //Executa depois que a ação é executada
    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("### Executando -> OnActionExecuted");
        _logger.LogInformation("###########################################");
        _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation($"StatusCode : {context.HttpContext.Response.StatusCode}");
        _logger.LogInformation("###########################################");
    }

}
