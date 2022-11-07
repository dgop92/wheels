using Wheels.WebApi.Middlewares;
using Wheels.Infrastructure.Repository;
using SharedKernel.Errors;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<SharedCarNetworkMockRepository>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        await ExceptionMiddleware.HandleException(context);
    });
});

app.MapGet("/networks", (SharedCarNetworkMockRepository repository) =>
{
    return repository.GetAll();
});

app.MapGet(
    "/networks/{id}", (string id, SharedCarNetworkMockRepository repository) =>
{
    var network = repository.GetById(id);
    if (network != null)
    {
        return network;
    }

    throw new PresentationException("network not found", ErrorCode.NotFound);
});

app.Run();
