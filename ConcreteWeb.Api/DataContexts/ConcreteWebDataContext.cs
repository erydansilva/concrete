using ConcreteWeb.Api.Models;
using System.Data.Entity;

namespace ConcreteWeb.Api.DataContexts
{
	public class ConcreteWebDataContext : DbContext
	{
		public ConcreteWebDataContext() : base("ConexaoConcrete")
		{
			Database.SetInitializer<ConcreteWebDataContext>(new ConcreteWebDataContextInitializer());
		}

		public DbSet<Usuario> Usuarios { get; set; }
		public DbSet<Telefone> Telefones { get; set; }
	}

	public class ConcreteWebDataContextInitializer : CreateDatabaseIfNotExists<ConcreteWebDataContext>
	{
	}
}