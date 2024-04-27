namespace DotNet7.Template.Api.Middlewares
{
    public class ExampleMiddleware
    {
        private readonly RequestDelegate _next;

        public ExampleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // 在處理請求「前」要先執行的程式

            await _next(context);

            // 在處理請求「後」要先執行的程式
        }
    }
}
