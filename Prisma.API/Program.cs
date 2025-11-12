using MDFusionLabHaute.API.Helper;
using MDFusionLabHaute.Domain.Abstract;
using MDFusionLabHaute.Domain.Concrete;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ILogRepository, LogRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IExceptionLogRepository, ExceptionLogRepository>();
builder.Services.AddTransient<ICanistersRepository, CanistersRepository>();
builder.Services.AddTransient<IDispenseRepository, DispenseRepository>();
builder.Services.AddTransient<IRefillTrackingRepository, RefillTrackingRepository>();
builder.Services.AddTransient<ICanisterLookupRepository, CanisterLookupRepository>();
builder.Services.AddTransient<ISanitisingTrakingRepository, SanitisingTrakingRepository>();
builder.Services.AddTransient<IMachineRepository, MachineRepository>();
builder.Services.AddTransient<IFormulaRepository, FormulaRepository>();
//builder.Services.AddCors();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3000", "http://createyourformula.hautecustombeauty.com")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHsts();
}
app.UseRouting();
app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true) // allow any origin
            .AllowCredentials());
app.UseAuthorization();
app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ConnectionHub>("/ConnectionHub"); // Restore this
});
//app.MapHub<ConnectionHub>("/ConnectionHub");
app.Run();