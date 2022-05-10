using System;
using System.ComponentModel.DataAnnotations;

namespace ApiProducto.Models.DTO
{
	public class CategoriaDTO
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "El nombre es obligaotrio")]
		public string Nombre { get; set; }
		public DateTime FechaCreacion { get; set; }
	}
}
