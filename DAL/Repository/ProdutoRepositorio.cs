using DAL.Context;
using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System;
using System.Data.Entity;
using System.Linq;

namespace DAL.Repository
{
    public class ProdutoRepositorio : BaseRepositorio<Produto>, IProdutoRepositorio
    {
        public bool Excluir(int idProduto)
        {
            using (var context = new BaseContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {

                    try
                    {

                        var produtoFornecedor = context.ProdutoFornecedor.Where(x => x.IdProduto == idProduto).ToList();

                        foreach(var item in produtoFornecedor)
                        {
                            context.Entry(item).State = EntityState.Deleted;
                        }

                        var avaliacaoCriticidade = context.AvaliacaoCriticidade.Where(x => x.IdProduto == idProduto).ToList();

                        foreach (var item in avaliacaoCriticidade)
                        {
                            context.Entry(item).State = EntityState.Deleted;
                        }

                        var criterioQualificacao = context.CriterioQualificacao.Where(x => x.IdProduto == idProduto).ToList();

                        foreach (var item in criterioQualificacao)
                        {
                            context.Entry(item).State = EntityState.Deleted;
                        }

                        var criterioAvaliacao = context.CriterioAvaliacao.Where(x => x.IdProduto == idProduto).ToList();

                        foreach (var item in criterioAvaliacao)
                        {

                            var historicoCriterioAvaliacao = context.HistoricoCriterioAvaliacao.Where(x => x.IdCriterioAvaliacao == item.IdCriterioAvaliacao).ToList();

                            foreach (var itemHistorico in historicoCriterioAvaliacao)
                            {
                                context.Entry(itemHistorico).State = EntityState.Deleted;
                            }

                            var avaliaCriterioAvaliacao = context.AvaliaCriterioAvaliacao.Where(x => x.IdCriterioAvaliacao == item.IdCriterioAvaliacao).ToList();

                            foreach (var itemAvaliacao in avaliaCriterioAvaliacao)
                            {
                                context.Entry(itemAvaliacao).State = EntityState.Deleted;
                            }

                            context.Entry(item).State = EntityState.Deleted;
                        }

                        var avaliaCriterioQualificacao = context.AvaliaCriterioQualificacao.Where(x => x.CriterioQualificacao.IdProduto == idProduto).ToList();

                        foreach (var item in avaliaCriterioQualificacao)
                        {
                            context.Entry(item).State = EntityState.Deleted;
                        }

                        var produto = context.Produto.Where(x => x.IdProduto == idProduto).FirstOrDefault();
                        
                        context.Entry(produto).State = EntityState.Deleted;

                        context.SaveChanges();

                        dbContextTransaction.Commit();
                        return true;
                        

                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        throw ex;
                        return true;
                    }
                }
            }
        }
    }
}
