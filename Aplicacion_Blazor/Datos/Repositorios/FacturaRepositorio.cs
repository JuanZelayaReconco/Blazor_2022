using Dapper;
using Datos.Interfaces;
using Modelos;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Repositorios
{
    public class FacturaRepositorio : IFacturaRepositorio
    {
        private string CadenaConexion;
        public FacturaRepositorio(string cadenaConexion)
        {
            CadenaConexion = cadenaConexion;
        }

        private MySqlConnection Conexion()
        {
            return new MySqlConnection(CadenaConexion);
        }

        public async Task<int> Nueva(Factura factura)
        {
            int idFactura = 0;
            try
            {
                using MySqlConnection conexion = Conexion();
                await conexion.OpenAsync();
                string sql = @"INSERT INTO Factura (Fecha, Cliente, Subtotal, ISV, Descuento, Total, CodigoUsuario) 
                             VALUES(@Fecha, @Cliente, @Subtotal, @ISV, @Descuento, @Total, @CodigoUsuario); SELECT LAST_INSERT_ID()";
                //En la anterior instrucción podemos dar enter, y que siga siendo la misma y una
                //unica instrucción sin agregar "+", al ingresar una "@" al principio después del
                //igual.
                idFactura = Convert.ToInt32(await conexion.ExecuteScalarAsync(sql, factura));
            }
            catch (Exception)
            {
            }
            return idFactura;
        }
    }
}
