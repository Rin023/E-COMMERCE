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
    public class CD_Ubicacion
    {
        //listamos los departamentos y municipios para la tienda online
        public List<Departamento> ObtenerDepartamento()
        {
            List<Departamento> lista = new List<Departamento>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                   
                    SqlCommand cmd = new SqlCommand(" select * from DEPARTAMENTO", oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Departamento()
                            {
                             
                                IdDepartamento = dr["IdDepartamento"].ToString(),
                                Descripcion = dr["Descripcion"].ToString()

                            });

                        }
                    }
                }
            }
            catch(Exception e)
            {
                lista = new List<Departamento>();
            }
            return lista;

        }

        public List<Municipio> ObtenerMunicipio(string id_departamento)
        {
            List<Municipio> lista = new List<Municipio>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {

                    SqlCommand cmd = new SqlCommand("   select *from MUNICIPIO where IdDepartamento = @idDEP", oconexion);
                    cmd.Parameters.AddWithValue("@idDEP", id_departamento);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Municipio()
                            {
                                IdMunicipio = dr["IdMunicipio"].ToString(),
                                Descripcion = dr["Descripcion"].ToString()

                            });

                        }
                    }
                }
            }
            catch (Exception e)
            {
                lista = new List<Municipio>();
            }
            return lista;

        }

    }
}
