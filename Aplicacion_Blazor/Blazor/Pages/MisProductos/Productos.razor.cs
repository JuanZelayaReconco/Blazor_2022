using Blazor.Interfaces;
using Microsoft.AspNetCore.Components;
using Modelos;

namespace Blazor.Pages.MisProductos
{
    partial class Productos
    {
        [Inject] private IProductoServicio productoServicio { get; set; }

        private IEnumerable<Producto> listaProductos { get; set; }

        protected override async Task OnInitializedAsync()
        {
            listaProductos = await productoServicio.GetLista();
        }
    }
}
