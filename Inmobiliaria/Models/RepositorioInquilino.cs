using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_.Net_Core.Models
{
	public class RepositorioInquilino 
	{
		private readonly string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog = Inmobiliaria; Integrated Security = True;";

		public int Alta(Inquilino e)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Inquilino (Nombre, Apellido, Dni, LugarTrabajo, Telefono, Email, NombreGarante, DniGarante, TelefonoGarante) " +
					$"VALUES (@nombre, @apellido, @dni, @lugar, @telefono, @email, @ngarante, @dnigarante, @tgarante)";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", e.Nombre);
					command.Parameters.AddWithValue("@apellido", e.Apellido);
					command.Parameters.AddWithValue("@dni", e.Dni);
					command.Parameters.AddWithValue("@lugar", e.LugarTrabajo);
					command.Parameters.AddWithValue("@telefono", e.Telefono);
					command.Parameters.AddWithValue("@email", e.Email);
					command.Parameters.AddWithValue("@ngarante", e.NombreGarante);
					command.Parameters.AddWithValue("@dnigarante", e.DniGarante);
					command.Parameters.AddWithValue("@tgarante", e.TelefonoGarante);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					e.Id = res;
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
				string sql = $"DELETE FROM Inquilino WHERE Id = {id}";
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
		public int Modificacion(Inquilino e)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Inquilino SET Nombre=@nombre, Apellido=@apellido, Dni=@dni, LugarTrabajo=@lugar, Telefono=@telefono, Email=@email, NombreGarante=@ngarante, DniGarante=@dnigarante, TelefonoGarante=@tgarante " +
					$"WHERE Id = @id";
				
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", e.Nombre);
					command.Parameters.AddWithValue("@apellido", e.Apellido);
					command.Parameters.AddWithValue("@dni", e.Dni);
					command.Parameters.AddWithValue("@lugar", e.LugarTrabajo);
					command.Parameters.AddWithValue("@telefono", e.Telefono);
					command.Parameters.AddWithValue("@email", e.Email);
					command.Parameters.AddWithValue("@ngarante", e.NombreGarante);
					command.Parameters.AddWithValue("@dnigarante", e.DniGarante);
					command.Parameters.AddWithValue("@tgarante", e.TelefonoGarante);
					command.Parameters.AddWithValue("@id", e.Id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Inquilino> ObtenerTodos()
		{
			IList<Inquilino> res = new List<Inquilino>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT Id, Nombre, Apellido, Dni, LugarTrabajo, Telefono, Email, NombreGarante, DniGarante, TelefonoGarante" +
					$" FROM Inquilino";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inquilino e = new Inquilino
						{
							Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Dni = reader.GetString(3),
							LugarTrabajo = reader.GetString(4),
							Telefono = reader.GetString(5),
							Email = reader.GetString(6),
							NombreGarante = reader.GetString(7),
							DniGarante = reader.GetString(8),
							TelefonoGarante = reader.GetString(9),
						};
						res.Add(e);
					}
					connection.Close();
				}
			}
			return res;
		}

		

		public Inquilino ObtenerPorId(int id)
		{
			Inquilino e = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT Id, Nombre, Apellido, Dni, LugarTrabajo, Telefono, Email, NombreGarante, DniGarante, TelefonoGarante FROM Inquilino" +
					$" WHERE Id=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						e = new Inquilino
						{
							Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Dni = reader.GetString(3),
							LugarTrabajo = reader.GetString(4),
							Telefono = reader.GetString(5),
							Email = reader.GetString(6),
							NombreGarante = reader.GetString(7),
							DniGarante = reader.GetString(8),
							TelefonoGarante = reader.GetString(9)
						};
					}
					connection.Close();
				}
			}
			return e;
		}
	}
}
