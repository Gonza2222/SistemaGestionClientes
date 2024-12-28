using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data; //proporciona las clases necesarias para trabajar con datos y bases de datos en general.
using System.Data.OleDb;//OleDb se usa para conectarse a varias fuentes de datos, como Access o Excel, utilizando controladores de OLE DB
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;

namespace pryExamenFinal
{
    internal class clsSocio
    {
        private OleDbConnection conexion = new OleDbConnection(); //este objeto permite conectarnos con la base de datos
        private OleDbCommand comando = new OleDbCommand();  //este objeto permite enviar ordenes a la base de datos
        private OleDbDataAdapter adaptador = new OleDbDataAdapter(); //este objeto permite adaptar los datos de la base de datos a datos reconocidos por .NET 

        private String CadenaConexion = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=BD_Clientes.mdb";
        private String Tabla = "Socio";

        private Int32 cantidad;
        private Decimal deuMayor;
        private Decimal deuMenor;

        private Int32 idSoc;
        private String nom;
        private String dir;
        private Int32 idBar;
        private Int32 idAct;
        private Decimal deu;
        private String nomBar;
        private String nomAct;

        public Int32 DNI
        {
            get { return idSoc; }
            set { idSoc = value; }
        }

        public String Nombre
        {
            get { return nom; }
            set { nom = value; }
        }

        public String Direccion
        {
            get { return dir; }
            set { dir = value; }
        }

        public Int32 idBarrio
        {
            get { return idBar; }
            set { idBar = value; }
        }

        public String NombreBarrio
        {
            get { return nomBar; }
            set { nomBar = value; }
        }

        public Int32 idActividad
        {
            get { return idAct; }
            set { idAct = value; }
        }
        public String NombreActividad
        {
            get { return nomAct; }
            set { nomAct = value; }
        }

        public Decimal Deuda
        {
            get { return deu; }
            set { deu = value; }
        }

        public Int32 CantidadClientes
        {
            get { return cantidad; }
        }
        public Decimal Promedio
        {
            get { return deu / cantidad; }
        }

        public Decimal DeudaMayor
        {
            get { return deuMayor; }
        }

        public Decimal DeudaMenor
        {
            get { return deuMenor; }
        }

