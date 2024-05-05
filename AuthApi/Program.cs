using AuthApi.Context;
using AuthApi.Handlers;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(x =>
{
    x.User.RequireUniqueEmail = true;
    x.SignIn.RequireConfirmedAccount = true;
    x.SignIn.RequireConfirmedEmail = true;
    x.Password.RequiredLength = 8;
    x.Lockout.MaxFailedAccessAttempts = 5;

   } ).AddEntityFrameworkStores<DataContext>();

builder.Services.AddSingleton<ServiceBusSender>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("ServiceBus");
    var queueName = builder.Configuration["ServiceBus:SenderQueue"];
    var client = new ServiceBusClient(connectionString);
    return client.CreateSender(queueName);
});

builder.Services.AddSingleton<ServiceBusHandler>();
builder.Services.AddHostedService(x=> x.GetRequiredService<ServiceBusHandler>());

builder.Services.AddHttpClient();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
