using Blazor.Interfaces;
using Microsoft.AspNetCore.Components;
using CurrieTechnologies.Razor.SweetAlert2;
using Modelos;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace Blazor.Pages.Facturacion
{
    partial class NuevaFactura
    {
        [Inject] private IFacturaServicio facturaServicio { get; set; }
        [Inject] private IProductoServicio productoServicio { get; set; }
        [Inject] private IDetalleFacturaServicio detalleFacturaServicio { get; set; }
        [Inject] private SweetAlertService Swal { get; set; }
        [Inject] private NavigationManager _navigationManager { get; set; }
        [Inject] private IHttpContextAccessor httpcontextAccessor { get; set; } //Nos sirve para 
        //acceder al código de usuario el cual está logueado.
        [Inject] private IUsuarioServicio _usuarioServicio { get; set; }

        private Factura factura = new Factura();
        private List<DetalleFactura> listaDetallefactura = new List<DetalleFactura>();
        private Usuario user = new Usuario();

        public Producto producto = new Producto();
        private string codigoProducto { get; set; }
        private string cantidad { get; set; }

        protected override async Task OnInitializedAsync()
        {
            user = await _usuarioServicio.GetPorCodigo(httpcontextAccessor.HttpContext.User.Identity.Name);
            factura.Fecha = DateTime.Now;
        }

        private async void BuscarProducto(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                producto = await productoServicio.GetPorCodigo(codigoProducto);
            }
        }

        protected async Task AgregarProducto(MouseEventArgs args)
        {
            if (args.Detail != 0)
            {
                if (!string.IsNullOrEmpty(producto.Descripcion) && !string.IsNullOrEmpty(cantidad))
                {
                    if (cantidad == "0")
                    {
                        await Swal.FireAsync("Atención!", "Debe ingresar al menos una unidad", SweetAlertIcon.Error);
                        return;
                    }

                    if (producto.Existencias >= Convert.ToInt32(cantidad))
                    {
                        DetalleFactura detalle = new DetalleFactura();
                        detalle.Producto = producto.Descripcion;
                        detalle.CodigoProducto = producto.Codigo;
                        detalle.Cantidad = Convert.ToInt32(cantidad);
                        detalle.Precio = producto.Precio;
                        detalle.Total = producto.Precio * Convert.ToInt32(cantidad);
                        listaDetallefactura.Add(detalle);

                        producto.Codigo = string.Empty;
                        producto.Descripcion = string.Empty;
                        producto.Precio = 0;
                        producto.Existencias = 0;
                        cantidad = string.Empty;
                        codigoProducto = string.Empty;

                        factura.SubTotal = factura.SubTotal + detalle.Total;
                        factura.ISV = factura.SubTotal * 0.15M;
                        factura.Total = factura.SubTotal + factura.ISV - factura.Descuento;
                    }
                    else
                    {
                        await Swal.FireAsync("Atención!", "No puede exceder el maximo de existencias", SweetAlertIcon.Warning);
                        return;
                    }
                    
                }
                else
                {
                    await Swal.FireAsync("Atención!", "Debe ingresar un código valido y al menos una unidad", SweetAlertIcon.Error);
                    return;
                }
            }
        }

        protected async Task Guardar()
        {
            factura.CodigoUsuario = httpcontextAccessor.HttpContext.User.Identity.Name;
            int idFactura = await facturaServicio.Nueva(factura);

            if (idFactura != 0)
            {
                foreach (var item in listaDetallefactura)
                {
                    item.IdFactura = idFactura;
                    await detalleFacturaServicio.Nuevo(item);
                }
                await Swal.FireAsync("Felicidades!", "La factura ha sido guardada exitosamente", SweetAlertIcon.Success);
            }
            else
            {
                await Swal.FireAsync("Error!!!", "No se pudo realizar la factura", SweetAlertIcon.Error);
            }
        }
    }
}
