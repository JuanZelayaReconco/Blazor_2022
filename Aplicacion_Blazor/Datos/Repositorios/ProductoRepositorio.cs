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
    public class ProductoRepositorio : IProductoRepositorio
    {
        private string CadenaConexion;
        public ProductoRepositorio(string cadenaConexion)
        {
            CadenaConexion = cadenaConexion;
        }

        private MySqlConnection Conexion()
        {
            return new MySqlConnection(CadenaConexion);
        }

        public async Task<bool> Actualizar(Producto producto)
        {
            bool result = false;
            try
            {
                using MySqlConnection conexion = Conexion();
                await conexion.OpenAsync();
                string sql = "UPDATE Producto SET Descripcion = @Descripcion, Existencias = @Existencias, Precio = @Precio WHERE Codigo = @Codigo;";
                result = Convert.ToBoolean(await conexion.ExecuteAsync(sql, producto));
            }
            catch (Exception)
            {
            }
            return result;
        }

        public async Task<bool> Eliminar(Producto producto)
        {
            bool result = false;
            try
            {
                using MySqlConnection conexion = Conexion();
                await conexion.OpenAsync();
                string sql = "DELETE FROM Producto WHERE Codigo = @Codigo;";
                result = Convert.ToBoolean(await conexion.ExecuteAsync(sql, new { producto.Codigo }));
            }
            catch (Exception)
            {
            }
            return result;
        }

        public async Task<IEnumerable<Producto>> GetLista()
        {
            IEnumerable<Producto> lista = new List<Producto>();
            try
            {
                using MySqlConnection conexion = Conexion();
                await conexion.OpenAsync();
                string sql = "SELECT * FROM Producto;";
                lista = await conexion.QueryAsync<Producto>(sql);
            }
            catch (Exception x)
            {
            }
            return lista;
        }

        public async Task<Producto> GetPorCodigo(string codigo)
        {
            Producto prod = new Producto();
            try
            {
                using MySqlConnection conexion = Conexion();
                await conexion.OpenAsync();
                string sql = "SELECT * FROM Producto WHERE Codigo = @Codigo;";
                prod = await conexion.QueryFirstAsync<Producto>(sql, new { codigo });
            }
            catch (Exception)
            {
            }
            return prod;
        }

        public async Task<bool> Nuevo(Producto producto)
        {
            bool result = false;
            try
            {
                using MySqlConnection conexion = Conexion();
                await conexion.OpenAsync();
                string sql = @"INSERT INTO Producto(Codigo, Descripcion, Existencias, Precio) 
                             VALUES(@Codigo, @Descripcion, @Existencias, @Precio);";
                //En la anterior instrucción podemos dar enter, y que siga siendo la misma y una
                //unica instrucción sin agregar "+", al ingresar una "@" al principio después del
                //igual.
                result = Convert.ToBoolean(await conexion.ExecuteAsync(sql, producto));
            }
            catch (Exception)
            {
            }
            return result;
        }
    }
}
