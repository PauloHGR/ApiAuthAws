using Amazon.CognitoIdentityProvider.Model;
using Amazon.CognitoIdentityProvider;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using APIStockManager;
using Amazon;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

/*builder.Services.AddCognitoIdentity();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.Authority = "https://cognito-idp.us-east-1.amazonaws.com/us-east-1_XXXXXXXXX";
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,
        };
    });
*/

builder.Services.Configure<IdentityProviderConfiguration>(
    builder.Configuration.GetSection("AWS:Cognito"));

builder.Services.AddSingleton<IAmazonCognitoIdentityProvider>(sp =>
{
    var config = sp.GetRequiredService<IOptions<IdentityProviderConfiguration>>().Value;
    var awsRegion = RegionEndpoint.GetBySystemName(config.Region);
    return new AmazonCognitoIdentityProviderClient(awsRegion);
});

builder.Services.AddScoped<ICognitoService, CognitoService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseAuthentication();
//app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.UseCors();
app.Run();

