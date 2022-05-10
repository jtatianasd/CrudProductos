using ApiProducto.Models;
using System.Collections.Generic;

namespace ApiProducto.Repository.IRepository
{
	public interface IProductoRepository
	{
		ICollection<Producto> GetProductos();
		ICollection<Producto> GetProductosEnCategoria(int CatId);
		Producto GetProducto(int productoId);
		bool ExisteProducto(string nombre);
		IEnumerable<Producto> BuscarProducto(string nombre);
		bool ExisteProducto(int id);
		bool CrearProducto(Producto producto);
		bool ActualizarProducto(Producto producto);
		bool BorrarProducto(Producto producto);
		bool Guardar();
	}
}
