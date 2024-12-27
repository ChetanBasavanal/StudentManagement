using System.Net;

namespace StudentManagement.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                var errorID= Guid.NewGuid();

                //Log exception
                logger.LogError(ex,$"{errorID} : {ex.Message}");
                //Return a custom exception
                httpContext.Response.StatusCode= (int) HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType="application/json";

                //this is custom exception
                var error = new
                {
                    id = errorID,
                    Errormessage = "Something went wrong we are looking into resolving this"
                };

                //paased to user
                await httpContext.Response.WriteAsJsonAsync(error);
                
            }
        }
    }
}
