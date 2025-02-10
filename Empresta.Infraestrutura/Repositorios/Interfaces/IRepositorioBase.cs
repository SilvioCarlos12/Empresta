using System.Linq.Expressions;

namespace Empresta.Infraestrutura.Repositorios.Interfaces
{
    public interface IRepositorioBase<T> where T : class
    {
        Task Add(T entity, CancellationToken cancellationToken);
        Task<T?> GetById(Guid id, CancellationToken cancellationToken);
        Task<List<T>> GetAll(CancellationToken cancellationToken);
        Task Update(T entity,CancellationToken cancellationToken);
        Task Delete(Guid id, CancellationToken cancellationToken);
        Task<List<T>> GetByFilter(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);
    }
}
