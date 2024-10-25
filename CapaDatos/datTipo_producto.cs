using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class datTipo_producto
    {
        #region sigleton
        public static readonly datTipo_producto _instancia = new datTipo_producto();
        public static datTipo_producto Instancia
        {
            get
            {
                return datTipo_producto._instancia;
            }
        }
        #endregion singleton
        public List<entTipo_producto> ListartipoProducto()
        {
            SqlCommand cmd = null;
            List<entTipo_producto> lista = new List<entTipo_producto>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar(); //singleton
                cmd = new SqlCommand("MostrarTipoProductos", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entTipo_producto tip = new entTipo_producto();
                    tip.id_tìpo_producto = Convert.ToInt32(dr["id_tipo_producto"]);
                    tip.Nombre = dr["Nombre"].ToString();
                    tip.Descripcion = dr["Descripcion"].ToString();
                    lista.Add(tip);
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
            return lista;
        }
        public Boolean Insertartipoproducto(entTipo_producto mep)
        {
            SqlCommand cmd = null;
            Boolean inserta = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("Insertartipoproducto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", mep.Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", mep.Descripcion);

                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    inserta = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cmd.Connection.Close(); }
            return inserta;
        }
        public Boolean Editartipoproducto(entTipo_producto etp)
        {
            SqlCommand cmd = null;
            Boolean edita = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("Editartipoproducto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_tipo_producto", etp.id_tìpo_producto);
                cmd.Parameters.AddWithValue("@Nombre", etp.Nombre);
                cmd.Parameters.AddWithValue("@descripcion", etp.Descripcion);
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    edita = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cmd.Connection.Close(); }
            return edita;
        }
        public Boolean Eliminartipoproducto(entTipo_producto ttra)
        {
            SqlCommand cmd = null;
            Boolean delete = false;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("Eliminartipoproducto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_tipo_producto", ttra.id_tìpo_producto);
                cn.Open();
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    delete = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { cmd.Connection.Close(); }
            return delete;
        }
    }
}
