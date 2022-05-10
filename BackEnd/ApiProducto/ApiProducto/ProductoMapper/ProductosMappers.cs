using ApiProducto.Models;
using ApiProducto.Models.DTO;
using AutoMapper;

namespace ApiProducto.ProductoMapper
{
	public class ProductosMappers : Profile
	{
		public ProductosMappers()
		{
			CreateMap<Categoria, CategoriaDTO>().ReverseMap();
			CreateMap<Producto, ProductoDTO>().ReverseMap();
			CreateMap<Producto, ProductoDTOCreate>().ReverseMap();
		}
		
	}
}
