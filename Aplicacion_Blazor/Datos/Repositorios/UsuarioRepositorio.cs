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
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private string CadenaConexion;
        public UsuarioRepositorio(string cadenaConexion)
        {
            CadenaConexion = cadenaConexion;
        }

        private MySqlConnection Conexion()
        {
            return new MySqlConnection(CadenaConexion);
        }

        public async Task<bool> Actualizar(Usuario usuario)
        {
            bool result = false;
            try
            {
                using MySqlConnection conexion = Conexion();
                await conexion.OpenAsync();
                string sql = @"UPDATE Usuario SET Nombre = @Nombre, Clave = @Clave, Rol = @Rol, 
                             EstaActivo = @EstaActivo WHERE Codigo = @Codigo;";
                //En la anterior instrucción podemos dar enter, y que siga siendo la misma y una
                //unica instrucción sin agregar "+", al ingresar una "@" al principio después del
                //igual.
                result = Convert.ToBoolean(await conexion.ExecuteAsync(sql, usuario));
            }
            catch (Exception)
            {
            }
            return result;
        }

        public async Task<bool> Eliminar(Usuario usuario)
        {
            bool result = false;
            try
            {
                using MySqlConnection conexion = Conexion();
                await conexion.OpenAsync();
                string sql = "DELETE FROM Usuario WHERE Codigo = @Codigo;";
                result = Convert.ToBoolean(await conexion.ExecuteAsync(sql, new {usuario.Codigo}));
            }
            catch (Exception)
            {
            }
            return result;
        }

        public async Task<IEnumerable<Usuario>> GetLista()
        {
            IEnumerable<Usuario> lista = new List<Usuario>();
            try
            {
                using MySqlConnection conexion = Conexion();
                await conexion.OpenAsync();
                string sql = "SELECT * FROM Usuario";
                lista = await conexion.QueryAsync<Usuario>(sql);
            }
            catch (Exception x)
            {
            }
            return lista;
        }

        public async Task<Usuario> GetPorCodigo(string codigo)
        {
            Usuario user = new Usuario();
            try
            {
                using MySqlConnection conexion = Conexion();
                await conexion.OpenAsync();
                string sql = "SELECT * FROM Usuario WHERE Codigo = @Codigo";
                user = await conexion.QueryFirstAsync<Usuario>(sql, new {codigo});
            }
            catch (Exception)
            {
            }
            return user;
        }

        public async Task<bool> Nuevo(Usuario usuario)
        {
            bool result = false;
            try
            {
                using MySqlConnection conexion = Conexion();
                await conexion.OpenAsync();
                string sql = @"INSERT INTO Usuario(Codigo, Nombre, Clave, Rol, 
                             EstaActivo) VALUES(@Codigo, @Nombre, @Clave, @Rol, @EstaActivo);";
                //En la anterior instrucción podemos dar enter, y que siga siendo la misma y una
                //unica instrucción sin agregar "+", al ingresar una "@" al principio después del
                //igual.
                result = Convert.ToBoolean(await conexion.ExecuteAsync(sql, usuario));
            }
            catch (Exception)
            {
            }
            return result;
        }
    }
}
