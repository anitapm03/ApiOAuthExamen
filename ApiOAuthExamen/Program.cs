using ApiOAuthExamen.Data;
using ApiOAuthExamen.Helpers;
using ApiOAuthExamen.Repositories;
using ApiOAuthExamen.Services;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
/*
 para cuando haya keys
    builder.Services.AddAzureClients(factory =>
    {
    factory.AddSecretClient
        (builder.Configuration.GetSection("KeyVault");
    });

SecretClient secretClient = 
    builder.Services.BuildServiceProvider().GetService<SecretClient>();
KeyVaultSecret secretSql = await 
    secretClient.GetSecretAsync("SqlAzure");
KeyVaultSecret secretIssuer = await 
    secretClient.GetSecretAsync("Issuer");
KeyVaultSecret secretAudience = await 
    secretClient.GetSecretAsync("Audience");
KeyVaultSecret secretKey = await 
    secretClient.GetSecretAsync("SecretKey");
 */
builder.Services.AddAzureClients(factory =>
{
    factory.AddSecretClient
        (builder.Configuration.GetSection("KeyVault"));
});
SecretClient secretClient =
    builder.Services.BuildServiceProvider().GetService<SecretClient>();

KeyVaultSecret secretSql = await
    secretClient.GetSecretAsync("SQL");


string azureKeys = builder.Configuration.GetValue<string>
    ("AzureKeys:StorageAccount");
BlobServiceClient blobServiceClient =
    new BlobServiceClient(azureKeys);
builder.Services.AddTransient<BlobServiceClient>
    (x => blobServiceClient);
builder.Services.AddTransient<ServiceStorageBlobs>();


HelperActionServicesOAuth helper =
    new HelperActionServicesOAuth(builder.Configuration);
builder.Services.AddSingleton<HelperActionServicesOAuth>(helper);
builder.Services.AddAuthentication
    (helper.GetAuthenticationSchema())
    .AddJwtBearer(helper.GetJwtBearerOptions());

string connectionString = secretSql.Value;
    //builder.Configuration.GetConnectionString("SQL");
builder.Services.AddTransient<RepositoryCubos>();
builder.Services.AddDbContext<CubosContext>
    (options => options.UseSqlServer(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Api Examen Cubos",
        Description = "AAAAAAAAAAAAAAA",
        Version = "v1"
    });
});

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(url: "/swagger/v1/swagger.json"
        , name: "Api Cubos Examen v1");
    options.RoutePrefix = "";
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
