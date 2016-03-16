using PetaPoco;
using System.Collections.Generic;

namespace Smart.Data
{
    public class PetaPocoRepository<T> : Core.Data.IRepository<T>
    {
        private Database _database;
        public PetaPocoRepository(Database database)
        {
            _database = database;
        }

        public int Delete(T entity)
        {
            return _database.Delete<T>(entity);
        }

        public int Execute(string sql, params object[] args)
        {
            return _database.Execute(sql, args);
        }

        public bool Exists(string predicate, params object[] args)
        {
            return _database.Exists<T>(predicate, args);
        }

        public T Get(string sql, params object[] args)
        {
            return _database.FirstOrDefault<T>(sql, args);
        }

        public T GetById(object id)
        {
            return _database.FirstOrDefault<T>("select * from " + typeof(T).Name, id);
        }

        public int Insert(T entity)
        {
            return _database.Insert(entity) == null ? -1 : 1;
        }

        public IEnumerable<T> Query(string sql, params object[] args)
        {
            return _database.Query<T>(sql, args);
        }

        public int Update(T entity)
        {
            return _database.Update(entity);
        }
    }
}
