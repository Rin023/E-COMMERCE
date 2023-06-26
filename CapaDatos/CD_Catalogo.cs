using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using CapaEntidad;
using System.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CD_Catalogo
    {
        //listamos el catalogo
        public List<Catalogo> Listar()
        {

            List<Catalogo> lista = new List<Catalogo>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select id_catalogo,Nombre from CATALOGO";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Catalogo()
                            {

                                id_catalogo = dr["id_catalogo"].ToString(),
                                Nombre = dr["Nombre"].ToString()
                               
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Catalogo>();
            }

            return lista;
        }


    }
}
