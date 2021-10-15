using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//Usados
using System.Data.SqlClient;
using System.IO;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Windows.Forms;

namespace SuperChivo
{
    /// <summary>
    /// Gestiona la conexion a la base de datos de Super Chivo, para realizar las respectivas consultas
    /// </summary>
    class ConexionBD
    {
        SqlConnection conexion;

        public ConexionBD()
        {
            conexion = new SqlConnection("Server=.; DataBase = SuperChivo ;Integrated Security=true");
        }

        public void Conectar()
        {
            conexion.Open();
        }
        public void Desconectar()
        {
            conexion.Close();
        }
        /// <summary>
        /// Retorna los valores si se ha encontrado los campos introducidos en la base de datos.
        /// Retorna null si no se ha encontrado ningun valor;
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public (string id, string password) ConsultaUsuarios(string id, string password)
        {
            string va1=null, va2 =null;
            string cad = string.Format(@"SELECT nombreEmpleado,cargo  FROM Empleados WHERE idUser = @id AND contracena = @contracena; ");
            SqlCommand consulta = new SqlCommand(cad, conexion);
            
            consulta.Parameters.AddWithValue("id", id);
            consulta.Parameters.AddWithValue("contracena", password);
            SqlDataAdapter adapatadorSQL = new SqlDataAdapter(consulta);
            DataTable Datos = new DataTable();
            adapatadorSQL.Fill(Datos);
            if (Datos.Rows.Count > 0)
            {
                va1= Datos.Rows[0][0].ToString();
                va2 = Datos.Rows[0][1].ToString();
            }
            return (va1, va2);

        }
        /// <summary>
        /// Metodo encargado de leer datos de la base de datos cuando el valor introducido coincida con
        /// algun valor en la base de datos.
        /// Si Producto es null no se encontro ningun resultado en la consulta
        /// </summary>
        /// <param name="codigo">Almacena el valor del codigo introducido por el usuario para su respectiva verififcacion</param>
        /// <returns></returns>
        public ( string producto, string interno, int descuento, double precio) ConsultarProductos(string codigo)
        {
            string  producto = null, interno = null;
            int descuento= 0;
            double precio = 0.0;
            string cad = string.Format(@"SELECT *  FROM Productos WHERE codigo = @codigo OR interno = @codigo; ");
            SqlCommand consulta = new SqlCommand(cad, conexion);

            consulta.Parameters.AddWithValue("codigo",codigo);
            SqlDataAdapter adapatadorSQL = new SqlDataAdapter(consulta);
            DataTable Datos = new DataTable();
            adapatadorSQL.Fill(Datos);
            if (Datos.Rows.Count > 0)
            {
                try
                {
                    interno = Datos.Rows[0][3].ToString();
                    producto = Datos.Rows[0][2].ToString();
                    descuento = Convert.ToInt16(Datos.Rows[0][4].ToString());
                    precio = Convert.ToDouble(Datos.Rows[0][5].ToString());
                }
                catch (Exception e) { MessageBox.Show("Error al convertir la cadena"); }
            }
            return (producto,interno,descuento,precio);

        }
    }
}
