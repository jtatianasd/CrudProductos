import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ProductoService } from '../../services/producto.service';
import { CategoriaService } from '../../services/categoria.service';
import { NgxPaginationModule } from 'ngx-pagination';


@Component({
  selector: 'app-productos',
  templateUrl: './productos.component.html',
  styleUrls: ['./productos.component.css']
})
export class ProductosComponent implements OnInit {
  listProductos:any[]=[];
  listCategorias:any[]=[];
  listItem:any[]=[];
  form:FormGroup;
  busqueda:FormGroup;
  accion= 'Agregar';
  id: number | undefined;
  pages: number = 1;
  constructor(private fb: FormBuilder,
    private toastr: ToastrService,
    private _productoService: ProductoService,
    private _categoriaService: CategoriaService) { 
    this.form=this.fb.group({
      nombre:[''],
      descripcion:[''],
      categoria:[''],
      rutaImagen:['']
    }) 
    this.busqueda=this.fb.group({
      valor:[''],
      criterio:['']
    })
    
  }

  ngOnInit(): void {
   this.obtenerProductos();
    this.obtenerCategorias();

  }
  obtenerCategorias(){
   this._categoriaService.getCategorias().subscribe(data=>{
    this.listCategorias=data;
  },error=>{
    console.log(error);
  })
  }
obtenerProductos(){
  this._productoService.GetProductos().subscribe(data=>{
    this.listProductos=data;
    console.log(this.listProductos);
  },error=>{
    console.log(error);
  })
}
  guardarProducto(){
    
      let nombre = this.form.get('nombre')?.value;
      let descripcion =this.form.get('descripcion')?.value;
      let categoriaId =this.form.get('categoria')?.value;
      let rutaImagen =this.form.get('rutaImagen')?.value;
    

    if(this.id == undefined)
    {
      this._productoService.saveProducto(nombre,descripcion,categoriaId,rutaImagen).subscribe(data=>{
        this.toastr.success('El producto fue registrado con exito', 'Producto agregado');
        this.obtenerProductos();
        this.form.reset();
      },error=>{
        this.toastr.error('El producto no pudo ser registrado', 'Producto no agregado');
        console.log(error);
      })
    }
    else
    {
      this.accion='Editar';
      this._productoService.updateProducto(this.id,nombre,descripcion,categoriaId,rutaImagen).subscribe(data=>{
        this.form.reset();
        this.accion='Agregar';
        this.id=undefined;
        this.toastr.info('El producto fue actualizado con exito', 'Producto actualizado');
        this.obtenerProductos();
      },error=>{
        this.toastr.error('El producto no pudo ser actualizado', 'Producto no actualizado');
        console.log(error);
      } )
    }



  }
  eliminarProducto(id:number)
  {
    this._productoService.deleteProductos(id).subscribe(data=>{
      this.toastr.success('El producto fue eliminado con exito', 'Producto eliminado');
      this.obtenerProductos();
    },error=>{
      this.toastr.error('El producto no pudo ser eliminado', 'Producto no eliminado');
      console.log(error);
    })
  }

  editarProducto(prod:any)
  {
    this.accion='Editar';
    this.id=prod.id;

    this.form.patchValue({
      nombre:prod.nombre,
      descripcion:prod.descripcion,
      categoria:prod.categoria.id

    })
  }
  buscar()
  {  
    let criterio =this.busqueda.get('criterio')?.value;
    let valor= this.busqueda.get('valor')?.value;
    console.log(criterio)
    console.log(valor)
    if(criterio=="id_prod")
    {
      this._productoService.searchProductById(valor).subscribe(data =>{
        this.toastr.success('Se encontro el producto '+data.nombre, 'Producto no encontrado');
      },error=>{
        this.toastr.error('No se encontro el ID del producto', 'Producto no encontrado');
        console.log(error);
      })
    }
    else if(criterio=="nombre_prod")
    {
      this._productoService.searchProductByName(valor).subscribe(data=>{
        console.log(data);
      this.listProductos=data;
      },error=>{
        this.toastr.error('No se encontro el nombre del producto', 'Producto no encontrado');
        console.log(error);
      })
    }
    else
    {
      console.log(criterio)
      this.toastr.error('Debe seleccionar un criterio de busqueda', 'Busqueda fallida');

    }
  }

  
}
