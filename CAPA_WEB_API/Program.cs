using CAPA_DATOS;
using CAPA_NEGOCIO;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Registrar servicios
builder.Services.AddScoped<IDataAccess, DataAccess>();
builder.Services.AddScoped<IRoles_Services, Roles_Services>();
builder.Services.AddScoped<IUsers_Services, Users_Services>();
builder.Services.AddScoped<IAccess_Services, Access_Services>();
builder.Services.AddScoped<IGates_Services, Gates_Services>();
builder.Services.AddScoped<ICasting_Services, Casting_Services>();
builder.Services.AddScoped<ICritically_Services, Critically_Services>();
builder.Services.AddScoped<IActuator_Services, Actuator_Services>();
builder.Services.AddScoped<IMold_Services, Mold_Services>();
builder.Services.AddScoped<ILocation_Services, Location_Services>();
builder.Services.AddScoped<ITransaction_Services, Transaction_Services>();
builder.Services.AddScoped<IItemBom_Services, ItemBom_Services>();
builder.Services.AddScoped<IInventoryBOH_Services, InventoryBOH_Services>();
builder.Services.AddScoped<IInventoryTransactions_Services, InventoryTransactions_Services>();
builder.Services.AddScoped<IPartMaintenance_Services, PartMaintenance_Services>();
builder.Services.AddScoped<Ivw_EBS_WorkOrders_Services, vw_EBS_WorkOrders_Services>();
builder.Services.AddScoped<IListNumber_Services, ListNumber_Services>();
builder.Services.AddScoped<Ivw_EBS_List_Numbers_Services, vw_EBS_List_Numbers_Services>();
builder.Services.AddScoped<IMoldEvaluationPart_Services,MoldEvaluationPart_Services>();
builder.Services.AddScoped<IMoldEvaluation_Services,MoldEvaluation_Services>();
builder.Services.AddScoped<IDashboard_Services, Dashboard_Services>();
builder.Services.AddScoped<ICategorization_Services, Categorization_Services>();


// Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.DictionaryKeyPolicy = null;
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CAPA_WEB_API",
        Version = "v1"
    });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// (Opcional futuro) Auth JWT aquí

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
