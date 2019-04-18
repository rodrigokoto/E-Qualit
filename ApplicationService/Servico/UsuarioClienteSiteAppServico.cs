﻿using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using ApplicationService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationService.Servico
{
    public class UsuarioClienteSiteAppServico : BaseServico<UsuarioClienteSite>, IUsuarioClienteSiteAppServico
    {
        private readonly IUsuarioClienteSiteRepositorio _usuarioClienteSiteRepositorio;

        public UsuarioClienteSiteAppServico(IUsuarioClienteSiteRepositorio usuarioClienteSiteRepositorio) : base(usuarioClienteSiteRepositorio)
        {
            _usuarioClienteSiteRepositorio = usuarioClienteSiteRepositorio;
        }
        
        public List<Usuario> ListarPorEmpresa(int idEmpresa)
        {
            var _usuarios = new List<Usuario>();
            try
            {
                var _usuariosCTX = _usuarioClienteSiteRepositorio.Get(x => x.IdSite == idEmpresa);

                foreach (var _usuarioCTX in _usuariosCTX.Where(x => x.Usuario.FlAtivo == true))
                {
                    _usuarios.Add(new Usuario
                    {
                        IdUsuario = _usuarioCTX.Usuario.IdUsuario,
                        NmCompleto = _usuarioCTX.Usuario.NmCompleto
                    });
                };
            }
            catch (Exception ex)
            {
            }

            return  UsuarioAppServico.RetiraDuplicado(_usuarios);
        }

        public List<UsuarioClienteSite> ListarPorUsuario(int idUsuario)
        {
            var _usuarios = new List<UsuarioClienteSite>();
            try
            {
                _usuarios = _usuarioClienteSiteRepositorio.ListarPorUsuario(idUsuario);

            }
            catch (Exception ex)
            {
            }

            return _usuarios;
        }

        public List<UsuarioClienteSite> ObterSitesPorUsuario(int idUsuario)
        {
            return _usuarioClienteSiteRepositorio.Get(x => x.IdUsuario == idUsuario).ToList();
        }
    }
}
