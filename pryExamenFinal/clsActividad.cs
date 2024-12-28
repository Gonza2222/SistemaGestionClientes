using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data; //proporciona las clases necesarias para trabajar con datos y bases de datos en general.
using System.Data.OleDb;//OleDb se usa para conectarse a varias fuentes de datos, como Access o Excel, utilizando controladores de OLE DB
using System.Windows.Forms;
using System.IO;

namespace pryExamenFinal
{
    internal class clsActividad
    {
        private OleDbConnection conexion = new OleDbConnection(); //este objeto permite conectarnos con la base de datos
        private OleDbCommand comando = new OleDbCommand();  //este objeto permite enviar ordenes a la base de datos
        private OleDbDataAdapter adaptador = new OleDbDataAdapter(); //este objeto permite adaptar los datos de la base de datos a datos reconocidos por .NET 

        private String CadenaConexion = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=BD_Clientes.mdb";
        private String Tabla = "Actividad";

        public void ListarActividad(ComboBox cmbActividad)
        {
            try
            {
                conexion.ConnectionString = CadenaConexion;
                conexion.Open();
                comando.Connection = conexion;
                comando.CommandType = CommandType.TableDirect;
                comando.CommandText = Tabla;
                adaptador = new OleDbDataAdapter(comando);
                DataSet ds = new DataSet();
                adaptador.Fill(ds, Tabla);
                cmbActividad.DataSource = ds.Tables[Tabla];
                cmbActividad.DisplayMember = "Nombre";
                cmbActividad.ValueMember = "idActividad";
                conexion.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }
        }

        public String Buscar(Int32 idActividad)
        {
            try
            {
                conexion.ConnectionString = CadenaConexion;// Esta línea asigna a conexion los parámetros de conexión para interactuar con la base de datos.
                conexion.Open();//Se abre la conexión con la base de datos para comenzar a trabajar con ella.
                comando.Connection = conexion;//Asocia el comando a la conexión abierta previamente.
                comando.CommandType = CommandType.TableDirect;//Se especifica que el comando accederá directamente a una tabla completa.
                comando.CommandText = Tabla;// Se establece el nombre de la tabla sobre la cual se ejecutará el comando.
                OleDbDataReader DR = comando.ExecuteReader(); //Se ejecuta el comando SQL y se obtiene un lector de datos que permite recorrer los resultados de la consulta.
                String Resultado = "";
                if (DR.HasRows)
                {
                    while (DR.Read())
                    {
                        if (DR.GetInt32(0) == idActividad)
                        {
                            Resultado = DR.GetString(1);
                        }
                    }
                }
                conexion.Close();// Cierra la conexión con la base de datos para liberar recursos.
                return Resultado;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
    }
}
