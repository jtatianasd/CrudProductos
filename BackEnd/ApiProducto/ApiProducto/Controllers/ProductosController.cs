using ApiProducto.Repository.IRepository;
using ApiProducto.Models;
using ApiProducto.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.IO;

namespace ApiProducto.Controllers
{
	[Route("api/Productos")]
	[ApiController]
	public class ProductosController : Controller
	{
		private readonly IProductoRepository _prodRepo;
		private readonly IWebHostEnvironment _hostingEnviroment;
		private readonly IMapper _mapper;
		public ProductosController(IProductoRepository prodRepo, IMapper mapper, IWebHostEnvironment hostingEnviroment)
		{
			_prodRepo = prodRepo;
			_mapper = mapper;
			_hostingEnviroment = hostingEnviroment;
		}
		/// <summary>
		/// Obtener todos los productos
		/// </summary>
		/// <returns></returns>
		[ProducesResponseType(200, Type = typeof(List<ProductoDTO>))]
		[ProducesResponseType(400)]
		[HttpGet]
		public IActionResult GetProductos()
		{
			var listaProductos = _prodRepo.GetProductos();
			var listaProductosDTO = new List<ProductoDTO>();
			foreach (var lista in listaProductos)
			{
				listaProductosDTO.Add(_mapper.Map<ProductoDTO>(lista));
			}
			return Ok(listaProductosDTO);
		}

		/// <summary>
		///Obtener un producto individual por ID
		/// </summary>
		/// <param name="productoId"> </param>
		/// <returns></returns>
		[ProducesResponseType(200, Type = typeof(ProductoDTO))]
		[ProducesResponseType(404)]
		[ProducesDefaultResponseType]
		[HttpGet("{productoId:int}", Name = "GetProducto")]
		public IActionResult GetProducto(int productoId)
		{
			var itemProducto = _prodRepo.GetProducto(productoId);
			if (itemProducto == null)
			{
				return NotFound();
			}
			else
			{
				var itemProductoDTO = _mapper.Map<ProductoDTO>(itemProducto);
				return Ok(itemProductoDTO);
			}
		}

		/// <summary>
		///Obtener todos los productos de una categoria
		/// </summary>
		/// <param name="categoriaId"> </param>
		/// <returns></returns>
		[ProducesResponseType(200, Type = typeof(ProductoDTO))]
		[ProducesResponseType(404)]
		[ProducesDefaultResponseType]
		[HttpGet("GetProductosEnCategoria/{categoriaId:int}")]
		public IActionResult GetProductosEnCategoria(int categoriaId)
		{
			var listaProducto = _prodRepo.GetProductosEnCategoria(categoriaId);
			if (listaProducto == null || listaProducto.Count == 0)
			{
				return NotFound();
			}
			var itemProducto = new List<ProductoDTO>();
			foreach (var item in listaProducto)
			{
				itemProducto.Add(_mapper.Map<ProductoDTO>(item));
			}
			return Ok(itemProducto);
		}
		/// <summary>
		///Obtener un producto en especifico por nombre
		/// </summary>
		/// <param name="nombre"> </param>
		/// <returns></returns>
		[ProducesResponseType(200, Type = typeof(ProductoDTO))]
		[ProducesResponseType(404)]
		[ProducesDefaultResponseType]
		[HttpGet("Buscar")]
		public IActionResult Buscar(string nombre)
		{
			try
			{
				var resultado = _prodRepo.BuscarProducto(nombre);
				if (resultado.Any())
				{
					return Ok(resultado);
				}
				return NotFound();
			}
			catch (Exception)
			{

				return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos de la aplicacion");
			}
		}
		/// <summary>
		/// Crear un nuevo producto
		/// </summary>
		/// <param name="productoDTO"></param>
		/// <returns></returns>
		[ProducesResponseType(201, Type = typeof(ProductoDTOCreate))]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[HttpPost]
		public IActionResult CrearProducto([FromBody] ProductoDTOCreate productoDTO)
		{

			if (productoDTO == null)
			{
				return BadRequest(ModelState);
			}
			if (_prodRepo.ExisteProducto(productoDTO.Nombre))
			{
				ModelState.AddModelError("", "El Producto ya existe");
				return StatusCode(404, ModelState);
			}

			if(productoDTO.Foto!=null)
			{
				var archivo = productoDTO.Foto;
				string rutaPrincipal = _hostingEnviroment.WebRootPath;
				var archivos = HttpContext.Request.Form.Files;
				if (archivo != null)
				{
					if (archivo.Length > 0)
					{
						var nombreFoto = Guid.NewGuid().ToString();
						var subidas = Path.Combine(rutaPrincipal, @"fotos");
						var extension = Path.GetExtension(archivos[0].FileName);
						using (var fileStreams = new FileStream(Path.Combine(subidas, nombreFoto + extension), FileMode.Create))
						{
							archivos[0].CopyTo(fileStreams);
						}
						productoDTO.RutaImagen = @"\fotos\" + nombreFoto + extension;
					}
				}
			}


			var Producto = _mapper.Map<Producto>(productoDTO);

			if (!_prodRepo.CrearProducto(Producto))
			{
				ModelState.AddModelError("", $"Algo Salio mal guardando el registro{Producto.Nombre}");
				return StatusCode(500, ModelState);
			}
			return CreatedAtRoute("GetProducto", new { productoId = Producto.Id }, Producto);
		}


		/// <summary>
		/// Actualizar un producto existente
		/// </summary>
		/// <param name="productoId"></param>
		/// <param name="productoDTO"></param>
		/// <returns></returns>
		[ProducesResponseType(204)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[HttpPatch("{productoId:int}", Name = "ActualizarProducto")]
		public IActionResult ActualizarProducto(int productoId, [FromBody] ProductoDTO productoDTO)
		{
			if (productoDTO == null || productoId != productoDTO.Id)
			{
				return BadRequest(ModelState);
			}
			var Producto = _mapper.Map<Producto>(productoDTO);
			if (!_prodRepo.ActualizarProducto(Producto))
			{
				ModelState.AddModelError("", $"Algo Salio mal actualizando el registro{Producto.Nombre}");
				return StatusCode(500, ModelState);
			}
			return NoContent();
		}
		/// <summary>
		/// Borrar un producto existente
		/// </summary>
		/// <param name="productoId"></param>
		/// <returns></returns>
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesDefaultResponseType]
		[HttpDelete("{productoId:int}", Name = "BorrarProducto")]
		public IActionResult BorrarProducto(int productoId)
		{
			if (!_prodRepo.ExisteProducto(productoId))
			{
				return NotFound();
			}
			var Producto = _prodRepo.GetProducto(productoId);
			if (!_prodRepo.BorrarProducto(Producto))
			{
				ModelState.AddModelError("", $"Algo Salio mal borrando el registro{Producto.Nombre}");
				return StatusCode(500, ModelState);
			}
			return NoContent();
		}
	}
}
