using Blazor.Data;
using Blazor.Interfaces;
using Datos.Interfaces;
using Datos.Repositorios;
using Modelos;

namespace Blazor.Servicios
{
    public class DetalleFacturaServicio : IDetalleFacturaServicio
    {
        private readonly MySQLConfiguracion config;
        private IDetalleFacturaRepositorio detalleFacturaRepositorio;

        public DetalleFacturaServicio(MySQLConfiguracion configuracion)
        {
            config = configuracion;
            detalleFacturaRepositorio = new DetalleFacturaRepositorio(configuracion.CadenaConexion);
        }

        public async Task<bool> Nuevo(DetalleFactura detallefactura)
        {
            return await detalleFacturaRepositorio.Nuevo(detallefactura);
        }
    }
}
