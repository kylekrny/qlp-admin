using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;


using var db = new DatabaseContext();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "QLP Admin API", Description = "QLP API Documentation", Version = "v1"});
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "QLP Admin API V1");
});

app.MapGet("/", () => "Hello World!");

app.MapGet("/orders", () => {});
app.MapPost("/orders", (Order order) => {});
app.MapPatch("/orders", (int id) => {});
app.MapDelete("/orders", (int id) => {});


app.MapPost("/login", (string username, string password) => {});
app.MapPost("/logout", () => {});


app.Run();


// Console.WriteLine($"Database path: {db.DbPath}.");

// Console.WriteLine("Inserting a new user");
// db.Add(new User { Email = "kylekrny@gmail.com", FistName = "Kyle", LastName = "Kearney", Password = "Password"  });
// db.SaveChanges();

// Console.WriteLine("Querying for a user");
// var user = db.Users
//     .OrderBy(b => b.Id)
//     .First();
// Console.WriteLine(user);

// db.Remove(user);
// db.SaveChanges();

// Console.WriteLine("User Deleted");