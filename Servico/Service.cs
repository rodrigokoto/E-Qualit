using DAL.Repository;
using DAL.Services;
using System;
using System.Linq;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Servico
{
    public partial class Service : IDisposable
    {
        public Exception Error;
        public bool Sucesso { get; set; }
        public string Metodo { get; set; }
        public string Mensagem { get; set; }
        public string ErrorCode { get; set; }
        protected Services dal = new Services();
        protected UnitOfWork unitOfWork = new UnitOfWork();

        public Service()
        {
            Sucesso = false;
        }

        internal string GetMethodName(MethodBase method)
        {
            string methodName = method.Name;
            string className = method.ReflectedType.Name;

            string fullMethodName = className + "." + methodName;
            return fullMethodName;
        }

        public long GravaLogErro(Exception erro, string origem, string arquivo)
        {
            long idErro = DateTime.Now.Ticks;

            try
            {
                StringBuilder stackTrace = new StringBuilder();
                StringBuilder msgErro = new StringBuilder();
                msgErro.Append("-------------------------------------------------------\r\n");
                msgErro.Append(origem + "\r\n");
                msgErro.Append("-------------------------------------------------------\r\n");
                msgErro.Append("Identificador do erro: " + idErro.ToString() + "\r\n");
                msgErro.Append("Data: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "\r\n");
                msgErro.Append("Descrição do erro:\r\n");
                Exception erroAux = erro;
                while (erroAux != null && erroAux.Message != null && erroAux.Message.Trim() != "")
                {
                    msgErro.Append(erroAux.Message + "\r\n\r\n");
                    stackTrace.Append(erroAux.StackTrace + "\r\n\r\n");

                    erroAux = erroAux.InnerException;
                    if (erroAux != null)
                    {
                        msgErro.Append(erroAux.Message + "\r\n\r\n");
                        stackTrace.Append(erroAux.StackTrace + "\r\n\r\n");
                    }
                }
                msgErro.Append("\r\n\r\n");
                msgErro.Append("Stack Trace:\r\n");
                msgErro.Append(stackTrace.ToString());
                msgErro.Append("\r\n-------------------------------------------------------");
                msgErro.Append("\r\n\r\n");

                //grava no arquivo				
                StreamWriter mWriter = new StreamWriter(arquivo, true);

                //Grava a msg de erro
                mWriter.WriteLine(msgErro.ToString());

                //Fecha o arquivo
                mWriter.Close();
            }
            catch
            {
                //retorna id indicando que erro nao foi registrado
                idErro = -1;
            }
            return idErro;
        }

        internal void DBErros(string metodo, DbEntityValidationException e)
        {
            string _erro = string.Empty;
            foreach (var eve in e.EntityValidationErrors)
            {
                _erro = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                foreach (var ve in eve.ValidationErrors)
                {
                    _erro += string.Format("\r\n- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                }
            }
            Sucesso = false;
            Error = new Exception(_erro);
            Metodo = metodo;
            GravaLogErro(new Exception(_erro), Metodo, string.Format(ConfigurationManager.AppSettings["LogErro"], DateTime.Now));
        }

        internal List<Entidade.ListaValor> GetLista(string cdTabela)
        {
            return (from x in dal.ListaValorGetAll().Where(w => w.CdTabela == cdTabela)
                    orderby x.DsDescricao
                    select new Entidade.ListaValor()
                    {
                        CdTabela = x.CdTabela,
                        CdCodigo = x.CdCodigo,
                        DsDescricao = x.DsDescricao,
                        IdListaValor = x.IdListaValor
                    }).ToList();
        }

        internal void Erros(string metodo, Exception e)
        {
            string _erro = string.Empty;
            Sucesso = false;
            Error = e;
            Metodo = metodo;
            GravaLogErro(new Exception(_erro), Metodo, string.Format(ConfigurationManager.AppSettings["LogErro"], DateTime.Now));
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    unitOfWork.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
