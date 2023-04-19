using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using FunnyTrees.WebApi.DI;
using System.Globalization;
using System.Reflection;

const string AppVersion = "v1";

var builder = WebApplication.CreateBuilder(args);

/* Configure Autofac DI */
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>((ctx, cb) =>
{
    var connectionString = ctx.Configuration.GetConnectionString("FunnyTrees");
    cb.RegisterModule(new MainModule(connectionString!));
});

builder.Services.AddControllers(opt => opt.SuppressAsyncSuffixInActionNames = false);

/* Configure Fluent Validation */
builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssemblyContaining<Program>();

/* Configure Swagger/OpenAPI */
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SupportNonNullableReferenceTypes();
    opt.SwaggerDoc(AppVersion, new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Title = "FunnyTrees.WebApi",
        Version = AppVersion
    });
})
.AddSwaggerGenNewtonsoftSupport();

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/* Configure the HTTP request pipeline. */
app.Use(async (ctx, next) =>
{
    // to read request body in error handling middleware
    ctx.Request.EnableBuffering();
    await next();
});
app.UseExceptionHandler("/error");
app.MapControllers();

LogStartInfo(app.Logger);
app.Run();

app.Logger.LogInformation("Application has been stopped");

static void LogStartInfo(ILogger logger)
{
    var assembly = Assembly.GetExecutingAssembly();
    logger.LogInformation("Welcome to {AssemblyInfo}", assembly.ToString());
    logger.LogInformation("App version: {AppVersion}", AppVersion);
    logger.LogInformation("Current platform and OS: {OSInfo}", Environment.OSVersion);
    logger.LogInformation("Current culture: {CultureInfo}", CultureInfo.CurrentCulture);
}
