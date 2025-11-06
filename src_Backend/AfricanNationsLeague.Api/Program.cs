using AfricanNationsLeague.Api.Abstracts;
using AfricanNationsLeague.Api.Options;
using AfricanNationsLeague.Api.Services;
using AfricanNationsLeague.Application.Services;
using AfricanNationsLeague.Domain.Enums;
using AfricanNationsLeague.Infrastructure.Configuration;
using AfricanNationsLeague.Infrastructure.Context;
using AfricanNationsLeague.Infrastructure.Data;
using AfricanNationsLeague.Infrastructure.Interface;
using AfricanNationsLeague.Infrastructure.Repository;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("Mongo"));
builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();
builder.Services.AddScoped<TournamentService>();

builder.Services.AddSingleton<IMongoContext, MongoContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<TeamService>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<MatchService>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<CountryService>();
builder.Services.AddScoped<IMongoDatabase>(sp =>
    sp.GetRequiredService<IMongoContext>().Database);
builder.Services.AddHttpClient();



builder.Services.Configure<GmailOptions>(
    builder.Configuration.GetSection(GmailOptions.GmailOptionsKey))
    ;

builder.Services.AddScoped<IMailService, GmailService>();



BsonSerializer.RegisterSerializer(typeof(Role), new EnumSerializer<Role>(MongoDB.Bson.BsonType.String));

var config = builder.Configuration;




var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
var http = scope.ServiceProvider.GetRequiredService<HttpClient>();
await CountrySeeder.SeedAsync(db, http);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
