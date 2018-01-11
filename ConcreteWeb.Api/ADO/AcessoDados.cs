using ConcreteWeb.Api.Models;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ConcreteWeb.Api.ADO
{
	public class AcessoDados
	{
		private bool InsereRegistroUsuario(string connectionString, Usuario usuario)
		{
			try
			{
				//Query de inserção
				string query = "INSERT INTO Usuarios (Nome, Email, Senha, Data_Criacao, Ultimo_Login) " +
									"VALUES (@Nome, @Email, @Senha, @Data_Criacao, @Ultimo_Login) ";

				//Cria conexão e comandos
				using (SqlConnection cn = new SqlConnection(connectionString))
				using (SqlCommand cmd = new SqlCommand(query, cn))
				{
					//Define parâmetros
					cmd.Parameters.Add("@Nome", SqlDbType.NVarChar, 250).Value = usuario.Nome;
					cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = usuario.Email;
					cmd.Parameters.Add("@Senha", SqlDbType.NVarChar, 20).Value = usuario.Senha;
					cmd.Parameters.Add("@Data_Criacao", SqlDbType.DateTime).Value = usuario.Data_Criacao;
					cmd.Parameters.Add("@Ultimo_Login", SqlDbType.DateTime).Value = usuario.Ultimo_Login;

					cn.Open();
					cmd.ExecuteNonQuery();
					cn.Close();
				}
				return true;
			}
			catch(Exception e)
			{
				return false;
			}
		}

		private bool InsereRegistroTelefone(string connectionString, Telefone telefone, int id)
		{
			try
			{
				//Query de inserção
				string query = "INSERT INTO Telefones (Ddd, Numero, UsuarioId) " +
									"VALUES (@Ddd, @Numero, @UsuarioId)";
				//Cria conexão e comandos
				using (SqlConnection cn = new SqlConnection(connectionString))
				using (SqlCommand cmd = new SqlCommand(query, cn))

				{
					//Define parâmetros
					cmd.Parameters.Add("@Ddd", SqlDbType.SmallInt).Value = telefone.Ddd;
					cmd.Parameters.Add("@Numero", SqlDbType.Int).Value = telefone.Numero;
					cmd.Parameters.Add("@UsuarioId", SqlDbType.Int).Value = id;

					cn.Open();
					cmd.ExecuteNonQuery();
					cn.Close();
				}
				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}

		public int buscaIdUsuario(string email)
		{
			try
			{
				string connectionString = ConfigurationManager.ConnectionStrings["ConexaoConcrete"].ConnectionString;
				int res = 0;

				//Query de busca
				string query = "SELECT Id FROM Usuarios WHERE Email = @Email";

				//Cria conexão e comandos
				using (SqlConnection cn = new SqlConnection(connectionString))
				using (SqlCommand cmd = new SqlCommand(query, cn))
				{
					//Define parâmetros
					cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = email;

					cn.Open();
					SqlDataReader dr = cmd.ExecuteReader();
					while (dr.Read())
					{
						res = (int)dr[0];
					}
					cn.Close();
				}
				return res;
			}
			catch (Exception e)
			{
				return 0;
			}
		}

		public bool InsereNovoUsuario(Usuario usuario)
		{
			try
			{
				string connectionString = ConfigurationManager.ConnectionStrings["ConexaoConcrete"].ConnectionString;

				if (InsereRegistroUsuario(connectionString, usuario))
				{
					int id = buscaIdUsuario(usuario.Email);
					foreach (Telefone telefone in usuario.Telefones)
					{
						if(!InsereRegistroTelefone(connectionString, telefone, id))
							return false;
					}
					return true;
				}
				else
					return false;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}