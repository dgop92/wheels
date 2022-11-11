using Microsoft.AspNetCore.Mvc;
using Wheels.Domain.NetworkAlgorithms;
using Wheels.Domain.Dtos;
using Wheels.Domain.Repository;
using Wheels.Domain.UseCases;
using Wheels.Domain.Services;
using Wheels.Infrastructure.UseCases;
using Wheels.Infrastructure.Repository;
using Wheels.WebApi.Middlewares;
using Wheels.WebApi.ModelAPI;
using SharedKernel.Errors;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<
    IPathService,
    EuclideanPathService
>();
builder.Services.AddScoped<
    ISharedCarNetworkRepository,
    SharedCarNetworkMockRepository
>();
builder.Services.AddScoped<SharedCarNetworkReComputer>(x => new
    SharedCarNetworkReComputer(
        x.GetRequiredService<IPathService>(),
        x.GetRequiredService<IPathService>()
    )
);
builder.Services.AddScoped<
    ISharedCarNetworkUseCase,
    SharedCarNetworkUseCase
>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        await ExceptionMiddleware.HandleException(context);
    });
});

app.MapGet("/networks", async (ISharedCarNetworkUseCase useCase) =>
{
    var networks = await useCase.GetAll();
    var networksDtos = networks.Select(n => Transformers.ToSharedCarNetworkDTO(n));

    return networksDtos;
});

app.MapGet(
    "/networks/{id}", async (string id, ISharedCarNetworkUseCase useCase) =>
{
    var network = await useCase.GetById(id);
    if (network != null)
    {
        return Transformers.ToSharedCarNetworkDTO(network);
    }

    throw new PresentationException("network not found", ErrorCode.NotFound);
});

app.MapPost("/network-add", async (
    [FromBody] ReComputeNetworkDTO reComputeNetworkDTO,
    ISharedCarNetworkUseCase useCase) =>
{
    var newNetwork = await useCase.AddPassengerToNetwork(reComputeNetworkDTO);
    var networkDto = Transformers.ToSharedCarNetworkDTO(newNetwork);

    return networkDto;
});

app.Run();
