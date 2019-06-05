using DAL.Context;
using Dominio.Enumerado;
using Dominio.Interface.Repositorio;
using Dominio.Servico;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.Repository
{
    public class BaseRepositorio<TEntity> : IDisposable, IBaseRepositorio<TEntity> where TEntity : class
    {
        protected BaseContext Db;
        public BaseRepositorio()
        {
            Db = new BaseContext();
            if (Debugger.IsAttached)
            {
                Db.Database.Log = (s) => Debug.Write(s);
            }
        }


        public void Add(TEntity obj)
        {
            Db.Set<TEntity>().Add(obj);
            Db.SaveChanges();
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
                                               Func<IQueryable<TEntity>,
                                               IOrderedQueryable<TEntity>> orderBy = null,
                                               string includeProperties = "")
        {

            IQueryable<TEntity> query = Db.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Db.Set<TEntity>().ToList();
        }

        public IEnumerable<TEntity> GetAllAsNoTracking()
        {
            using (var dbContext = Db)
            {
                dbContext.Configuration.ProxyCreationEnabled = false;

                return dbContext.Set<TEntity>().AsNoTracking().ToList();
            }
        }

        public TEntity GetById(int id)
        {
            var teste = Db.Set<TEntity>().Find(id);


            return teste;
        }

        public void Update(TEntity obj)
        {
            try
            {
                Db.Entry(obj).State = EntityState.Modified;
                Db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                Db.Entry(obj).State = EntityState.Detached;
                throw;
            }
        }

        public void AlteraEstado(TEntity obj, EstadoObjetoEF estado)
        {
            //Db.Entry(obj).State =;

            using (var context = new BaseContext())
            {
                context.Set<TEntity>().Attach(obj);
                context.ChangeTracker.Entries<TEntity>().First(e => e.Entity == obj).State = (EntityState)estado;
                context.SaveChanges();
            }

        }

        public DataTable GetDataTable(string sql, CommandType commandType, Dictionary<string, Object> parameters)
        {
            // creates resulting dataset
            var result = new DataSet();

            // creates a data access context (DbContext descendant)
            using (var context = new BaseContext())
            {
                // creates a Command 
                var cmd = context.Database.Connection.CreateCommand();
                cmd.CommandType = commandType;
                cmd.CommandText = sql;

                // adds all parameters
                foreach (var pr in parameters)
                {
                    var p = cmd.CreateParameter();
                    p.ParameterName = pr.Key;
                    p.Value = pr.Value;
                    cmd.Parameters.Add(p);
                }

                try
                {
                    // executes
                    context.Database.Connection.Open();
                    var reader = cmd.ExecuteReader();

                    // loop through all resultsets (considering that it's possible to have more than one)
                    do
                    {
                        // loads the DataTable (schema will be fetch automatically)
                        var tb = new DataTable();
                        tb.Load(reader);
                        result.Tables.Add(tb);

                    } while (!reader.IsClosed);
                }
                finally
                {
                    // closes the connection
                    context.Database.Connection.Close();
                }
            }

            // returns the DataTable
            return result.Tables[0];
        }

        public void Dispose()
        {
            Db.Dispose();
            GC.SuppressFinalize(this);
        }

        public void Remove(TEntity obj)
        {
            Db.Entry(obj).State = EntityState.Deleted;
            Db.SaveChanges();
        }
    }
}