        public void AgregarNuevoSocio()
        {
            try
            {
                conexion.ConnectionString = CadenaConexion;// Establece la cadena de conexión y abre la conexión a la base de datos
                conexion.Open();
                comando.Connection = conexion;// Asocia el comando con la conexión abierta
                comando.CommandType = CommandType.TableDirect;// Configura el tipo de comando como TableDirect para acceder directamente a una tabla
                comando.CommandText = Tabla;// Establece el nombre de la tabla a la que se va a acceder
                adaptador = new OleDbDataAdapter(comando); // Crea un adaptador para transferir datos entre la base de datos y el DataSet
                DataSet ds = new DataSet();// Crear un nuevo DataSet para almacenar los datos
                adaptador.Fill(ds, Tabla);// Llenar el DataSet con los datos de la tabla especificada usando el adaptador
                DataTable DT = ds.Tables[Tabla];// Obtener la tabla especificada del DataSet
                DataRow fila = DT.NewRow();// Crear una nueva fila en la tabla
                fila["idSocio"] = idSoc;  // Asigna valores a las columnas de la nueva fila
                fila["Nombre"] = nom;
                fila["Direccion"] = dir;
                fila["idBarrio"] = idBar;
                fila["idActividad"] = idAct;
                fila["Deuda"] = 0;
                DT.Rows.Add(fila);// Agregar la nueva fila al DataTable
                OleDbCommandBuilder ConciliarCambios = new OleDbCommandBuilder(adaptador);// Crear un OleDbCommandBuilder para facilitar la reconciliación de cambios en la base de datos
                adaptador.Update(ds, Tabla);// Actualizar la base de datos con los cambios realizados en el DataSet
                conexion.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void BuscarCliente(Int32 DNI)
        {
            try
            {
                conexion.ConnectionString = CadenaConexion;// Establece la cadena de conexión y abre la conexión a la base de datos
                conexion.Open();
                comando.Connection = conexion;// Asocia el comando con la conexión abierta
                comando.CommandType = CommandType.TableDirect;// Configura el tipo de comando como TableDirect para acceder directamente a una tabla
                comando.CommandText = Tabla;// Establece el nombre de la tabla a la que se va a acceder
                OleDbDataReader DR = comando.ExecuteReader();
                clsBarrio objBarrio = new clsBarrio();
                clsActividad objActividad = new clsActividad();
                idSoc = 0;
                if (DR.HasRows)// Verifica si el DataReader tiene filas (es decir, si encontró registros en la consulta)
                {
                    while (DR.Read())// Recorre cada fila obtenida del DataReader
                    {
                        if (DR.GetInt32(0) == DNI) // Verifica si el ID del socio (columna 0) es igual del 'idSoc' ingresado
                        {
                            // Si el DNI coincide, asigna los valores de las columnas a las variables correspondientes
                            idSoc = DR.GetInt32(0); //Cuando se cumple la condicion pasa los datos a estas variables locales y privadas, que van a ser accedidas desde la interfaz grafica
                            nom = DR.GetString(1);
                            dir = DR.GetString(2);
                            nomBar = objBarrio.Buscar(DR.GetInt32(3));
                            nomAct = objActividad.Buscar(DR.GetInt32(4));
                            deu = DR.GetDecimal(5);
                        }
                    }
                }
                conexion.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        public void ModificarCliente(Int32 idSocio)
        {
            try
            {
                string sql = "";
                sql = "UPDATE SOCIO SET DEUDA = '";
                sql += deu.ToString();
                sql += "' WHERE IDSOCIO = " + idSocio.ToString();
                conexion.ConnectionString = CadenaConexion;// Establece la cadena de conexión y abre la conexión a la base de datos
                conexion.Open();
                comando.Connection = conexion;// Asocia el comando con la conexión abierta
                comando.CommandType = CommandType.Text;
                comando.CommandText = sql;
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void EliminarClientes(Int32 DNI)
        {
            try
            {
                string sql = "";
                sql = "DELETE * FROM SOCIO WHERE IDSOCIO = " + DNI.ToString();
                conexion.ConnectionString = CadenaConexion;// Establece la cadena de conexión y abre la conexión a la base de datos
                conexion.Open();
                comando.Connection = conexion;// Asocia el comando con la conexión abierta
                comando.CommandType = CommandType.Text;
                comando.CommandText = sql;
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void ListarSocio(ComboBox cmbCliente)
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
                cmbCliente.DataSource = ds.Tables[Tabla];
                cmbCliente.DisplayMember = "Nombre";
                cmbCliente.ValueMember = "idSocio";
                conexion.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }

        }

        public void ConsultarSocio(ComboBox cmbSocios)
        {
            try
            {
                conexion.ConnectionString = CadenaConexion;// Esta línea asigna a conexion los parámetros de conexión para interactuar con la base de datos.
                conexion.Open();//Se abre la conexión con la base de datos para comenzar a trabajar con ella.
                comando.Connection = conexion;//Asocia el comando a la conexión abierta previamente.
                comando.CommandType = CommandType.TableDirect;//Se especifica que el comando accederá directamente a una tabla completa.
                comando.CommandText = Tabla;// Se establece el nombre de la tabla sobre la cual se ejecutará el comando.
                OleDbDataReader DR = comando.ExecuteReader(); //Se ejecuta el comando SQL y se obtiene un lector de datos que permite recorrer los resultados de la consulta.
                clsBarrio objBarrio = new clsBarrio();
                clsActividad objActividad = new clsActividad();
                if (DR.HasRows)
                {
                    while (DR.Read())
                    {
                        if (DR.GetInt32(0) == (Int32)cmbSocios.SelectedValue)
                        {
                            idSoc = DR.GetInt32(0); //Cuando se cumple la condicion pasa los datos a estas variables locales y privadas, que van a ser accedidas desde la interfaz grafica
                            nom = DR.GetString(1);
                            dir = DR.GetString(2);
                            nomBar = objBarrio.Buscar(DR.GetInt32(3));
                            nomAct = objActividad.Buscar(DR.GetInt32(4));
                            deu = DR.GetDecimal(5);
                        }
                    }
                    conexion.Close();
                }
            }

            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void ListarCliente(DataGridView DgvCliente)
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
                deuMayor = decimal.MinValue;
                deuMenor = decimal.MaxValue;
                deu = 0;
                cantidad = 0;
                DgvCliente.Rows.Clear();
                if (ds.Tables[Tabla].Rows.Count > 0)
                {
                    foreach (DataRow fila in ds.Tables[Tabla].Rows)
                    {
                        DgvCliente.Rows.Add(fila["IdSocio"], fila["Nombre"], fila["Deuda"]);
                        if (Convert.ToDecimal(fila["Deuda"]) < deuMenor)
                        {
                            deuMenor = Convert.ToDecimal(fila["Deuda"]);
                        }
                        if (deuMayor < Convert.ToDecimal(fila["Deuda"]))
                        {
                            deuMayor = Convert.ToDecimal(fila["Deuda"]);
                        }
                        deu += Convert.ToDecimal(fila["Deuda"]);
                        cantidad++;
                    }
                }
                conexion.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        public void GenerarReporte(String nombreArchivo)
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
                StreamWriter ADClientes = new StreamWriter(nombreArchivo, false, Encoding.UTF8);
                ADClientes.WriteLine("Listado de clientes\n");
                ADClientes.WriteLine("Código;Nombre;Deuda");
                if (ds.Tables[Tabla].Rows.Count > 0)
                {
                    deu = 0;
                    cantidad = 0;
                    foreach (DataRow fila in ds.Tables[Tabla].Rows)
                    {
                        ADClientes.Write(fila["IdSocio"]);
                        ADClientes.Write(";");
                        ADClientes.Write(fila["Nombre"]);
                        ADClientes.Write(";");
                        ADClientes.Write("$");
                        ADClientes.WriteLine(fila["Deuda"]);
                        deu += Convert.ToDecimal(fila["Deuda"]);
                        cantidad++;
                    }
                    ADClientes.Write("Cantidad de clientes:;;");
                    ADClientes.WriteLine(cantidad);
                    ADClientes.Write("Total de deuda:;;$");
                    ADClientes.WriteLine(deu);
                }
                ADClientes.Close();
                ADClientes.Dispose();
                conexion.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void ImprimirReporte(PrintPageEventArgs reporte)
        {
            try
            {
                Int32 f = 100;
                Font titulo = new Font("Times New Roman", 26, FontStyle.Italic);
                Font subtitulos = new Font("Times New Roman", 16, FontStyle.Italic);
                Font tipoLetra = new Font("Times New Roman", 14);

                using (OleDbConnection conexion = new OleDbConnection(CadenaConexion))
                {
                    conexion.Open();
                    using (OleDbCommand comando = new OleDbCommand())
                    {
                        comando.Connection = conexion;
                        comando.CommandType = CommandType.TableDirect;
                        comando.CommandText = Tabla;

                        using (OleDbDataAdapter adaptador = new OleDbDataAdapter(comando))
                        {
                            DataSet ds = new DataSet();
                            adaptador.Fill(ds, Tabla);

                            reporte.Graphics.DrawString("Clientes", titulo, Brushes.Black, 285, 20);
                            reporte.Graphics.DrawString("Documento", subtitulos, Brushes.Black, 25, 70);
                            reporte.Graphics.DrawString("Cliente", subtitulos, Brushes.Black, 150, 70);
                            reporte.Graphics.DrawString("Deuda", subtitulos, Brushes.Black, 365, 70);

                            foreach (DataRow fila in ds.Tables[Tabla].Rows)
                            {
                                string idSocio = fila["IdSocio"]?.ToString() ?? "N/A";
                                string nombre = fila["Nombre"]?.ToString() ?? "Sin nombre";
                                string deuda = fila["Deuda"]?.ToString() ?? "0.00";

                                reporte.Graphics.DrawString(idSocio, tipoLetra, Brushes.DarkBlue, 50, f);
                                reporte.Graphics.DrawString(nombre, tipoLetra, Brushes.DarkBlue, 150, f);
                                reporte.Graphics.DrawString("$", tipoLetra, Brushes.DarkBlue, 365, f);
                                reporte.Graphics.DrawString(deuda, tipoLetra, Brushes.DarkBlue, 375, f);
                                f += 25;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
            }
        }

        public void ListarClientesDeudores(DataGridView DgvCliente)
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
                deuMayor = decimal.MinValue;
                deuMenor = decimal.MaxValue;
                deu = 0;
                cantidad = 0;
                if (ds.Tables[Tabla].Rows.Count > 0)
                {
                    foreach (DataRow fila in ds.Tables[Tabla].Rows)
                    {
                        if (Convert.ToDecimal(fila["Deuda"]) > 0)
                        {
                            DgvCliente.Rows.Add(fila["IdSocio"], fila["Nombre"], fila["Deuda"]);
                            if (Convert.ToDecimal(fila["Deuda"]) < deuMenor)
                            {
                                deuMenor = Convert.ToDecimal(fila["Deuda"]);
                            }
                            if (deuMayor < Convert.ToDecimal(fila["Deuda"]))
                            {
                                deuMayor = Convert.ToDecimal(fila["Deuda"]);
                            }
                            deu += Convert.ToDecimal(fila["Deuda"]);
                            cantidad++;
                        }

                    }
                }
                conexion.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        public void ReporteClientesDeudores(String nombreArchivo)
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
                StreamWriter ADClientes = new StreamWriter(nombreArchivo, false, Encoding.UTF8);
                ADClientes.WriteLine("Listado de clientes deudores\n");
                ADClientes.WriteLine("Código;Nombre;Deuda");
                if (ds.Tables[Tabla].Rows.Count > 0)
                {
                    deu = 0;
                    cantidad = 0;
                    foreach (DataRow fila in ds.Tables[Tabla].Rows)
                    {
                        if (Convert.ToDecimal(fila["Deuda"]) > 0)
                        {
                            ADClientes.Write(fila["IdSocio"]);
                            ADClientes.Write(";");
                            ADClientes.Write(fila["Nombre"]);
                            ADClientes.Write(";");
                            ADClientes.Write("$");
                            ADClientes.WriteLine(fila["Deuda"]);
                            deu += Convert.ToDecimal(fila["Deuda"]);
                            cantidad++;
                        }
                    }
                    ADClientes.Write("Cantidad de clientes deudores:;;");
                    ADClientes.WriteLine(cantidad);
                    ADClientes.Write("Total de deuda:;;$");
                    ADClientes.WriteLine(deu);
                }
                ADClientes.Close();
                ADClientes.Dispose();
                conexion.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void ImprimirReporteDeudores(PrintPageEventArgs reporte)
        {
            try
            {
                Int32 f = 100; 
                Font titulo = new Font("Times New Roman", 26, FontStyle.Italic);
                Font subtitulos = new Font("Times New Roman", 16, FontStyle.Italic);
                Font tipoLetra = new Font("Times New Roman", 14);
                using (OleDbConnection conexion = new OleDbConnection(CadenaConexion))
                {
                    conexion.Open();
                    using (OleDbCommand comando = new OleDbCommand())
                    {
                        comando.Connection = conexion;
                        comando.CommandType = CommandType.TableDirect;
                        comando.CommandText = Tabla;
                        using (OleDbDataAdapter adaptador = new OleDbDataAdapter(comando))
                        {
                            DataSet ds = new DataSet();
                            adaptador.Fill(ds, Tabla);
                            reporte.Graphics.DrawString("Clientes deudores", titulo, Brushes.Black, 285, 20);
                            reporte.Graphics.DrawString("Documento", subtitulos, Brushes.Black, 25, 70);
                            reporte.Graphics.DrawString("Cliente", subtitulos, Brushes.Black, 150, 70);
                            reporte.Graphics.DrawString("Deuda", subtitulos, Brushes.Black, 365, 70);

                            foreach (DataRow fila in ds.Tables[Tabla].Rows)
                            {
                                // Validación segura de la columna "Deuda"
                                if (fila["Deuda"] != DBNull.Value && Convert.ToDecimal(fila["Deuda"]) > 0)
                                {
                                    string idSocio = fila["IdSocio"]?.ToString() ?? "N/A";
                                    string nombre = fila["Nombre"]?.ToString() ?? "Sin nombre";
                                    string deuda = fila["Deuda"]?.ToString() ?? "0.00";

                                    reporte.Graphics.DrawString(idSocio, tipoLetra, Brushes.DarkBlue, 50, f);
                                    reporte.Graphics.DrawString(nombre, tipoLetra, Brushes.DarkBlue, 150, f);
                                    reporte.Graphics.DrawString("$", tipoLetra, Brushes.DarkBlue, 365, f);
                                    reporte.Graphics.DrawString(deuda, tipoLetra, Brushes.DarkBlue, 375, f);

                                    f += 25; // Incrementar la posición vertical para la siguiente fila
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
            }
        }

        public void ListarClienteActividad(DataGridView DgvCliente, Int32 idActividad)
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
                deuMayor = decimal.MinValue;
                deuMenor = decimal.MaxValue;
                deu = 0;
                cantidad = 0;
                DgvCliente.Rows.Clear();
                if (ds.Tables[Tabla].Rows.Count > 0)
                {
                    foreach (DataRow fila in ds.Tables[Tabla].Rows)
                    {
                        if (Convert.ToInt32(fila["idActividad"]) == idActividad)
                        {
                            DgvCliente.Rows.Add(fila["IdSocio"], fila["Nombre"], fila["Deuda"]);
                            if (Convert.ToDecimal(fila["Deuda"]) < deuMenor)
                            {
                                deuMenor = Convert.ToDecimal(fila["Deuda"]);
                            }
                            if (deuMayor < Convert.ToDecimal(fila["Deuda"]))
                            {
                                deuMayor = Convert.ToDecimal(fila["Deuda"]);
                            }
                            deu += Convert.ToDecimal(fila["Deuda"]);
                            cantidad++;
                        }
                    }
                }
                conexion.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void ReporteClientesActividad(String nombreArchivo, Int32 idActividad)
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
                StreamWriter ADClientes = new StreamWriter(nombreArchivo, false, Encoding.UTF8);
                ADClientes.WriteLine("Listado de clientes por actividad\n");
                ADClientes.WriteLine("Código;Nombre;Deuda");
                if (ds.Tables[Tabla].Rows.Count > 0)
                {
                    deu = 0;
                    cantidad = 0;
                    foreach (DataRow fila in ds.Tables[Tabla].Rows)
                    {
                        if (Convert.ToDecimal(fila["idActividad"]) == idActividad)
                        {
                            ADClientes.Write(fila["IdSocio"]);
                            ADClientes.Write(";");
                            ADClientes.Write(fila["Nombre"]);
                            ADClientes.Write(";");
                            ADClientes.Write("$");
                            ADClientes.WriteLine(fila["Deuda"]);
                            deu += Convert.ToDecimal(fila["Deuda"]);
                            cantidad++;
                        }
                    }
                    ADClientes.Write("Cantidad de clientes por actividad:;;");
                    ADClientes.WriteLine(cantidad);
                    ADClientes.Write("Total de deuda:;;$");
                    ADClientes.WriteLine(deu);
                }
                ADClientes.Close();
                ADClientes.Dispose();
                conexion.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void ImprimirReporteActividad(PrintPageEventArgs reporte, Int32 idActividad)
        {
            try
            {
                Int32 f = 100;
                Font titulo = new Font("Times New Roman", 26, FontStyle.Italic);
                Font subtitulos = new Font("Times New Roman", 16, FontStyle.Italic);
                Font tipoLetra = new Font("Times New Roman", 14);

                using (OleDbConnection conexion = new OleDbConnection(CadenaConexion))
                {
                    conexion.Open();
                    comando.Connection = conexion;
                    comando.CommandType = CommandType.TableDirect;
                    comando.CommandText = Tabla;
                    adaptador = new OleDbDataAdapter(comando);
                    DataSet ds = new DataSet();
                    adaptador.Fill(ds, Tabla);
                    reporte.Graphics.DrawString("Clientes por actividad", titulo, Brushes.Black, 250, 20);
                    reporte.Graphics.DrawString("Documento", subtitulos, Brushes.Black, 25, 70);
                    reporte.Graphics.DrawString("Cliente", subtitulos, Brushes.Black, 150, 70);
                    reporte.Graphics.DrawString("Deuda", subtitulos, Brushes.Black, 365, 70);
                    foreach (DataRow fila in ds.Tables[Tabla].Rows)
                    {
                        if (fila["idActividad"] == DBNull.Value || Convert.ToInt32(fila["idActividad"]) != idActividad)
                            continue;
                        string idSocio = fila["IdSocio"]?.ToString() ?? "N/A";
                        string nombre = fila["Nombre"]?.ToString() ?? "Sin nombre";
                        string deuda = fila["Deuda"]?.ToString() ?? "0.00";

                        reporte.Graphics.DrawString(idSocio, tipoLetra, Brushes.DarkBlue, 50, f);
                        reporte.Graphics.DrawString(nombre, tipoLetra, Brushes.DarkBlue, 150, f);
                        reporte.Graphics.DrawString("$", tipoLetra, Brushes.DarkBlue, 365, f);
                        reporte.Graphics.DrawString(deuda, tipoLetra, Brushes.DarkBlue, 375, f);

                        f += 25;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
            }
        }

        public void ListarClienteBarrio(DataGridView DgvCliente, Int32 idBarrio)
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
                deu = 0;
                cantidad = 0;
                DgvCliente.Rows.Clear();
                if (ds.Tables[Tabla].Rows.Count > 0)
                {
                    foreach (DataRow fila in ds.Tables[Tabla].Rows)
                    {
                        if (Convert.ToInt32(fila["idBarrio"]) == idBarrio)
                        {
                            DgvCliente.Rows.Add(fila["IdSocio"], fila["Nombre"], fila["Deuda"]);
                            deu += Convert.ToDecimal(fila["Deuda"]);
                            cantidad++;
                        }
                    }
                }
                conexion.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void ReporteClientesBarrio(String nombreArchivo, Int32 idBarrio)
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
                StreamWriter ADClientes = new StreamWriter(nombreArchivo, false, Encoding.UTF8);
                ADClientes.WriteLine("Listado de clientes por barrio\n");
                ADClientes.WriteLine("Código;Nombre;Deuda");
                if (ds.Tables[Tabla].Rows.Count > 0)
                {
                    deu = 0;
                    cantidad = 0;
                    foreach (DataRow fila in ds.Tables[Tabla].Rows)
                    {
                        if (Convert.ToDecimal(fila["idBarrio"]) == idBarrio)
                        {
                            ADClientes.Write(fila["IdSocio"]);
                            ADClientes.Write(";");
                            ADClientes.Write(fila["Nombre"]);
                            ADClientes.Write(";");
                            ADClientes.Write("$");
                            ADClientes.WriteLine(fila["Deuda"]);
                            deu += Convert.ToDecimal(fila["Deuda"]);
                            cantidad++;
                        }
                    }
                    ADClientes.Write("Cantidad de clientes por barrio:;;");
                    ADClientes.WriteLine(cantidad);
                    ADClientes.Write("Total de deuda:;;$");
                    ADClientes.WriteLine(deu);
                }
                ADClientes.Close();
                ADClientes.Dispose();
                conexion.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void ImprimirReporteBarrio(PrintPageEventArgs reporte, Int32 idBarrio)
        {
            try
            {
                Int32 f = 100;
                Font titulo = new Font("Times New Roman", 26, FontStyle.Italic);
                Font subtitulos = new Font("Times New Roman", 16, FontStyle.Italic);
                Font tipoLetra = new Font("Times New Roman", 14);

                using (OleDbConnection conexion = new OleDbConnection(CadenaConexion))
                {
                    conexion.Open();
                    comando.Connection = conexion;
                    comando.CommandType = CommandType.TableDirect;
                    comando.CommandText = Tabla;
                    adaptador = new OleDbDataAdapter(comando);
                    DataSet ds = new DataSet();
                    adaptador.Fill(ds, Tabla);
                    reporte.Graphics.DrawString("Clientes por barrio", titulo, Brushes.Black, 250, 20);
                    reporte.Graphics.DrawString("Documento", subtitulos, Brushes.Black, 25, 70);
                    reporte.Graphics.DrawString("Cliente", subtitulos, Brushes.Black, 150, 70);
                    reporte.Graphics.DrawString("Deuda", subtitulos, Brushes.Black, 365, 70);

                    foreach (DataRow fila in ds.Tables[Tabla].Rows)
                    {
                        if (fila["idBarrio"] == DBNull.Value || Convert.ToInt32(fila["idBarrio"]) != idBarrio)
                            continue;

                        string idSocio = fila["IdSocio"]?.ToString() ?? "N/A";
                        string nombre = fila["Nombre"]?.ToString() ?? "Sin nombre";
                        string deuda = fila["Deuda"]?.ToString() ?? "0.00";

                        reporte.Graphics.DrawString(idSocio, tipoLetra, Brushes.DarkBlue, 50, f);
                        reporte.Graphics.DrawString(nombre, tipoLetra, Brushes.DarkBlue, 150, f);
                        reporte.Graphics.DrawString("$", tipoLetra, Brushes.DarkBlue, 365, f);
                        reporte.Graphics.DrawString(deuda, tipoLetra, Brushes.DarkBlue, 375, f);

                        f += 25;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
            }
        }
    }
}
