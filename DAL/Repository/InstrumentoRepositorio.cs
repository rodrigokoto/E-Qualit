using Dominio.Entidade;
using Dominio.Interface.Repositorio;
using System;
using System.Linq;

namespace DAL.Repository
{
    public class InstrumentoRepositorio : BaseRepositorio<Instrumento>, IInstrumentoRepositorio
    {
        public void RemoverComDelecaoDosRelacionamentos(int id)
        {
            try
            {
                var instrumento = Db.Instrumento
                                    .Include("Calibracao")
                                    .Where(x => x.IdInstrumento == id).FirstOrDefault();

                Db.Instrumento.Remove(instrumento);
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
