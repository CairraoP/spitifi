using System.Reflection;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using spitifi.Data;
using spitifi.Data.DbInitializerDev;
using spitifi.Services.AlbumEraser;
using spitifi.Services.Email;
using spitifi.Services.JWT;
using spitifi.Services.SignalR;

//===================================================================================
//==================== SERVICES CONFIGURATION =======================================
//===================================================================================

var builder = WebApplication.CreateBuilder(args);

// Aumenta tamanho maximo do body... por causa do upload das musicas
builder.Services.Configure<FormOptions>(options => {
    options.MultipartBodyLengthLimit = 100000000; // ~100MB
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartHeadersLengthLimit = int.MaxValue;
});

//-----------------------------------------------------------------------------------
//-------------------- DATABASE CONFIGURATION ---------------------------------------
//-----------------------------------------------------------------------------------
var connectionString = builder.Configuration.GetConnectionString("ConStringMySQL") ??
                       throw new InvalidOperationException("Connection string not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 39))
    ));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//-----------------------------------------------------------------------------------
//-------------------- IDENTITY CONFIGURATION ---------------------------------------
//-----------------------------------------------------------------------------------
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

//-----------------------------------------------------------------------------------
//-------------------- SESSION CONFIGURATION ----------------------------------------
//-----------------------------------------------------------------------------------
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//-----------------------------------------------------------------------------------
//-------------------- JWT AUTHENTICATION CONFIGURATION -----------------------------
//-----------------------------------------------------------------------------------
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
    {
        //options.DefaultScheme = IdentityConstants.ApplicationScheme;
        //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            RoleClaimType = ClaimTypes.Role
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"JWT Auth Failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                // check if we have a valid claims identity
                if (context.Principal?.Identity is ClaimsIdentity claimsIdentity)
                {
                    // find every existing role claims
                    var roleClaims = claimsIdentity.FindAll(ClaimTypes.Role).ToList();
                    foreach (var claim in roleClaims)
                    {
                        // remove the original claim containing comma-separated roles
                        claimsIdentity.RemoveClaim(claim);
                        
                        // split combined roles (comma-separated roles) and create individual claims
                        foreach (var role in claim.Value.Split(','))
                        {
                            // add new claim for each role
                            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.Trim()));
                        }
                    }
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddScoped<JwtService>();

//-----------------------------------------------------------------------------------
//-------------------- EMAIL SERVICES CONFIGURATION ---------------------------------
//-----------------------------------------------------------------------------------
builder.Services.Configure<EmailSenderConfigModel>(builder.Configuration.GetSection("EmailConf"));
builder.Services.AddTransient<ICustomMailer, CustomMailer>();

//-----------------------------------------------------------------------------------
//-------------------- APPLICATION SERVICES CONFIGURATION ---------------------------
//-----------------------------------------------------------------------------------
builder.Services.AddTransient<AlbumEraser>();
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddDistributedMemoryCache();

//-----------------------------------------------------------------------------------
//-------------------- SWAGGER CONFIGURATION ----------------------------------------
//-----------------------------------------------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo 
    {
        Title = "Spitifi",
        Version = "v2",
        Description = "Spitifi API Controllers"
    });
    
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using Bearer scheme. Example: 'Bearer token'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

//===================================================================================
//==================== APPLICATION SETUP ============================================
//===================================================================================
var app = builder.Build();

//-----------------------------------------------------------------------------------
//-------------------- DATABASE INITIALIZATION --------------------------------------
//-----------------------------------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (dbContext.Database.GetDbConnection().State != System.Data.ConnectionState.Open)
    {
        dbContext.Database.OpenConnection();
    }

    dbContext.Database.EnsureCreated();
}

app.UseItToSeedSqlServer();

//-----------------------------------------------------------------------------------
//-------------------- SWAGGER UI MIDDLEWARE ----------------------------------------
//-----------------------------------------------------------------------------------
app.UseSwagger();
app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Spitifi v1"));

//-----------------------------------------------------------------------------------
//-------------------- ENVIRONMENT CONFIGURATION ------------------------------------
//-----------------------------------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//-----------------------------------------------------------------------------------
//-------------------- MIDDLEWARE PIPELINE ------------------------------------------
//-----------------------------------------------------------------------------------
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

//-----------------------------------------------------------------------------------
//-------------------- ENDPOINT CONFIGURATION ---------------------------------------
//-----------------------------------------------------------------------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapHub<LikesServices>("/likes");

app.Run();
