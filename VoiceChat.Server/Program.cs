using Npgsql;
using VoiceChat.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VoiceChat.Server.Hubs;

// postgres setup
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSignalR();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

//identity
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddDbContext<VoiceChatDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("VoiceChatDb")));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(); 
    app.MapControllers();
    app.UseHttpsRedirection();
    app.MapHub<VoiceHub>("/voiceHub");
}
app.Run();