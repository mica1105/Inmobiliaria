using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public class RepositorioPago
    {
		private readonly string connectionString;
		private readonly IConfiguration conf;
        public RepositorioPago(IConfiguration configuration)
        {
			this.conf = configuration;
			this.connectionString = conf["ConnectionStrings:DefaultConnection"];
		}

		public int Alta(Pago entidad)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Pago (NroPago, Fecha, Importe, ContratoId) " +
					"VALUES (@pago, @fecha, @importe, @contratoId);" +
					"SELECT SCOPE_IDENTITY();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
				using (var command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@pago", entidad.NroPago);
					command.Parameters.AddWithValue("@fecha", entidad.Fecha);
					command.Parameters.AddWithValue("@importe", entidad.Importe);
					command.Parameters.AddWithValue("@contratoId", entidad.ContratoId);
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
				string sql = $"DELETE FROM Pago WHERE Id = {id}";
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
		public int Modificacion(Pago entidad)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "UPDATE Pago SET " +
					"NroPago=@pago, Fecha=@fecha, Importe=@importe, ContratoId=@contratoId " +
					"WHERE Id = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@pago", entidad.NroPago);
					command.Parameters.AddWithValue("@fecha", entidad.Fecha);
					command.Parameters.AddWithValue("@importe", entidad.Importe);
					command.Parameters.AddWithValue("@contratoId", entidad.ContratoId);
					command.Parameters.AddWithValue("@id", entidad.Id);
					command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
		public IList<Pago> ObtenerTodos(int id)
		{
			IList<Pago> res = new List<Pago>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT p.Id, NroPago, Fecha, Importe, ContratoId, c.Precio, c.InmuebleId, c.InquilinoId" +
					$" FROM Pago p INNER JOIN Contrato c ON p.ContratoId = c.Id" +
					$" WHERE c.Id = @idContrato";
					
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@idContrato", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Pago entidad = new Pago
						{
							Id = reader.GetInt32(0),
							NroPago = reader.GetInt32(1),
							Fecha = reader.GetDateTime(2),
							Importe = reader.GetDecimal(3),
							ContratoId = reader.GetInt32(4),				
							Alquiler = new Contrato
							{
								Id = reader.GetInt32(4),
								Precio = reader.GetDecimal(5),
								InmuebleId = reader.GetInt32(6),
								InquilinoId = reader.GetInt32(7),
							}		
						};
						res.Add(entidad);
					}
					connection.Close();
				}
			}
			return res;
		}
		public Pago ObtenerPorId(int id)
		{
			Pago entidad = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT p.Id, NroPago, Fecha, Importe, ContratoId, c.Precio, c.InmuebleId, c.InquilinoId" +
					$" FROM Pago p INNER JOIN Contrato c ON p.ContratoId = c.Id" +
					$" WHERE p.Id=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						entidad = new Pago
						{
							Id = reader.GetInt32(0),
							NroPago = reader.GetInt32(1),
							Fecha = reader.GetDateTime(2),
							Importe = reader.GetDecimal(3),
							ContratoId = reader.GetInt32(4),
							Alquiler = new Contrato
							{
								Id = reader.GetInt32(4),
								Precio = reader.GetDecimal(5),
								InmuebleId = reader.GetInt32(6),
								InquilinoId = reader.GetInt32(7),
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
