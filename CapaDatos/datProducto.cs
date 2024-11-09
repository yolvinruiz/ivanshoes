using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;

namespace CapaDatos
{
    public class datProducto
    {
        #region singleton
        private static readonly datProducto _instancia = new datProducto();
        public static datProducto Instancia
        {
            get { return datProducto._instancia; }
        }
        #endregion singleton

        // Obtener listas para ComboBox
        public List<KeyValuePair<int, string>> ListarTiposProducto()
        {
            SqlCommand cmd = null;
            List<KeyValuePair<int, string>> lista = new List<KeyValuePair<int, string>>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("ObtenerTiposProducto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new KeyValuePair<int, string>(
                        Convert.ToInt32(dr["id_tipo_producto"]),
                        dr["Nombre"].ToString()
                    ));
                }
            }
            catch (Exception e) { throw e; }
            finally { cmd.Connection.Close(); }
            return lista;
        }
        public List<KeyValuePair<int, string>> ListarMarcas()
        {
            SqlCommand cmd = null;
            List<KeyValuePair<int, string>> lista = new List<KeyValuePair<int, string>>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("ObtenerMarcas", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new KeyValuePair<int, string>(
                        Convert.ToInt32(dr["id_Marca"]),
                        dr["Nombre"].ToString()
                    ));
                }
            }
            catch (Exception e) { throw e; }
            finally { cmd.Connection.Close(); }
            return lista;
        }

        public List<KeyValuePair<int, string>> ListarTallas()
        {
            SqlCommand cmd = null;
            List<KeyValuePair<int, string>> lista = new List<KeyValuePair<int, string>>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("ObtenerTallas", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new KeyValuePair<int, string>(
                        Convert.ToInt32(dr["id_Talla"]),
                        dr["Nombre"].ToString()
                    ));
                }
            }
            catch (Exception e) { throw e; }
            finally { cmd.Connection.Close(); }
            return lista;
        }

        public List<KeyValuePair<int, string>> ListarColores()
        {
            SqlCommand cmd = null;
            List<KeyValuePair<int, string>> lista = new List<KeyValuePair<int, string>>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("ObtenerColores", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new KeyValuePair<int, string>(
                        Convert.ToInt32(dr["id_Color"]),
                        dr["Nombre"].ToString()
                    ));
                }
            }
            catch (Exception e) { throw e; }
            finally { cmd.Connection.Close(); }
            return lista;
        }

        public List<KeyValuePair<int, string>> ListarCategorias()
        {
            SqlCommand cmd = null;
            List<KeyValuePair<int, string>> lista = new List<KeyValuePair<int, string>>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("ObtenerCategorias", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new KeyValuePair<int, string>(
                        Convert.ToInt32(dr["id_Categoria"]),
                        dr["Nombre"].ToString()
                    ));
                }
            }
            catch (Exception e) { throw e; }
            finally { cmd.Connection.Close(); }
            return lista;
        }
    
    // Implementar métodos similares para Marcas, Tallas, Colores y Categorías...

    public void InsertarProducto(entProducto producto)
        {
            SqlCommand cmd = null;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("InsertarProducto", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@nombre", producto.nombre);
                cmd.Parameters.AddWithValue("@precio", producto.precio);
                cmd.Parameters.AddWithValue("@stock", producto.stock);
                cmd.Parameters.AddWithValue("@NombreTipoProducto", producto.NombreTipoProducto);
                cmd.Parameters.AddWithValue("@NombreMarca", producto.NombreMarca);
                cmd.Parameters.AddWithValue("@NombreTalla", producto.NombreTalla);
                cmd.Parameters.AddWithValue("@NombreColor", producto.NombreColor);
                cmd.Parameters.AddWithValue("@NombreCategoria", producto.NombreCategoria);
                cmd.Parameters.AddWithValue("@Imagen", producto.Imagen ?? "");

                cn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e) { throw e; }
            finally { cmd.Connection.Close(); }
        }

        public entProducto BuscarProductoPorId(int idProducto)
        {
            SqlCommand cmd = null;
            entProducto producto = null;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("BuscarProductoPorId", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_producto", idProducto);

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    producto = new entProducto
                    {
                        id_producto = Convert.ToInt32(dr["id_producto"]),
                        nombre = dr["nombre"].ToString(),
                        precio = Convert.ToDouble(dr["precio"]),
                        stock = Convert.ToInt32(dr["stock"]),
                        id_tipo_producto = Convert.ToInt32(dr["id_tipo_producto"]),
                        id_marca = Convert.ToInt32(dr["id_marca"]),
                        id_talla = Convert.ToInt32(dr["id_talla"]),
                        id_color = Convert.ToInt32(dr["id_color"]),
                        id_categoria = Convert.ToInt32(dr["id_categoria"]),
                        NombreTipoProducto = dr["NombreTipoProducto"].ToString(),
                        NombreMarca = dr["NombreMarca"].ToString(),
                        NombreTalla =dr["NombreTalla"].ToString(),
                        NombreColor = dr["NombreColor"].ToString(),
                        NombreCategoria = dr["NombreCategoria"].ToString(),
                        Imagen = dr["Imagen"].ToString(),
                    };
                }
            }
            catch (Exception e) { throw e; }
            finally { cmd.Connection.Close(); }
            return producto;
        }

        public void ModificarProducto(entProducto producto)
        {
            SqlCommand cmd = null;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("ModificarProducto", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id_producto", producto.id_producto);
                cmd.Parameters.AddWithValue("@nombre", producto.nombre);
                cmd.Parameters.AddWithValue("@precio", producto.precio);
                cmd.Parameters.AddWithValue("@stock", producto.stock);
                cmd.Parameters.AddWithValue("@NombreTipoProducto", producto.NombreTipoProducto);
                cmd.Parameters.AddWithValue("@NombreMarca", producto.NombreMarca);
                cmd.Parameters.AddWithValue("@NombreTalla", producto.NombreTalla);
                cmd.Parameters.AddWithValue("@NombreColor", producto.NombreColor);
                cmd.Parameters.AddWithValue("@NombreCategoria", producto.NombreCategoria);
                cmd.Parameters.AddWithValue("@Imagen", producto.Imagen ?? "");

                cn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e) { throw e; }
            finally { cmd.Connection.Close(); }
        }

        public List<entProducto> ListarProducto()
        {
            SqlCommand cmd = null;
            List<entProducto> lista = new List<entProducto>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar(); //singleton
                cmd = new SqlCommand("MostrarProductos", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entProducto producto = new entProducto
                    {
                        id_producto = Convert.ToInt32(dr["id_producto"]),
                        nombre = dr["nombre"].ToString(),
                        stock = Convert.ToInt32(dr["stock"]),
                        precio = Convert.ToDouble(dr["precio"]),
                        id_tipo_producto = Convert.ToInt32(dr["id_tipo_producto"]),
                        id_marca = Convert.ToInt32(dr["id_marca"]),
                        id_talla = Convert.ToInt32(dr["id_talla"]),
                        id_color = Convert.ToInt32(dr["id_color"]),
                        id_categoria = Convert.ToInt32(dr["id_categoria"]),
                        Imagen = dr["Imagen"].ToString() // Ruta de la imagen
                    };
                    lista.Add(producto);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd?.Connection.Close();
            }
            return lista;
        }


        public Boolean EliminarProducto(entProducto Producto)
        {
            SqlCommand cmd = null;
            Boolean elimina = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("EliminarProducto", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id_producto", Producto.id_producto);

                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    elimina = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return elimina;
        }

        public List<entProducto> BuscarProductoConNombres(string termino)
        {
            SqlCommand cmd = null;
            List<entProducto> lista = new List<entProducto>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("BuscarProductosConNombres", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@termino", termino);

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entProducto producto = new entProducto();
                    producto.id_producto = Convert.ToInt32(dr["id_producto"]);
                    producto.nombre = dr["NombreProducto"].ToString();
                    producto.stock = Convert.ToInt32(dr["stock"]);
                    producto.precio = Convert.ToDouble(dr["precio"]);
                    producto.NombreTalla = dr["Talla"].ToString();
                    producto.NombreTipoProducto = dr["TipoProducto"].ToString();
                    producto.NombreMarca = dr["Marca"].ToString();
                    producto.NombreColor = dr["Color"].ToString();
                    producto.NombreCategoria = dr["Categoria"].ToString();
                    producto.Imagen = dr["Imagen"].ToString();
                    lista.Add(producto);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (cmd != null && cmd.Connection != null)
                    cmd.Connection.Close();
            }
            return lista;
        }
    }
}
