using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConcreteWeb.Api.Models
{
	public class Usuario
	{
		public Usuario()
		{
			this.Telefones = new List<Telefone>();
		}

		public int Id { get; set; }

		[Required]
		[StringLength(250)]
		public string Nome { get; set; }

		[Required]
		[Index(IsUnique = true)]
		[StringLength(100)]
		[RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "E-mail em formato inválido.")]
		public string Email { get; set; }

		[Required]
		[StringLength(20, MinimumLength = 4, ErrorMessage = "A senha deve conter entre 4 e 20 caracteres.")]
		public string Senha { get; set; }

		public virtual ICollection<Telefone> Telefones { get; set; }

		[DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime Data_Criacao { get; set; }

		public DateTime? Data_Atualizacao { get; set; }

		public DateTime Ultimo_Login { get; set; }
	}
}