using DAL.Context;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System;
using System.Data.Entity;
using System.Linq;

namespace DAL.Repository
{
    public class UsuarioRepositorio : BaseRepositorio<Usuario>, IUsuarioRepositorio
    {

        public bool Excluir(int id, int idUsuarioMigracao)
        {
            using (var context = new BaseContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {

                    try
                    {


                        var temDependencia = AtualizaDependenciasUsuario(id, idUsuarioMigracao, context);

                        var usuario = context.Usuario.Where(x => x.IdUsuario == id).FirstOrDefault();

                        usuario.FlAtivo = false;

                        context.Entry(usuario).State = EntityState.Modified;


                        #region Exclusao de registros

                        //var usuariosClientesSites = context.UsuarioClienteSite.Where(x => x.IdUsuario == id).ToList();

                        //foreach (var usuarioClienteSite in usuariosClientesSites)
                        //{

                        //    var usuarioCargos = context.UsuarioCargo.Where(x => x.IdUsuario == usuarioClienteSite.IdUsuario).ToList();

                        //    foreach (var usuarioCargo in usuarioCargos)
                        //    {
                        //        context.Entry(usuarioCargo).State = EntityState.Deleted;
                        //    }

                        //    var usuarioAnexos = context.UsuarioAnexo.Where(x => x.IdUsuario == usuarioClienteSite.IdUsuario).ToList();

                        //    foreach (var usuarioAnexo in usuarioAnexos)
                        //    {
                        //        context.Entry(usuarioAnexo).State = EntityState.Deleted;

                        //        var anexo = context.Anexo.Where(x => x.IdAnexo == usuarioAnexo.IdAnexo).FirstOrDefault();
                        //        context.Entry(anexo).State = EntityState.Deleted;
                        //    }

                        //    var usuarioSenhas = context.UsuarioSenha.Where(x => x.IdUsuario == usuarioClienteSite.IdUsuario).ToList();

                        //    foreach (var usuarioSenha in usuarioSenhas)
                        //    {
                        //        context.Entry(usuarioSenha).State = EntityState.Deleted;
                        //    }

                        //    context.Entry(usuarioClienteSite).State = EntityState.Deleted;

                        //    var usuario = context.Usuario.Where(x => x.IdUsuario == usuarioClienteSite.IdUsuario).FirstOrDefault();

                        //    context.Entry(usuario).State = EntityState.Deleted;

                        //}

                        #endregion

                        context.SaveChanges();

                        dbContextTransaction.Commit();
                        return true;

                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                        return false;
                    }
                }
            }
        }

