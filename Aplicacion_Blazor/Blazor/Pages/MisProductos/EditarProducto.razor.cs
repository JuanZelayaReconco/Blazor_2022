using Blazor.Interfaces;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Modelos;

namespace Blazor.Pages.MisProductos
{
    partial class EditarProducto
    {
        [Inject] IProductoServicio productoServicio { get; set; }
        [Inject] NavigationManager navigationManager { get; set; }

        [Parameter] public string Codigo { get; set; }
        [Inject] private SweetAlertService Swal { get; set; }

        private Producto product = new Producto();

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(Codigo))
            {
                product = await productoServicio.GetPorCodigo(Codigo);
            }
        }

        protected async Task Guardar()
        {
            if (string.IsNullOrEmpty(product.Codigo) || string.IsNullOrEmpty(product.Descripcion))
            {
                await Swal.FireAsync("Atención!", "Debe ingresar el código y la descripción del producto", SweetAlertIcon.Info);
                return;
            }

            bool update = await productoServicio.Actualizar(product);

            if (update)
            {
                await Swal.FireAsync("Felicidades!", "Producto actualizado con éxito", SweetAlertIcon.Success);
            }
            else
            {
                await Swal.FireAsync("Error!!!", "No se pudo actualizar el producto", SweetAlertIcon.Error);
            }
            navigationManager.NavigateTo("/Productos");
        }

        protected async Task Cancelar()
        {
            navigationManager.NavigateTo("/Productos");
        }

        protected async Task Eliminar()
        {
            bool delete = false;

            SweetAlertResult result = await Swal.FireAsync(new SweetAlertOptions
            {
                Title = "¡Advertencia!",
                Text = "¿Está seguro que desea eliminar el producto?",
                Icon = SweetAlertIcon.Warning, //Puede ser "Question" en vez de "Warning".
                ShowCancelButton = true,
                ConfirmButtonText = "Sí",
                CancelButtonText = "No"
            });

            if (!string.IsNullOrEmpty(result.Value))
            {
                delete = await productoServicio.Eliminar(product);
                if (delete)
                {
                    await Swal.FireAsync("Hecho!", "Producto eliminado con éxito", SweetAlertIcon.Success);
                    navigationManager.NavigateTo("/Productos");
                }
                else
                {
                    await Swal.FireAsync("Error!!!", "No se pudo eliminar el producto", SweetAlertIcon.Error);
                }
            }
        }
    }
}
