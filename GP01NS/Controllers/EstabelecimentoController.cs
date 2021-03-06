﻿using GP01NS.Classes.ViewModels;
using GP01NS.Classes.ViewModels.Estabelecimento;
using GP01NS.Models;
using GP01NSLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GP01NS.Controllers
{
    public class EstabelecimentoController : BaseController
    {
        private EstabelecimentoVM Estabelecimento;

        public ActionResult Index()
        {
            this.Estabelecimento = new EstabelecimentoVM(this.BaseUsuario);

            return View();
        }

        public ActionResult Cadastro()
        {
            this.Estabelecimento = new EstabelecimentoVM(this.BaseUsuario);

            var cadastro = new CadastroVM(this.Estabelecimento);

            return View(cadastro);
        }

        [HttpPost]
        public ActionResult Cadastro(CadastroVM model) 
        {
            this.Estabelecimento = new EstabelecimentoVM(this.BaseUsuario);

            if (ModelState.IsValid)
            {
                if (model.ValidarEmail(this.Estabelecimento))
                {
                    if (model.ValidarNomeUsuario(this.Estabelecimento))
                    {
                        if (model.SaveChanges(this.Estabelecimento))
                            ViewBag.Sucesso = "Os dados foram salvos.";
                        else
                            ViewBag.Erro = "Não foi possível estabelecer conexão com o servidor, por favor, tente novamente mais tarde.";
                    }
                    else
                    {
                        ViewBag.Erro = "O nome de usuário informado já está sendo utilizado.";
                    }
                }
                else
                {
                    ViewBag.Erro = "O endereço de e-mail informado já está sendo utilizado.";
                }
            }
            else
            {
                ViewBag.Erro = "Por favor, confira os dados informados e tente novamente.";
            }

            return Redirect("/inicio/");
        }

        public ActionResult Endereco()
        {
            this.Estabelecimento = new EstabelecimentoVM(this.BaseUsuario);

            var endereco = this.Estabelecimento.Endereco;

            return View(endereco);
        }

        [HttpPost]
        public ActionResult Endereco(EnderecoVM model)
        {
            this.Estabelecimento = new EstabelecimentoVM(this.BaseUsuario);

            if (model.SaveChanges(this.Estabelecimento))
            {
                ViewBag.Sucesso = "Os dados de endereço foram salvos.";
            }
            else
            {
                ViewBag.Erro = "Por favor, confira os dados informados e tente novamente.";
            }

            return View(model);
        }
    }
}
