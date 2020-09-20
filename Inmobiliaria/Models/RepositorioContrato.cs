using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public class RepositorioContrato : RepositorioBase, IRepositorio<Contrato>
    {
		

        public RepositorioContrato(IConfiguration configuration) : base(configuration)
        {
			
		}

		public int Alta(Contrato entidad)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Contrato (FechaInicio, FechaFin, Precio, InmuebleId, InquilinoId) " +
					"VALUES (@inicio, @fin, @precio, @inmuebleId, @inquilinoId);" +
					"SELECT SCOPE_IDENTITY();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
				using (var command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@inicio", entidad.FechaInicio);
					command.Parameters.AddWithValue("@fin", entidad.FechaFin);
					command.Parameters.AddWithValue("@precio", entidad.Precio);
					command.Parameters.AddWithValue("@inmuebleId", entidad.InmuebleId);
					command.Parameters.AddWithValue("@inquilinoId", entidad.InquilinoId);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					entidad.Id = res;
					connection.Close();
				}
			}
			return res;
		}
		public int Baja(int id)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"DELETE FROM Pago WHERE ContratoId = {id};" +
					$"DELETE FROM Contrato WHERE Id = {id};";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
		public int Modificacion(Contrato entidad)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "UPDATE Contrato SET " +
					"FechaInicio=@inicio, FechaFin=@fin, Precio=@precio, InmuebleId=@inmuebleId, InquilinoId=@inquilinoId " +
					"WHERE Id = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@inicio", entidad.FechaInicio);
					command.Parameters.AddWithValue("@fin", entidad.FechaFin);
					command.Parameters.AddWithValue("@precio", entidad.Precio);
					command.Parameters.AddWithValue("@inmuebleId", entidad.InmuebleId);
					command.Parameters.AddWithValue("@inquilinoId", entidad.InquilinoId);
					command.Parameters.AddWithValue("@id", entidad.Id);
					command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
		public IList<Contrato> ObtenerTodos()
		{
			IList<Contrato> res = new List<Contrato>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "SELECT c.Id, FechaInicio, FechaFin, c.Precio, InmuebleId, p.Direccion, InquilinoId, i.Nombre, i.Apellido" +
					" FROM contrato c INNER JOIN inquilino i ON c.InquilinoId = i.Id INNER JOIN inmueble p ON c.InmuebleId = p.Id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Contrato entidad = new Contrato
						{
							Id = reader.GetInt32(0),
							FechaInicio = reader.GetDateTime(1),
							FechaFin = reader.GetDateTime(2),
							Precio = reader.GetDecimal(3),
							InmuebleId = reader.GetInt32(4),
							Inmueble = new Inmueble {
								Id = reader.GetInt32(4),
								Direccion = reader.GetString(5),
							},
							InquilinoId = reader.GetInt32(6),
							Inquilino = new Inquilino
							{
								Id = reader.GetInt32(6),
								Nombre = reader.GetString(7),
								Apellido = reader.GetString(8),
							}
						};
						res.Add(entidad);
					}
					connection.Close();
				}
			}
			return res;
		}
		public Contrato ObtenerPorId(int id)
		{
			Contrato entidad = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT c.Id, FechaInicio, FechaFin, c.Precio, InmuebleId, p.Direccion, InquilinoId, i.Nombre, i.Apellido" +
					$" FROM contrato c INNER JOIN inquilino i ON c.InquilinoId = i.Id INNER JOIN inmueble p ON c.InmuebleId = p.Id" +
					$" WHERE c.Id=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						entidad = new Contrato
						{
							Id = reader.GetInt32(0),
							FechaInicio = reader.GetDateTime(1),
							FechaFin = reader.GetDateTime(2),
							Precio = reader.GetDecimal(3),
							InmuebleId = reader.GetInt32(4),
							Inmueble = new Inmueble
							{
								Id = reader.GetInt32(4),
								Direccion = reader.GetString(5),
							},
							InquilinoId = reader.GetInt32(6),
							Inquilino = new Inquilino
							{
								Id = reader.GetInt32(6),
								Nombre = reader.GetString(7),
								Apellido = reader.GetString(8),
							}
						};
					}
					connection.Close();
				}
			}
			return entidad;
		}

	}
}
