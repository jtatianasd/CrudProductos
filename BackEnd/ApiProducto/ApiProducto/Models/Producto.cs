using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiProducto.Models
{
	public class Producto
	{
		[Key]
		public int Id { get; set; }
		public string Nombre { get; set; }
		public string Descripcion { get; set; }
		public string RutaImagen { get; set; }

		//Llave foranea con la tabla Categoria
		public int categoriaId { get; set; }
		[ForeignKey("categoriaId")]
		public Categoria Categoria { get; set; }
	}
}
