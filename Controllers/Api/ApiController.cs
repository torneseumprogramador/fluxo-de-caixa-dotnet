using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using fluxo_de_caixa_dotnet.Models;
using fluxo_de_caixa_dotnet.db;

namespace fluxo_de_caixa_dotnet.Controllers.ApiController;

[Route("/api/caixa")]
public class ApiController : ControllerBase
{
    public ApiController(MeuDbContext context)
    {
        _context = context;
    }

    private MeuDbContext _context;

    [Route("")]
    public dynamic Index( string tipo )
    {
        List<Caixa> lista;
        if(string.IsNullOrEmpty(tipo)){
            lista = _context.Caixas.ToList();
        }
        else{
            lista = _context.Caixas.Where(c => c.Tipo.ToLower().Contains(tipo.ToLower())).ToList();
        }

        double valorTotal = 0;
        double receitas = 0;
        double despesas = 0;

        foreach(var item in lista)
        {
            if(item.Status == Status.Entrada){
                receitas += item.Valor;
                valorTotal += item.Valor;
            }
            else{
                despesas += item.Valor;
                valorTotal -= item.Valor;
            }
        }

        return new {
            valorTotal = valorTotal,
            receitas = receitas,
            despesas = despesas,
            lista = lista,
        };
    }

    [Route("adicionar")]
    [HttpPost]
    public dynamic adicionarNoCaixa([FromBody] Caixa caixa)
    {
        _context.Caixas.Add(caixa);
        _context.SaveChanges();

        return caixa;
    }

    [Route("excluir/{id}")]
    [HttpDelete]
    public dynamic excluir([FromRoute] int id)
    {
        var item = _context.Caixas.Find(id);
        _context.Remove(item);
        _context.SaveChanges();
        
        return new {
            Mensagem = "Item excluido"
        };
    }
}
