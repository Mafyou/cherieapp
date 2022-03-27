using CherieAppBankSound.Models;
using CherieAppBankSound.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Chérie App API", Version = "v1" }
    ));
builder.Services.AddDbContext<MySoundContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnectionString")));

builder.Services.AddCors();
builder.Services.AddSignalR();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Chérie App API v1"));

app.UseCors(
        options =>
        {
            options.WithOrigins("https://as-mvc.azurewebsites.net").AllowAnyHeader().AllowAnyMethod();
        }
    );

app.MapHub<MyDiscussionHub>("/MyDiscussionHub");

app.UseHttpsRedirection();;

app.MapGet("/list",
    async (MySoundContext context, IHubContext<MyDiscussionHub> hubContext) =>
    {
        return await context.MySounds.ToListAsync();
    });
app.MapPost("/create",
    async (MySoundContext context, IHubContext<MyDiscussionHub> hubContext,
    MySound mySound) =>
    {
        try
        {
            if (!mySoundExists(context, mySound))
            {
                context.Add(mySound);
                await context.SaveChangesAsync();
                await hubContext.Clients.All.SendAsync("ServiceMessage", "Nouveau onS", mySound);
            }
            else
            {
                return Results.Conflict();
            }
        }
        catch (Exception)
        {
            return Results.StatusCode(500);
        }
        return Results.Ok();
    });
app.MapPost("/delete",
    async (MySoundContext context, IHubContext<MyDiscussionHub> hubContext,
    MySound mySound) =>
    {
        try
        {
            if (mySoundExists(context, mySound))
            {
                context.Remove(mySound);
                await context.SaveChangesAsync();
                await hubContext.Clients.All.SendAsync("ServiceMessage", "OnS supprimé", mySound);
            }
            else
            {
                return Results.NotFound();
            }
        }
        catch (Exception)
        {
            return Results.StatusCode(500);
        }
        return Results.Ok();
    });
bool mySoundExists(MySoundContext context, MySound mySound)
{
    return context.MySounds.Any(e => e.Id == mySound.Id);
}

app.Run();