using fluxo_de_caixa_dotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace fluxo_de_caixa_dotnet.db;

public class MeuDbContext : DbContext
{
    public MeuDbContext(DbContextOptions<MeuDbContext> options) : base(options) { }
    public DbSet<Caixa> Caixas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Caixa>(entity =>
        {
            // Configura a propriedade "Tipo" para ser VARCHAR(255)
            entity.Property(e => e.Tipo).HasColumnType("VARCHAR(255)");

            // Configura a propriedade "Id" como chave primária
            entity.HasKey(e => e.Id);
        });
    }
}