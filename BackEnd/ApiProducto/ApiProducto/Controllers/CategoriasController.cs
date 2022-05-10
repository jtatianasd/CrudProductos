using ApiProducto.Models;
using ApiProducto.Models.DTO;
using ApiProducto.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ApiProducto.Controllers
{
	[Route("api/Categorias")]
	[ApiController]
	public class CategoriasController : Controller
	{
		private readonly ICategoriaRepository _ctRepo;
		private readonly IMapper _mapper;
		public CategoriasController(ICategoriaRepository ctRepo, IMapper mapper)
		{
			_ctRepo = ctRepo;
			_mapper = mapper;
		}

		/// <summary>
		/// Obtener todas las categorias
		/// </summary>
		/// <returns></returns>
		[ProducesResponseType(200, Type = typeof(List<CategoriaDTO>))]
		[ProducesResponseType(400)]
		[HttpGet]
		public IActionResult GetCategorias()
		{
			var listaCategorias = _ctRepo.GetCategorias();
			var listaCategoriasDTO = new List<CategoriaDTO>();
			foreach (var lista in listaCategorias)
			{
				listaCategoriasDTO.Add(_mapper.Map<CategoriaDTO>(lista));
			}
			return Ok(listaCategoriasDTO);
		}

		/// <summary>
		///Obtener una categoria individual por ID
		/// </summary>
		/// <param name="categoriaId"> </param>
		/// <returns></returns>
		[ProducesResponseType(200, Type = typeof(ProductoDTO))]
		[ProducesResponseType(404)]
		[ProducesDefaultResponseType]
		[HttpGet("{categoriaId:int}", Name = "GetCategoria")]
		public IActionResult GetCategoria(int categoriaId)
		{
			var itemCategoria = _ctRepo.GetCategoria(categoriaId);
			if (itemCategoria == null)
			{
				return NotFound();
			}
			else
			{
				var itemCategoriaDTO = _mapper.Map<CategoriaDTO>(itemCategoria);
				return Ok(itemCategoriaDTO);
			}

		}

		/// <summary>
		/// Crear una nueva Categoria
		/// </summary>
		/// <param name="categoriaDTO"></param>
		/// <returns></returns>
		[ProducesResponseType(201, Type = typeof(CategoriaDTO))]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[HttpPost]
		public IActionResult CrearCategoria([FromBody] CategoriaDTO categoriaDTO)
		{
			if (categoriaDTO == null)
			{
				return BadRequest(ModelState);
			}
			if (_ctRepo.ExisteCategoria(categoriaDTO.Nombre))
			{
				ModelState.AddModelError("", "La Categoria ya existe");
				return StatusCode(404, ModelState);
			}

			var categoria = _mapper.Map<Categoria>(categoriaDTO);
			if (!_ctRepo.CrearCategoria(categoria))
			{
				ModelState.AddModelError("", $"Algo Salio mal guardando el registro{categoria.Nombre}");
				return StatusCode(500, ModelState);
			}
			return CreatedAtRoute("GetCategoria", new { categoriaId = categoria.Id }, categoria);
		}
		/// <summary>
		/// Actualizar una categoria existente
		/// </summary>
		/// <param name="categoriaId"></param>
		/// <param name="categoriaDTO"></param>
		/// <returns></returns>
		[ProducesResponseType(204)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[HttpPatch("{CategoriaId:int}", Name = "ActualizarCategoria")]
		public IActionResult ActualizarCategoria(int categoriaId, [FromBody] CategoriaDTO categoriaDTO)
		{
			if (categoriaDTO == null || categoriaId != categoriaDTO.Id)
			{
				return BadRequest(ModelState);
			}
			var categoria = _mapper.Map<Categoria>(categoriaDTO);
			if (!_ctRepo.ActualizarCategoria(categoria))
			{
				ModelState.AddModelError("", $"Algo Salio mal actualizando el registro{categoria.Nombre}");
				return StatusCode(500, ModelState);
			}
			return NoContent();
		}
		/// <summary>
		/// Borrar una categoria existente
		/// </summary>
		/// <param name="categoriaId"></param>
		/// <returns></returns>
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesDefaultResponseType]
		[HttpDelete("{categoriaId:int}", Name = "BorrarCategoria")]
		public IActionResult BorrarCategoria(int categoriaId)
		{
			if (!_ctRepo.ExisteCategoria(categoriaId))
			{
				return NotFound();
			}
			var categoria = _ctRepo.GetCategoria(categoriaId);
			if (!_ctRepo.BorrarCategoria(categoria))
			{
				ModelState.AddModelError("", $"Algo Salio mal borrando el registro{categoria.Nombre}");
				return StatusCode(500, ModelState);
			}
			return NoContent();
		}
	}
}
