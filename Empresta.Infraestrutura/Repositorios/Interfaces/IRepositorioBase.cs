using System.Linq.Expressions;

namespace Empresta.Infraestrutura.Repositorio
{
    public interface IRepositorioBase<T> where T : class
    {
        Task Add(T entity, CancellationToken cancellationToken);
        Task<T?> GetById(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken);
        Task Update(T entity);
        Task Delete(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<T>> GetByFilter(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);
    }
}
