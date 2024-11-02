using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class datDashboard
    {
        #region singleton
        private static readonly datDashboard _instancia = new datDashboard();
        public static datDashboard Instancia
        {
            get { return datDashboard._instancia; }
        }
        #endregion

        public EstadisticasVenta ObtenerEstadisticas(DateTime fechaInicio, DateTime fechaFin)
        {
            SqlCommand cmd = null;
            EstadisticasVenta estadisticas = new EstadisticasVenta();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("ObtenerEstadisticasPeriodo", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Convertir las fechas al formato que SQL Server espera
                cmd.Parameters.Add("@FechaInicio", SqlDbType.DateTime).Value = fechaInicio;
                cmd.Parameters.Add("@FechaFin", SqlDbType.DateTime).Value = fechaFin;

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    estadisticas.NumeroVentas = dr["NumeroVentas"] != DBNull.Value ?
                        Convert.ToInt32(dr["NumeroVentas"]) : 0;

                    estadisticas.VentasTotales = dr["VentasTotales"] != DBNull.Value ?
                        Convert.ToDecimal(dr["VentasTotales"]) : 0;

                    estadisticas.ProductosVendidos = dr["ProductosVendidos"] != DBNull.Value ?
                        Convert.ToInt32(dr["ProductosVendidos"]) : 0;

                    estadisticas.TicketPromedio = dr["TicketPromedio"] != DBNull.Value ?
                        Convert.ToDecimal(dr["TicketPromedio"]) : 0;
                }
            }
            catch (Exception e)
            {
                // Es buena práctica registrar el error antes de relanzarlo
                System.Diagnostics.Debug.WriteLine($"Error en ObtenerEstadisticas: {e.Message}");
                throw;
            }
            finally
            {
                if (cmd != null && cmd.Connection != null && cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }
            return estadisticas;
        }
        public List<VentaDiaria> ObtenerVentasDiarias(DateTime fechaInicio, DateTime fechaFin)
        {
            SqlCommand cmd = null;
            List<VentaDiaria> ventas = new List<VentaDiaria>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("ObtenerVentasDiarias", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@FechaInicio", SqlDbType.DateTime).Value = fechaInicio;
                cmd.Parameters.Add("@FechaFin", SqlDbType.DateTime).Value = fechaFin;

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    VentaDiaria venta = new VentaDiaria
                    {
                        Fecha = Convert.ToDateTime(dr["Fecha"]),
                        NumeroVentas = Convert.ToInt32(dr["NumeroVentas"]),
                        MontoTotal = Convert.ToDecimal(dr["MontoTotal"])
                    };
                    ventas.Add(venta);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Error en ObtenerVentasDiarias: {e.Message}");
                throw;
            }
            finally
            {
                if (cmd != null && cmd.Connection != null && cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }
            return ventas;
        }

        public List<TopProducto> ObtenerTopProductos(DateTime fechaInicio, DateTime fechaFin, int cantidad = 10)
        {
            var productos = new List<TopProducto>();
            SqlCommand cmd = null;
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("ObtenerTopProductos", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                cmd.Parameters.AddWithValue("@FechaFin", fechaFin);
                cmd.Parameters.AddWithValue("@Cantidad", cantidad);

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    productos.Add(new TopProducto
                    {
                        Nombre = dr["Nombre"].ToString(),
                        Cantidad = Convert.ToInt32(dr["Cantidad"]),
                        Total = Convert.ToDecimal(dr["Total"]),
                    });
                }
            }
            catch (Exception e) { throw e; }
            finally { if (cmd != null) cmd.Connection.Close(); }
            return productos;
        }
        public List<VentaCategoria> ObtenerVentasCategoria()
        {
            SqlCommand cmd = null;
            List<VentaCategoria> lista = new List<VentaCategoria>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("ObtenerVentasPorCategoria", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    VentaCategoria ventaCategoria = new VentaCategoria
                    {
                        Categoria = dr["Categoria"].ToString(),
                        TotalVentas = Convert.ToInt32(dr["TotalVentas"]),
                        MontoTotal = Convert.ToDecimal(dr["MontoTotal"])
                    };
                    lista.Add(ventaCategoria);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Connection.Close();
                }
            }
            return lista;
        }
        public List<EstadisticaProducto> ObtenerEstadisticasProductos(DateTime fechaInicio, DateTime fechaFin)
        {
            SqlCommand cmd = null;
            List<EstadisticaProducto> lista = new List<EstadisticaProducto>();
            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand("ObtenerEstadisticasProductos", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Agregar parámetros
                cmd.Parameters.Add("@FechaInicio", SqlDbType.Date).Value = fechaInicio;
                cmd.Parameters.Add("@FechaFin", SqlDbType.Date).Value = fechaFin;

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    EstadisticaProducto estadistica = new EstadisticaProducto
                    {
                        IdProducto = Convert.ToInt32(dr["id_producto"]),
                        NombreProducto = dr["NombreProducto"].ToString(),
                        PrecioUnitario = Convert.ToDecimal(dr["PrecioUnitario"]),
                        CostoUnitario = Convert.ToDecimal(dr["CostoUnitario"]),
                        UnidadesVendidas = dr["UnidadesVendidas"] != DBNull.Value ?
                            Convert.ToInt32(dr["UnidadesVendidas"]) : 0,
                        MontoVendido = dr["MontoVendido"] != DBNull.Value ?
                            Convert.ToDecimal(dr["MontoVendido"]) : 0,
                        Ganancia = dr["Ganancia"] != DBNull.Value ?
                            Convert.ToDecimal(dr["Ganancia"]) : 0
                    };
                    lista.Add(estadistica);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (cmd != null && cmd.Connection != null && cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }
            return lista;
        }

        // Método adicional para obtener el resumen total
        public (decimal TotalVendido, decimal GananciaTotal) ObtenerResumenTotal(DateTime fechaInicio, DateTime fechaFin)
        {
            SqlCommand cmd = null;
            decimal totalVendido = 0;
            decimal gananciaTotal = 0;

            try
            {
                SqlConnection cn = Conexion.Instancia.Conectar();
                cmd = new SqlCommand(@"
                SELECT 
                    SUM(dv.Subtotal) as TotalVendido,
                    SUM(dv.Subtotal - (p.CostoUnitario * dv.Cantidad)) as GananciaTotal
                FROM Venta v
                INNER JOIN Detalle_venta dv ON v.ID_venta = dv.id_Venta
                INNER JOIN Producto p ON dv.id_Producto = p.id_producto
                WHERE v.fecha BETWEEN @FechaInicio AND @FechaFin
                AND v.Estado = 1", cn);

                cmd.Parameters.Add("@FechaInicio", SqlDbType.Date).Value = fechaInicio;
                cmd.Parameters.Add("@FechaFin", SqlDbType.Date).Value = fechaFin;

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    totalVendido = dr["TotalVendido"] != DBNull.Value ?
                        Convert.ToDecimal(dr["TotalVendido"]) : 0;
                    gananciaTotal = dr["GananciaTotal"] != DBNull.Value ?
                        Convert.ToDecimal(dr["GananciaTotal"]) : 0;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (cmd != null && cmd.Connection != null && cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }

            return (totalVendido, gananciaTotal);
        }
    }
}
public class EstadisticaProducto
{
    public int IdProducto { get; set; }
    public string NombreProducto { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal CostoUnitario { get; set; }
    public int UnidadesVendidas { get; set; }
    public decimal MontoVendido { get; set; }
    public decimal Ganancia { get; set; }
}
public class EstadisticasVenta
    {
        public int NumeroVentas { get; set; }
        public decimal VentasTotales { get; set; }
        public int ProductosVendidos { get; set; }
        public decimal TicketPromedio { get; set; }
    }

    public class VentaDiaria
    {
        public DateTime Fecha { get; set; }
        public int NumeroVentas { get; set; }
        public decimal MontoTotal { get; set; }
    }

    public class TopProducto
    {
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
    }

    public class VentaCategoria
    {
        public string Categoria { get; set; }
        public int TotalVentas { get; set; }
        public decimal MontoTotal { get; set; }
    }