        private bool AtualizaDependenciasUsuario(int id, int idUsuarioMigracao, BaseContext context)
        {
            bool retorno = false;
            Byte statusRegistro4 = Convert.ToByte(4);

            var registros = context.RegistroConformidade.Where(x => x.StatusEtapa.Value < statusRegistro4 && x.IdResponsavelEtapa.Value == id).ToList();

            registros.ForEach(x =>
            {
                x.IdResponsavelEtapa = idUsuarioMigracao;

                if (x.StatusEtapa == 1)
                {
                    x.IdResponsavelInicarAcaoImediata = idUsuarioMigracao;
                }
                else if (x.StatusEtapa == 2)
                {
                    x.IdResponsavelImplementar = idUsuarioMigracao;
                }
                else if (x.StatusEtapa == 2)
                {
                    x.IdResponsavelAnalisar = idUsuarioMigracao;
                }

                if (x.TipoRegistro == "nc")
                {
                    var notificacoesNaoConformidade = context.Notificacao.Where(y => y.IdRelacionado == x.IdNaoConformidade && y.IdFuncionalidade == 3).ToList();

                    notificacoesNaoConformidade.ForEach(y =>
                    {
                        y.IdUsuario = idUsuarioMigracao;
                        context.Entry(y).State = EntityState.Modified;
                    });

                }
                else if (x.TipoRegistro == "gr")
                {
                    var notificacoesNaoConformidade = context.Notificacao.Where(y => y.IdRelacionado == x.IdNaoConformidade && y.IdFuncionalidade == 11).ToList();

                    notificacoesNaoConformidade.ForEach(y =>
                    {
                        y.IdUsuario = idUsuarioMigracao;
                        context.Entry(y).State = EntityState.Modified;
                    });
                }

                else if (x.TipoRegistro == "ac")
                {
                    var notificacoesNaoConformidade = context.Notificacao.Where(y => y.IdRelacionado == x.IdNaoConformidade && y.IdFuncionalidade == 4).ToList();

                    notificacoesNaoConformidade.ForEach(y =>
                    {
                        y.IdUsuario = idUsuarioMigracao;
                        context.Entry(y).State = EntityState.Modified;
                    });
                }

                context.Entry(x).State = EntityState.Modified;

                retorno = true;
            });


            Byte statusRegistro3 = Convert.ToByte(3);
            var documentosVerificadores = context.DocUsuarioVerificaAprova.Where(x => x.DocDocumento.FlStatus < statusRegistro3 && x.IdUsuario == id).ToList();

            documentosVerificadores.ForEach(x =>
            {
                x.IdUsuario = idUsuarioMigracao;

                var notificacoesNaoConformidade = context.Notificacao.Where(y => y.IdRelacionado == x.IdDocumento && y.IdFuncionalidade == 2).ToList();

                notificacoesNaoConformidade.ForEach(y =>
                {
                    y.IdUsuario = idUsuarioMigracao;
                    context.Entry(y).State = EntityState.Modified;
                });

                context.Entry(x).State = EntityState.Modified;

                retorno = true;
            });

            var documentos = context.DocDocumento.Where(x => x.FlStatus == 0 && x.IdElaborador == id).ToList();

            documentos.ForEach(x =>
            {
                x.IdElaborador = idUsuarioMigracao;

                var notificacoesNaoConformidade = context.Notificacao.Where(y => y.IdRelacionado == x.IdDocumento && y.IdFuncionalidade == 2).ToList();

                notificacoesNaoConformidade.ForEach(y =>
                {
                    y.IdUsuario = idUsuarioMigracao;
                    context.Entry(y).State = EntityState.Modified;
                });

                context.Entry(x).State = EntityState.Modified;

                retorno = true;

            });

            var fornecedores = context.Fornecedor.Where(x => x.IdUsuarioAvaliacao.Value == id).ToList();

            fornecedores.ForEach(x =>
            {
                x.IdUsuarioAvaliacao = idUsuarioMigracao;

                var notificacoesNaoConformidade = context.Notificacao.Where(y => y.IdRelacionado == x.IdFornecedor && y.IdFuncionalidade == 10).ToList();

                notificacoesNaoConformidade.ForEach(y =>
                {
                    y.IdUsuario = idUsuarioMigracao;
                    context.Entry(y).State = EntityState.Modified;
                });

                context.Entry(x).State = EntityState.Modified;

                retorno = true;

            });

            var avaliaCriteriosQualificacao = context.AvaliaCriterioQualificacao.Where(x => x.IdResponsavelPorControlarVencimento == id).ToList();

            avaliaCriteriosQualificacao.ForEach(x =>
            {
                x.IdResponsavelPorControlarVencimento = idUsuarioMigracao;

                var notificacoesNaoConformidade = context.Notificacao.Where(y => y.IdRelacionado == x.IdFornecedor && y.IdFuncionalidade == 10).ToList();

                notificacoesNaoConformidade.ForEach(y =>
                {
                    y.IdUsuario = idUsuarioMigracao;
                    context.Entry(y).State = EntityState.Modified;
                });

                context.Entry(x).State = EntityState.Modified;

                retorno = true;

            });

            Byte status0 = Convert.ToByte(0);
            Byte status3 = Convert.ToByte(3);

            var instrumentos = context.Calibracao.Where(x => x.Instrumento.IdResponsavel.Value == id && (x.Aprovado == status0 || x.Aprovado == status3)).Select(x => x.Instrumento).ToList();

            instrumentos.ForEach(x =>
            {
                x.IdResponsavel = idUsuarioMigracao;

                var notificacoesNaoConformidade = context.Notificacao.Where(y => y.IdRelacionado == x.IdInstrumento && y.IdFuncionalidade == 9).ToList();

                notificacoesNaoConformidade.ForEach(y =>
                {
                    y.IdUsuario = idUsuarioMigracao;
                    context.Entry(y).State = EntityState.Modified;
                });

                context.Entry(x).State = EntityState.Modified;

                retorno = true;

            });




            return retorno;
        }

        public bool AtualizaSenha(Usuario oUsuario)
        {
            using (var context = new BaseContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {

                    try
                    {
                        Usuario usuario = context.Usuario.Where(x => x.IdUsuario == oUsuario.IdUsuario).FirstOrDefault();

                        UsuarioSenha usuarioSenha = new UsuarioSenha();
                        usuarioSenha.CdSenha = usuario.CdSenha;
                        usuarioSenha.DtInclusaoSenha = DateTime.Now;
                        usuarioSenha.IdUsuario = usuario.IdUsuario;
                        usuarioSenha.Usuario = usuario;

                        usuario.UsuarioSenha.Add(usuarioSenha);
                        usuario.DtAlteracaoSenha = DateTime.Now;
                        usuario.CdSenha = oUsuario.CdSenha;
                        context.Entry(usuario).State = EntityState.Modified;

                        context.SaveChanges();

                        dbContextTransaction.Commit();
                        return true;

                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                        return false;
                    }
                }
            }
        }

    }
}
