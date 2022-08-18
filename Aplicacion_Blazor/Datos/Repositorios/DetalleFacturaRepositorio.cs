using Datos.Interfaces;
using Modelos;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Datos.Repositorios
{
    public class DetalleFacturaRepositorio : IDetalleFacturaRepositorio
    {
        private string CadenaConexion;
        public DetalleFacturaRepositorio(string cadenaConexion)
        {
            CadenaConexion = cadenaConexion;
        }

        private MySqlConnection Conexion()
        {
            return new MySqlConnection(CadenaConexion);
        }

        public async Task<bool> Nuevo(DetalleFactura detallefactura)
        {
            bool insert = false;
            try
            {
                using MySqlConnection conexion = Conexion();
                await conexion.OpenAsync();
                string sql = @"INSERT INTO DetalleFactura (IdFactura, CodigoProducto, Precio, Cantidad, Total) 
                             VALUES(@IdFactura, @CodigoProducto, @Precio, @Cantidad, @Total);";
                //En la anterior instrucción podemos dar enter, y que siga siendo la misma y una
                //unica instrucción sin agregar "+", al ingresar una "@" al principio después del
                //igual.
                insert = Convert.ToBoolean(await conexion.ExecuteAsync(sql, detallefactura));
            }
            catch (Exception)
            {
            }
            return insert;
        }
    }
}
