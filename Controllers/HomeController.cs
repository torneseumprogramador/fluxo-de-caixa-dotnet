using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using fluxo_de_caixa_dotnet.Models;
using fluxo_de_caixa_dotnet.db;

namespace fluxo_de_caixa_dotnet.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, MeuDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    private MeuDbContext _context;

    public IActionResult Index( string tipo )
    {
        List<Caixa> lista;
        if(string.IsNullOrEmpty(tipo)){
            lista = _context.Caixas.ToList();
        }
        else{
            lista = _context.Caixas.Where(c => c.Tipo.ToLower().Contains(tipo.ToLower())).ToList();
        }

        ViewBag.lista = lista;

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


        ViewBag.valorTotal = valorTotal;
        ViewBag.receitas = receitas;
        ViewBag.despesas = despesas;

        return View();
    }

    [Route("/adicionar")]
    public IActionResult adicionar()
    {
        return View();
    }

    [Route("/adicionar-no-caixa")]
    [HttpPost]
    public IActionResult adicionarNoCaixa(Caixa caixa)
    {
        _context.Caixas.Add(caixa);
        _context.SaveChanges();

        return Redirect("/");
    }

    [Route("/excluir/{id}")]
    [HttpGet]
    public IActionResult excluir(int id)
    {
        var item = _context.Caixas.Find(id);
        _context.Remove(item);
        _context.SaveChanges();
        
        return Redirect("/");
    }
    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
