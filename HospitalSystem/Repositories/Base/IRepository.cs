using System.Linq.Expressions;

namespace HospitalSystem.Repositories.Base
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T? GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        bool Any(Expression<Func<T, bool>> predicate);
        int Count(Expression<Func<T, bool>>? predicate = null);
    }
}
