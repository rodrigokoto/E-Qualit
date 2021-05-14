using Dominio.Enumerado;
using System;

namespace Dominio.Entidade
{
    public class Log
    {
        public int IdLog { get; set; }
        public int IdAcao { get; set; }
        public string IP { get; set; }
        public string Browser { get; set; }
        public string Mensagem { get; set; }
        public int? Modulo { get; set; }
        public DateTime DataCadastro { get; set; }
        public int? IdUsuario { get; set; }

        public int? Id { get; set; }
        public Log(int? idUsuario, int idAcao, string ip, string browser, Exception ex)
        {
            IdUsuario = idUsuario;
            IdAcao = idAcao;
            IP = ip;
            Browser = browser;
            DataCadastro = DateTime.Now;
            Mensagem = string.Format("{0},{1},{2}",
                                 ex.Message,
                                 ex.StackTrace,
                                 ex.InnerException != null ? ex.InnerException.Message : "");
        }

        public Log(int idAcao, Exception ex)
        {
            IdAcao = idAcao;
            DataCadastro = DateTime.Now;
            Mensagem = string.Format("{0},{1},{2}",
                                 ex.Message,
                                 ex.StackTrace,
                                 ex.InnerException != null ? ex.InnerException.Message : "");
        }

        public Log(int? idUsuario, int idAcao, string ip, string browser, int modulo , int? id )
        {
            IdUsuario = idUsuario;
            IdAcao = idAcao;
            IP = ip;
            Browser = browser;
            DataCadastro = DateTime.Now;
            Mensagem = string.Format("{0} , {1}", Util.EnumExtensions.ToName((Acao)IdAcao), Util.EnumExtensions.ToName((Funcionalidades)modulo));
            Modulo = modulo;
            Id = id;
        }
    }
}
