using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public class RepositorioInmueble : RepositorioBase, IRepositorioInmueble
    {
		
		public RepositorioInmueble(IConfiguration configuration): base(configuration)
        {
			
		}

		public int Alta(Inmueble entidad)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Inmueble (Direccion, Tipo, Ambientes, Uso, Precio, Estado, Imagen, PropietarioId) " +
					"VALUES (@direccion, @tipo, @ambientes, @uso, @precio, @estado, @imagen, @propietarioId);" +
					"SELECT SCOPE_IDENTITY();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
				using (var command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@direccion", entidad.Direccion);
					command.Parameters.AddWithValue("@tipo", entidad.Tipo);
					command.Parameters.AddWithValue("@ambientes", entidad.Ambientes);
					command.Parameters.AddWithValue("@uso", entidad.Uso);
					command.Parameters.AddWithValue("@precio", entidad.Precio);
					command.Parameters.AddWithValue("@estado", entidad.Estado);
					if (String.IsNullOrEmpty(entidad.Imagen))
						command.Parameters.AddWithValue("@imagen", DBNull.Value);
					else
						command.Parameters.AddWithValue("@imagen", entidad.Imagen);
					command.Parameters.AddWithValue("@propietarioId", entidad.PropietarioId);
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
				string sql = $"DELETE FROM Pago FROM Contrato c INNER JOIN Pago p ON c.Id = p.ContratoId WHERE ContratoId = {id} ;" +
					$"DELETE FROM Contrato WHERE InmuebleId = {id};" +
					$"DELETE FROM Inmueble WHERE Id = {id}";
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
		public int Modificacion(Inmueble entidad)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "UPDATE Inmueble SET " +
					"Direccion=@direccion, Tipo=@tipo, Ambientes=@ambientes, Uso=@uso, Precio=@precio, Estado=@estado, Imagen= @imagen, PropietarioId=@propietarioId " +
					"WHERE Id = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@direccion", entidad.Direccion);
					command.Parameters.AddWithValue("@tipo", entidad.Tipo);
					command.Parameters.AddWithValue("@ambientes", entidad.Ambientes);
					command.Parameters.AddWithValue("@uso", entidad.Uso);
					command.Parameters.AddWithValue("@precio", entidad.Precio);
					command.Parameters.AddWithValue("@estado", entidad.Estado);
					command.Parameters.AddWithValue("@imagen", entidad.Imagen);
					command.Parameters.AddWithValue("@propietarioId", entidad.PropietarioId);
					command.Parameters.AddWithValue("@id", entidad.Id);
					command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
		public IList<Inmueble> ObtenerTodos()
		{
			IList<Inmueble> res = new List<Inmueble>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "SELECT i.Id, Direccion, Tipo, Ambientes, Uso, Precio, i.Estado, Imagen, PropietarioId, p.Nombre, p.Apellido" +
					" FROM Inmueble i INNER JOIN Propietario p ON i.PropietarioId = p.Id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inmueble entidad = new Inmueble
						{
							Id = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							Tipo = reader.GetString(2),
							Ambientes = reader.GetInt32(3),
							Uso = reader.GetString(4),
							Precio = reader.GetDecimal(5),
							Estado = reader.GetInt32(6),
							Imagen = reader["Imagen"].ToString(),
							PropietarioId = reader.GetInt32(8),
							Duenio = new Propietario
							{
								Id = reader.GetInt32(8),
								Nombre = reader.GetString(9),
								Apellido = reader.GetString(10),
							}
						};
						res.Add(entidad);
					}
					connection.Close();
				}
			}
			return res;
		}
		public Inmueble ObtenerPorId(int id)
		{
			Inmueble entidad = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT i.Id, Direccion, Tipo, Ambientes, Uso, Precio, i.Estado, Imagen, PropietarioId, p.Nombre, p.Apellido" +
					$" FROM Inmueble i INNER JOIN Propietario p ON i.PropietarioId = p.Id " +
					$"WHERE i.Id=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						entidad = new Inmueble
						{
							Id = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							Tipo = reader.GetString(2),
							Ambientes = reader.GetInt32(3),
							Uso = reader.GetString(4),
							Precio = reader.GetDecimal(5),
							Estado = reader.GetInt32(6),
							Imagen = reader["Imagen"].ToString(),
							PropietarioId = reader.GetInt32(8),
							Duenio = new Propietario
							{
								Id = reader.GetInt32(8),
								Nombre = reader.GetString(9),
								Apellido = reader.GetString(10),
							}
						};
					}
					connection.Close();
				}
			}
			return entidad;
		}
		public IList<Inmueble> BuscarPorPropietario(int id)
		{
			List<Inmueble> res = new List<Inmueble>();
			Inmueble entidad = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT i.Id, Direccion, Tipo, Ambientes, Uso, Precio, i.Estado, Imagen, PropietarioId, p.Nombre, p.Apellido" +
					$" FROM Inmueble i INNER JOIN Propietario p ON i.PropietarioId = p.Id" +
					$" WHERE PropietarioId = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						entidad = new Inmueble 
						{
							Id = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							Tipo = reader.GetString(2),
							Ambientes = reader.GetInt32(3),
							Uso = reader.GetString(4),
							Precio = reader.GetDecimal(5),
							Estado = reader.GetInt32(6),
							Imagen = reader["Imagen"].ToString(),
							PropietarioId = reader.GetInt32(8),
							Duenio = new Propietario
							{
								Id = reader.GetInt32(8),
								Nombre = reader.GetString(9),
								Apellido = reader.GetString(10),
							}
						};
						res.Add(entidad);
					}
					connection.Close();
				}
			}
			return res;
		}
		public IList<Inmueble> ObtenerPorEstado() {
			IList<Inmueble> res = new List<Inmueble>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "SELECT i.Id, Direccion, Tipo, Ambientes, Uso, Precio, i.Estado, Imagen, PropietarioId, p.Nombre, p.Apellido" +
					" FROM Inmueble i INNER JOIN Propietario p ON i.PropietarioId = p.Id" +
					" WHERE Estado = 1";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inmueble entidad = new Inmueble
						{
							Id = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							Tipo = reader.GetString(2),
							Ambientes = reader.GetInt32(3),
							Uso = reader.GetString(4),
							Precio = reader.GetDecimal(5),
							Estado = reader.GetInt32(6),
							Imagen = reader["Imagen"].ToString(),
							PropietarioId = reader.GetInt32(8),
							Duenio = new Propietario
							{
								Id = reader.GetInt32(8),
								Nombre = reader.GetString(9),
								Apellido = reader.GetString(10),
							}
						};
						res.Add(entidad);
					}
					connection.Close();
				}
			}
			return res;
		}
		public IList<Inmueble> BuscarPorFechas(DateTime ingreso, DateTime salida) {
			List<Inmueble> res = new List<Inmueble>();
			Inmueble entidad = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT i.Id, Direccion, Tipo, Ambientes, Uso, i.Precio, i.Estado, Imagen, PropietarioId, p.Nombre, p.Apellido" +
					$" FROM Inmueble i INNER JOIN Propietario p ON i.PropietarioId = p.Id LEFT JOIN Contrato c ON i.Id= c.InmuebleId " +
					$" WHERE FechaInicio > @salida OR FechaFin < @ingreso OR c.Id IS NULL AND Estado = 1";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@ingreso", SqlDbType.Date).Value = ingreso.Date;
					command.Parameters.Add("@salida", SqlDbType.Date).Value = salida.Date;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						entidad = new Inmueble
						{
							Id = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							Tipo = reader.GetString(2),
							Ambientes = reader.GetInt32(3),
							Uso = reader.GetString(4),
							Precio = reader.GetDecimal(5),
							Estado = reader.GetInt32(6),
							Imagen = reader["Imagen"].ToString(),
							PropietarioId = reader.GetInt32(8),
							Duenio = new Propietario
							{
								Id = reader.GetInt32(8),
								Nombre = reader.GetString(9),
								Apellido = reader.GetString(10),
							}
						};
						res.Add(entidad);
					}
					connection.Close();
				}
			}
			return res;
		}
	}
}

