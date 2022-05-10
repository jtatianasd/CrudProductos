using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ApiProducto.Models.DTO
{
	public class ProductoDTO
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "El nombre es obligaotrio")]
		public string Nombre { get; set; }

		public string RutaImagen { get; set; }
		public IFormFile Foto { get; set; }

		[Required(ErrorMessage = "La descripcion es obligatoria")]
		public string Descripcion { get; set; }



		//Llave foranea con la tabla Categoria
		public int categoriaId { get; set; }
		public Categoria Categoria { get; set; }
	}
}
