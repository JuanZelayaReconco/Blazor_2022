using Blazor.Data;
using Blazor.Interfaces;
using Datos.Interfaces;
using Datos.Repositorios;
using Modelos;

namespace Blazor.Servicios
{
    public class FacturaServicio : IFacturaServicio
    {
        private readonly MySQLConfiguracion config;
        private IFacturaRepositorio facturaRepositorio;

        public FacturaServicio(MySQLConfiguracion configuracion)
        {
            config = configuracion;
            facturaRepositorio = new FacturaRepositorio(configuracion.CadenaConexion);
        }

        public async Task<int> Nueva(Factura factura)
        {
            return await facturaRepositorio.Nueva(factura);
        }
    }
}
