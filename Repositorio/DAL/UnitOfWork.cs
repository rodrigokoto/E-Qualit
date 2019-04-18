using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.DAL
{
    public class UnitOfWork : IDisposable
    {
        protected Context context = new Context();

        private GenericRepository<CtrlFuncionalidade> ctrlFuncionalidadeRepository;
        private GenericRepository<CtrlFuncionalidadePerfil> ctrlFuncionalidadePerfilRepository;
        private GenericRepository<CtrlFuncionalidadeUsuario> ctrlFuncionalidadeUsuarioRepository;
        private GenericRepository<CtrlPerfil> ctrlPerfilRepository;
        private GenericRepository<CtrlUsuario> ctrlUsuarioRepository;
        private GenericRepository<Endereco> enderecoRepository;
        private GenericRepository<ListaValor> listaValorRepository;
        private GenericRepository<CtrlPerfilUsuario> ctrlPerfilUsuarioRepository;
        private GenericRepository<Telefone> telefoneRepository;

        public GenericRepository<CtrlFuncionalidade> CtrlFuncionalidadeRepository
        {
            get
            {
                if (this.ctrlFuncionalidadeRepository == null)
                {
                    this.ctrlFuncionalidadeRepository = new GenericRepository<CtrlFuncionalidade>(context);
                }
                return ctrlFuncionalidadeRepository;
            }
        }
        public GenericRepository<CtrlFuncionalidadePerfil> CtrlFuncionalidadePerfilRepository
        {
            get
            {
                if (this.ctrlFuncionalidadePerfilRepository == null)
                {
                    this.ctrlFuncionalidadePerfilRepository = new GenericRepository<CtrlFuncionalidadePerfil>(context);
                }
                return ctrlFuncionalidadePerfilRepository;
            }
        }
        public GenericRepository<CtrlFuncionalidadeUsuario> CtrlFuncionalidadeUsuarioRepository
        {
            get
            {
                if (this.ctrlFuncionalidadeUsuarioRepository == null)
                {
                    this.ctrlFuncionalidadeUsuarioRepository = new GenericRepository<CtrlFuncionalidadeUsuario>(context);
                }
                return ctrlFuncionalidadeUsuarioRepository;
            }
        }
        public GenericRepository<CtrlPerfil> CtrlPerfilRepository
        {
            get
            {
                if (this.ctrlPerfilRepository == null)
                {
                    this.ctrlPerfilRepository = new GenericRepository<CtrlPerfil>(context);
                }
                return ctrlPerfilRepository;
            }
        }
        public GenericRepository<CtrlUsuario> CtrlUsuarioRepository
        {
            get
            {
                if (this.ctrlUsuarioRepository == null)
                {
                    this.ctrlUsuarioRepository = new GenericRepository<CtrlUsuario>(context);
                }
                return ctrlUsuarioRepository;
            }
        }
        public GenericRepository<Endereco> EnderecoRepository
        {
            get
            {
                if (this.enderecoRepository == null)
                {
                    this.enderecoRepository = new GenericRepository<Endereco>(context);
                }
                return enderecoRepository;
            }
        }
        public GenericRepository<ListaValor> ListaValorRepository
        {
            get
            {
                if (this.listaValorRepository == null)
                {
                    this.listaValorRepository = new GenericRepository<ListaValor>(context);
                }
                return listaValorRepository;
            }
        }
        public GenericRepository<CtrlPerfilUsuario> CtrlPerfilUsuarioRepository
        {
            get
            {
                if (this.ctrlPerfilUsuarioRepository == null)
                {
                    this.ctrlPerfilUsuarioRepository = new GenericRepository<CtrlPerfilUsuario>(context);
                }
                return ctrlPerfilUsuarioRepository;
            }
        }
        public GenericRepository<Telefone> TelefoneRepository
        {
            get
            {
                if (this.telefoneRepository == null)
                {
                    this.telefoneRepository = new GenericRepository<Telefone>(context);
                }
                return telefoneRepository;
            }
        }

        public Context Context()
        {
            return context;
        }

        private bool disposed = false;

        public void Save()
        {
            context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
