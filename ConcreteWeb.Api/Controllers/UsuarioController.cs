using ConcreteWeb.Api.ADO;
using ConcreteWeb.Api.DataContexts;
using ConcreteWeb.Api.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ConcreteWeb.Api.Controllers
{
	[RoutePrefix("api/concrete")]
	public class UsuarioController : ApiController
   {
		private ConcreteWebDataContext db = new ConcreteWebDataContext();

		// GET: api/concrete/usuarios (Listar todos os usuários.)
		[Route("usuarios")]
		public HttpResponseMessage GetUsuarios()
		{
			try
			{
				var response = Request.CreateResponse(HttpStatusCode.OK, db.Usuarios);
				return response;
			}
			catch
			{
				return Request.CreateResponse(HttpStatusCode.InternalServerError, "Não foi possível listar os usuários.");
			}
		}

		// GET: api/concrete/usuarios/5 (Mostrar usuário pela Id.)
		[Route("usuarios/id={id}")]
		[ResponseType(typeof(Usuario))]
		public Task<HttpResponseMessage> GetUsuario(int id)
		{
			try
			{
				var task = new TaskCompletionSource<HttpResponseMessage>();
				var resposta = Request.CreateResponse(HttpStatusCode.OK);
				Usuario usuario = db.Usuarios.Find(id);

				if (usuario == null)
				{
					resposta = Request.CreateResponse(HttpStatusCode.NotFound, "Usuário não encontrado.");
				}
				else
				{
					resposta = Request.CreateResponse(HttpStatusCode.OK, usuario);
				}

				task.SetResult(resposta);
				return task.Task;
			}
			catch
			{
				var task = new TaskCompletionSource<HttpResponseMessage>();
				task.SetResult(Request.CreateResponse(HttpStatusCode.InternalServerError, "Não foi possível buscar o usuário."));
				return task.Task;
			}
			
		}

		// PUT: api/usuarios/5 (Atualizar dados de um usuário pela Id.)
		[Route("usuarios/{id}")]
		[ResponseType(typeof(void))]
		public Task<HttpResponseMessage> PutUsuario(int id, Usuario usuario)
		{
			var task = new TaskCompletionSource<HttpResponseMessage>();
			var resposta = Request.CreateResponse(HttpStatusCode.OK);

			usuario.Data_Atualizacao = DateTime.Now;
			foreach(Telefone tel in usuario.Telefones)
				tel.UsuarioId = id;

			if (!ModelState.IsValid)
			{
				resposta = Request.CreateResponse(HttpStatusCode.BadRequest, "Dados para alteração do usuário inválidos.");
				task.SetResult(resposta);
				return task.Task;
			}

			if (id != usuario.Id)
			{
				resposta = Request.CreateResponse(HttpStatusCode.BadRequest, "Dados inválidos.");
				task.SetResult(resposta);
				return task.Task;
			}

			try
			{
				db.Entry(usuario).State = EntityState.Modified;
				db.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!UsuarioExists(id))
					task.SetResult(Request.CreateResponse(HttpStatusCode.NotFound, "Usuário não existe."));
				else
					task.SetResult(Request.CreateResponse(HttpStatusCode.InternalServerError, "Alterações não foram salvas."));
				return task.Task;
			}
			resposta = Request.CreateResponse(HttpStatusCode.OK, usuario);
			task.SetResult(resposta);
			return task.Task;
		}

		// POST: concrete/signup (Cadastrar usuário novo.)
		[Route("signup")]
		[ResponseType(typeof(Usuario))]
		public Task<HttpResponseMessage> PostUsuario(Usuario usuario)
		{
			var task = new TaskCompletionSource<HttpResponseMessage>();
			var resposta = Request.CreateResponse(HttpStatusCode.OK);

			usuario.Data_Criacao = DateTime.Now;
			usuario.Ultimo_Login = DateTime.Now;

			if (!ModelState.IsValid)
			{
				resposta = Request.CreateResponse(HttpStatusCode.BadRequest, "Dados do usuário inválidos.");
				task.SetResult(resposta);
				return task.Task;
			}
			
			var ado = new AcessoDados();

			//Verifica existência do usuário.
			if (ado.buscaIdUsuario(usuario.Email) != 0)
			{
				resposta = Request.CreateResponse(HttpStatusCode.BadRequest, "E-mail já existente.");
				task.SetResult(resposta);
				return task.Task;
			}

			if (!ado.InsereNovoUsuario(usuario))
			{
				resposta = Request.CreateResponse(HttpStatusCode.InternalServerError, "Falha ao salvar usuário.");
				task.SetResult(resposta);
				return task.Task;
			}

			resposta = Request.CreateResponse(HttpStatusCode.OK, usuario);
			task.SetResult(resposta);
			return task.Task;
		}

		// DELETE: api/Usuario/5 (Apagar usuário pela Id.)
		[Route("usuarios/{id}")]
		[ResponseType(typeof(Usuario))]
		public async Task<IHttpActionResult> DeleteUsuario(int id)
		{
			Usuario usuario = await db.Usuarios.FindAsync(id);
			if (usuario == null)
			{
				return NotFound();
			}

			db.Usuarios.Remove(usuario);
			await db.SaveChangesAsync();

			return Ok(usuario);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}

		private bool UsuarioExists(int id)
		{
			return db.Usuarios.Count(e => e.Id == id) > 0;
		}
    }
}