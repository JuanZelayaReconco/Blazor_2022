using Blazor.Interfaces;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Modelos;

namespace Blazor.Pages.MisProductos
{
    partial class NuevoProducto
    {
        [Inject] private IProductoServicio productoServicio { get; set; }
        [Inject] private SweetAlertService Swal { get; set; }
        [Inject] NavigationManager _navigationManager { get; set; }

        private Producto producto = new Producto();

        protected override async Task OnInitializedAsync()
        {
            
        }

        protected async Task Guardar()
        {
            if (string.IsNullOrEmpty(producto.Codigo) || string.IsNullOrEmpty(producto.Descripcion))
            {
                await Swal.FireAsync("Atención!", "Debe ingresar almenos el código y la descripción del producto", SweetAlertIcon.Info);
                return;
            }

            Producto productoExistente = new Producto();

            productoExistente = await productoServicio.GetPorCodigo(producto.Codigo);

            if (!string.IsNullOrEmpty(productoExistente.Codigo))
            {
                await Swal.FireAsync("Atención!", "Debe ingresar un código diferente a los ya existentes", SweetAlertIcon.Info);
                return;
            }

            bool insert = await productoServicio.Nuevo(producto);

            if (insert)
            {
                await Swal.FireAsync("Felicidades!", "Producto guardado con éxito", SweetAlertIcon.Success);
            }
            else
            {
                await Swal.FireAsync("Error!!!", "No se pudo guardar el producto", SweetAlertIcon.Error);
            }
            _navigationManager.NavigateTo("/Productos");
        }

        protected async Task Cancelar()
        {
            _navigationManager.NavigateTo("/Productos");
        }
    }
}
