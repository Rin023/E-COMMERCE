using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class CD_Persona
    {
        //listamos los datos de persona tanto admins como clientes
        public List<Persona> Listar()
        {
            List<Persona> lista = new List<Persona>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "SELECT *FROM PERSONA";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Persona()
                            {
                                id_persona=dr["id_persona"].ToString(),
                                ObTipoPersona = new TipoPersona() { id_tipo =  dr["id_tipo"].ToString() },
                                Nombre = dr["Nombre"].ToString(),
                                Apellido = dr["Apellido"].ToString(),
                                Direccion = dr["Direccion"].ToString(),
                                Telefono = dr["Telefono"].ToString(),
                                correo= dr["correo"].ToString()
                            });;
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Persona>();
            }

            return lista;

        }
    }
}

