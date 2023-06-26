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
    public class CD_TipoPersona
    {

        //listamos el tipo de persona si es un cliente o administrador
        public List<TipoPersona> Listar()
        {
            List<TipoPersona> lista = new List<TipoPersona>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "SELECT *FROM TIPO_PERSONA";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new TipoPersona()
                            {
                                id_tipo = dr["id_tipo"].ToString(),
                                tipo = dr["tipo"].ToString()

                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<TipoPersona>();
            }

            return lista;
        
        }
    }
}
