using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ConcreteWeb.Api
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Serviços e configuração da API da Web

			// Remove o formato XML
			var formatos = GlobalConfiguration.Configuration.Formatters;
			formatos.Remove(formatos.XmlFormatter);

			// Rotas da API da Web
			config.MapHttpAttributeRoutes();

			// Modificardor de identação
			var setJson = formatos.JsonFormatter.SerializerSettings;
			setJson.Formatting = Formatting.Indented;
			setJson.ContractResolver = new CamelCasePropertyNamesContractResolver();

			formatos.JsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
		}
	}
}
