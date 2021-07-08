using System.Reflection;
using System.Security.Principal;
using System.Text;
using Berger.Data.Common;
using Berger.Data.MsfaEntity;
using BergerMsfaApi.ActiveDirectory;
using BergerMsfaApi.Filters;
using BergerMsfaApi.Helpers;
using BergerMsfaApi.Middlewares;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.Examples;
using BergerMsfaApi.Repositories;
using GenericServices.Setup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetCore.AutoRegisterDi;
using AutoMapper;
using Berger.Common.HttpClient;
using Berger.Odata.Services;
using Berger.Odata.Repositories;
using Berger.Common;
using BergerMsfaApi.Models.EmailVm;
using BergerMsfaApi.Services.Excel.Implementation;
using BergerMsfaApi.Services.Excel.Interface;

namespace BergerMsfaApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ActiveDirectorySettingsModel>(options => Configuration.GetSection("ActiveDirectorySettings").Bind(options));
            services.Configure<TokensSettingsModel>(options => Configuration.GetSection("TokensSettings").Bind(options));
            services.Configure<Berger.Odata.Model.ODataSettingsModel>(options => Configuration.GetSection("ODataSettings").Bind(options));
            services.Configure<FCMSettingsModel>(options => Configuration.GetSection("FCMSettings").Bind(options));
            //services.Configure<AuthMessageSenderOptions>(options =>
            //           Configuration.GetSection("SendGridEmailSettings").Bind(options));

            services.Configure<SmtpSettings>(options =>
                       Configuration.GetSection("SmtpSettings").Bind(options));

            var appTokensSettings = Configuration.GetSection("TokensSettings").Get<TokensSettingsModel>();

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString(nameof(ApplicationDbContext))));

            services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })

                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = appTokensSettings.Issuer,
                        ValidAudience = appTokensSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appTokensSettings.Key)),
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                    };
                });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(typeof(IHttpContextStorageContainer<>), typeof(HttpContextStorageContainer<>));
            services.AddScoped<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);
            services.AddScoped<IUserRequest, UserRequest>();

            services.AddScoped<DbContext, ApplicationDbContext>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, ApplicationDbContext>();
            services.AddScoped<IActiveDirectoryServices, ActiveDirectoryServices>();
            services.AddScoped<IHttpClientService, HttpClientService>();
            services.AddScoped<IODataService, ODataService>();
            services.AddScoped(typeof(IODataRepository<>), typeof(ODataRepository<>));
            services.AddScoped<IODataCommonService, ODataCommonService>();
            services.AddScoped<IODataBrandService, ODataBrandService>();
            services.AddScoped<ISalesDataService, SalesDataService>();
            services.AddScoped<IMTSDataService, MTSDataService>();
            services.AddScoped<IFinancialDataService, FinancialDataService>();
            services.AddScoped<IBalanceDataService, BalanceDataService>();
            services.AddScoped<IQuarterlyPerformanceDataService, QuarterlyPerformanceDataService>();
            services.AddScoped<IReportDataService, ReportDataService>();
            services.AddScoped<ICollectionDataService, CollectionDataService>();
            services.AddScoped<IStockDataService, StockDataService>();
            services.AddScoped<IODataNotificationService, ODataNotificationService>();
            services.AddScoped<IKpiDataService, KpiDataService>();
            services.AddScoped<IExcelReaderService, ExcelReaderService>();
            services.AddScoped<IAlertNotificationDataService, AlertNotificationDataService>();

            //services.Configure<AuthMessageSenderOptions>(Configuration);
            //services.Configure<SmtpSettings>(Configuration);

            //services.AddHostedService(serviceProvider =>
            //    new NotificationWorker(
            //        serviceProvider.GetService<ILogger<NotificationWorker>>(),
            //        serviceProvider.GetService<IServiceScopeFactory>(),
            //        serviceProvider.GetService<IWebHostEnvironment>().WebRootPath, 24));

            services.AddTransient<IEmailSender, EmailSender>();

            services.RegisterAssemblyPublicNonGenericClasses(Assembly.GetAssembly(typeof(Startup)))
                    .Where(c => c.Name.EndsWith("Repository"))
                    .AsPublicImplementedInterfaces();

            services.RegisterAssemblyPublicNonGenericClasses(Assembly.GetAssembly(typeof(Startup)))
                    .Where(c => c.Name.EndsWith("Service"))
                    .AsPublicImplementedInterfaces();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //cache
            services.AddMemoryCache();

            //filter
            //services.AddScoped<AuthorizeFilter>();
            //services.AddScoped<ValidationFilter>();
            //services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(GlobalActionFilter));
            });

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
                //o.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
            });
            //services.AddSignalR();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.GenericServicesSimpleSetup<ApplicationDbContext>(Assembly.GetAssembly(typeof(QuickCrudModel)));

            services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation  
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Berger MSFA Portal",
                    //Description = "Berger MSFA SWAGGER"
                });
                // To Enable authorization using Swagger (JWT)  
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[] {}
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.Use(next => new RequestDelegate(
                async context =>
                {
                    context.Request.EnableBuffering();
                    await next(context);
                }
            ));

            HttpHelper.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseAuthentication();
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Berger MSFA V1");
                c.RoutePrefix = "msfa";
            });

            app.UseRouting();
            app.UseAuthorization();
            app.UseRequestLocalization();
            app.UseMiddleware<ItemInHttpContextMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}

