using ApiProducto.Data;
using ApiProducto.Models;
using ApiProducto.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ApiProducto.Repository
{
	public class ProductoRepository : IProductoRepository
	{
		private readonly ApplicationDbContext _bd;
		public ProductoRepository(ApplicationDbContext bd)
		{
			_bd = bd;
		}
		public bool ActualizarProducto(Producto producto)
		{
			_bd.Producto.Update(producto);
			return Guardar();
		}

		public bool BorrarProducto(Producto producto)
		{
			_bd.Producto.Remove(producto);
			return Guardar();
		}

		public IEnumerable<Producto> BuscarProducto(string nombre)
		{
			IQueryable<Producto> query = _bd.Producto;
			if (!string.IsNullOrEmpty(nombre))
			{
				query = query.Where(e => e.Nombre.Contains(nombre) || e.Descripcion.Contains(nombre));
			}
			return query.ToList();
		}

		public bool CrearProducto(Producto producto)
		{
			_bd.Producto.Add(producto);
			return Guardar();
		}

		public bool ExisteProducto(string nombre)
		{
			bool valor = _bd.Producto.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
			return valor;
		}

		public bool ExisteProducto(int id)
		{
			return _bd.Producto.Any(c => c.Id == id);
		}

		public Producto GetProducto(int productoId)
		{
			return _bd.Producto.FirstOrDefault(c => c.Id == productoId);
		}

		public ICollection<Producto> GetProductos()
		{
			//return _bd.Producto.OrderBy(c => c.Nombre).ToList();
			return _bd.Producto.Include(ca=>ca.Categoria).OrderBy(c => c.Nombre).ToList();
		}

		public ICollection<Producto> GetProductosEnCategoria(int CatId)
		{
			return _bd.Producto.Include(ca => ca.Categoria).Where(ca => ca.categoriaId == CatId).ToList();
		}

		public bool Guardar()
		{
			return _bd.SaveChanges() >= 0 ? true : false;
		}
	}
}
