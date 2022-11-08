using Wheels.WebApi.Middlewares;
using Wheels.WebApi.ModelAPI;
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
    var networks = repository.GetAll();
    var networksDtos = networks.Select(n => Transformers.ToSharedCarNetworkDTO(n));

    return networksDtos;
});

app.MapGet(
    "/networks/{id}", (string id, SharedCarNetworkMockRepository repository) =>
{
    var network = repository.GetById(id);
    if (network != null)
    {
        return Transformers.ToSharedCarNetworkDTO(network);
    }

    throw new PresentationException("network not found", ErrorCode.NotFound);
});

app.Run();
