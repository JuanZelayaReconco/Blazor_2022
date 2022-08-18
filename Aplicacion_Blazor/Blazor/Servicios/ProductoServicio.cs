using Blazor.Data;
using Blazor.Interfaces;
using Datos.Interfaces;
using Datos.Repositorios;
using Modelos;

namespace Blazor.Servicios
{
    public class ProductoServicio : IProductoServicio
    {
        private readonly MySQLConfiguracion config;
        private IProductoRepositorio productoRepositorio;

        public ProductoServicio(MySQLConfiguracion configuracion)
        {
            config = configuracion;
            productoRepositorio = new ProductoRepositorio(configuracion.CadenaConexion);
        }

        public async Task<bool> Actualizar(Producto producto)
        {
            return await productoRepositorio.Actualizar(producto);
        }

        public async Task<bool> Eliminar(Producto producto)
        {
            return await productoRepositorio.Eliminar(producto);
        }

        public async Task<IEnumerable<Producto>> GetLista()
        {
            return await productoRepositorio.GetLista();
        }

        public async Task<Producto> GetPorCodigo(string codigo)
        {
            return await productoRepositorio.GetPorCodigo(codigo);
        }

        public async Task<bool> Nuevo(Producto producto)
        {
            return await productoRepositorio.Nuevo(producto);
        }
    }
}
