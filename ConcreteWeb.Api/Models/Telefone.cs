using System.ComponentModel.DataAnnotations;

namespace ConcreteWeb.Api.Models
{
	public class Telefone
	{
		public int Id { get; set; }

		[Required]
		[Range(11, 99)]
		public short Ddd { get; set; }

		[Required]
		[Range(10000000, 999999999)]
		public int Numero { get; set; }

		public int UsuarioId { get; set; }
		public virtual Usuario Usuario { get; set; }
	}
}