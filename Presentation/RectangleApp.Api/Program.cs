using FluentMigrator.Runner;
using Project.Core.Settings;
using Project.Infrastructure.Migrations;
using Project.Infrastructure.Utilities;
using Project.Infrastructure.Utilities.DependencyResolvers;
using Project.Service.Utilities.DependencyResolvers;
using RectangleApp.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCoreDependencies();
builder.Services.AddProjectDependencies();

builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
        .AddSqlServer()
        .WithGlobalConnectionString(AppSettings.Settings.AppDbConnectionModel.ToString())
        .ScanIn(typeof(CreateProcedures).Assembly).For.Migrations())
    .AddLogging(lb => lb.AddFluentMigratorConsole());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.Services.SeedAsync();

app.UseMiddleware<BasicAuthentication>();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
