
using skymoon.Models;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.WebHost.UseUrls("http://0.0.0.0:8000");

var app = builder.Build();

app.UseCors("AllowAll");

Funcionario[] funcionarios = new Funcionario[100];
int totalFuncionarios = 0;

app.MapGet("/", () =>
{
    return Results.Ok("API SkyMoon funcionando com sucesso!");
});

app.MapPost("/funcionario", (JsonElement body) =>
{
    Random Random = new Random();
    Funcionario funcionario = new Funcionario();

    //adiciona valores
    funcionario.Nome = body.GetProperty("nome").GetString();
    funcionario.Id = Random.Next(1000,9999);
    funcionario.Idade = body.GetProperty("idade").GetInt32();
    funcionario.Cargo = body.GetProperty("cargo").GetString();
    funcionario.Departamento = body.GetProperty("departamento").GetString();
    funcionario.Salario = body.GetProperty("salario").GetDouble();

    funcionarios[totalFuncionarios] = funcionario;
    totalFuncionarios++;

    return Results.Ok(
        new{
            funcionario
        
});
});

app.MapGet("/funcionario", () =>
{
    Funcionario[] funcionarioCadastrados = new Funcionario[totalFuncionarios];
    
    for(int i = 0; i< totalFuncionarios; i++){
        funcionarioCadastrados[i] = funcionarios[i];
    }
    return Results.Ok(
        new{
            funcionarioCadastrados
});
});

app.MapPatch("/funcionario/{id}", (int id, JsonElement body) =>
{
    double novo_salario = body.GetProperty("salario").GetDouble();

    for(int i=0; i<totalFuncionarios; i++){
         if(funcionarios[i].Id == id){
            funcionarios[i].Salario = novo_salario;
            
            return Results.Ok(new{ funcionario = funcionarios[i]});
         }
    }
     return Results.NotFound(new {mensage = "funcionario não encontrado"});

    
});



app.MapPut("/funcionario/{id}", (int id, JsonElement body) =>
{   
     for(int i=0; i<totalFuncionarios; i++){
         if(funcionarios[i].Id == id){
             funcionarios[i].Nome = body.GetProperty("nome").GetString();
             funcionarios[i].Idade = body.GetProperty("idade").GetInt32();
             funcionarios[i].Cargo = body.GetProperty("cargo").GetString();
             funcionarios[i].Departamento = body.GetProperty("departamento").GetString();
             funcionarios[i].Salario = body.GetProperty("salario").GetDouble();
             
              return Results.Ok(new{ funcionarios});
         }
     }
      return Results.NotFound(new {mensage = "funcionario não encontrado"});
});

app.MapDelete("/funcionario", (int id) =>
{
     for(int i=0; i<totalFuncionarios; i++){
         if(funcionarios[i].Id == id){
            Funcionario funcionario_removido = funcionarios[i];
              for(int x = i; x < totalFuncionarios; x++){
                      funcionarios[x] = funcionarios[x+1];   
              }
              totalFuncionarios--;
               return Results.Ok(new
            {
                mensagem = "Funcionário removido com sucesso.",
                funcionario = funcionario_removido
            });
         }
     }
        return Results.NotFound(new {mensage = "funcionario não encontrado"});
});
/*
app.MapGet("/funcionario/departamento/busca", (string departamento) =>
{
    
});

app.MapGet("/funcionario/busca", (string nome) =>
{
   
}); */

app.Run();