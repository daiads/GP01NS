﻿using GP01NS.Classes.Servicos;
using GP01NS.Classes.Util;
using GP01NS.Models;
using GP01NSLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GP01NS.Controllers
{
    public class BaseController : Controller
    {
        protected usuario BaseUsuario;

        protected override void OnActionExecuting(ActionExecutingContext filterContext) 
        {
            var controller = filterContext.RouteData.Values["controller"].ToString();
            var action = filterContext.RouteData.Values["action"].ToString();

            if (controller.Equals("entrar", StringComparison.CurrentCultureIgnoreCase) && action.Equals("index", StringComparison.CurrentCultureIgnoreCase))
            {
                return;
            }

            string id = string.Empty;

            try
            {
                id = Criptografia.Descriptografar(Session["IDUsuario"].ToString());
            }
            catch { }

            if (((string.IsNullOrEmpty(id)) && (!Session.IsNewSession)) || (Session.IsNewSession))
            {
                base.Session.RemoveAll();
                base.Session.Clear();
                base.Session.Abandon();
                base.Session["IDUsuario"] = string.Empty;

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "entrar" }, { "action", "index" } });
            }
            else
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var auths = db.autenticacao.Where(x => x.Sessao.Equals(this.Session.SessionID)).ToList();

                    var auth = auths.Last();

                    this.BaseUsuario = auth.usuario;
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
