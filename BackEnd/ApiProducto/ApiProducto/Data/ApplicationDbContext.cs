using ApiProducto.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiProducto.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext()
		{

		}
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}
		public DbSet<Producto> Producto { get; set; }
		public DbSet<Categoria> Categoria { get; set; }
	}
}
