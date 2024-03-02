using AutoMapper;
using MassTransit;
using MembersControlSystem.Extensions;
using MembersControlSystem.Repositories;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Repositories;
using Repositories.Config;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


//builder.Services.AddDbContext<RepositoryContext>();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRedis(builder.Configuration);
builder.Services.AddSignalR();
builder.Services.ConfigureSignalR();

builder.Services.ConfigureRepositories();
builder.Services.ConfigureActionFilter();

builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.ConfigureSendMail(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var memberContext = scope.ServiceProvider.GetRequiredService<RepositoryContext>();
    memberContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//SignalR test etmek için "http://localhost:8080/" adresine gidebilrisiniz
app.MapHub<MessageHub>("/messageHub", options =>
{
    options.Transports =
        HttpTransportType.WebSockets |
        HttpTransportType.LongPolling;
}
);
app.UseCors("Cors");
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
