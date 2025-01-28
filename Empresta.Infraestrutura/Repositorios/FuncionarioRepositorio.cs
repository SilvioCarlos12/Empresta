using Empresta.Dominio;
using Empresta.Infraestrutura.DbContext;
using Empresta.Infraestrutura.Repositorios.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace Empresta.Infraestrutura.Repositorios;

public class FuncionarioRepositorio:IFuncionarioRepositorio
{
    private readonly IDbContext _dbContext;

    public FuncionarioRepositorio(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task Add(Funcionario entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Funcionario>> GetAll(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Funcionario>> GetByFilter(Expression<Func<Funcionario, bool>> filter, CancellationToken cancellationToken)
    {
        var filtroBuild = Builders<Funcionario>.Filter;

        return await _dbContext.GetCollection<Funcionario>().Find(filtroBuild.Where(filter)).ToListAsync(cancellationToken);
    }

    public Task<Funcionario?> GetById(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task Update(Funcionario entity)
    {
        throw new NotImplementedException();
    }
}