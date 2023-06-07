using AP_MediaService.BLL;
using AP_MediaService.Common.ActionFilters;
using AP_MediaService.Common.Helper;
using AP_MediaService.Common.Helper.Interface;
using AP_MediaService.Common.Helper.Logging;
using AP_MediaService.Common.Helper.Logging.Interface;
using AP_MediaService.Common.Interfaces;
using AP_MediaService.Common.Services;
using AP_MediaService.Common.Utilities;
using AP_MediaService.DAL;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.OpenApi.Models;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc(config =>
    config.Filters.Add(typeof(ValidateModelAttribute))

);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IHeadersUtils, HeadersUtil>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<ILogService, LogService>();
builder.Services.AddTransient<IHttpResultHelper, HttpResultHelper>();
builder.Services.AddScoped<ISummaryLog, SummaryLog>();
builder.Services.AddTransient<ITraceLog, TraceLog>();
builder.Services.AddTransient<LogHelper, LogHelper>();
builder.Services.AddSingleton<MetricReporter>();

ServicesDependencyContainer.RegisterServices(builder.Services, builder.Configuration);
RepositoriesDependencyContainer.RegisterRepositories(builder.Services, builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "MediaServiceAPI",
        Description = "MediaService Resful API Document",
    });
    c.OperationFilter<SwaggerHeaderFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
