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

    public async Task Add(Funcionario entity, CancellationToken cancellationToken)
    {
         await _dbContext.InsertDocument(entity, cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        var funcionario = await GetById(id, cancellationToken);
        await _dbContext.DeleteDocument(funcionario, cancellationToken);

    }

    public async Task<IEnumerable<Funcionario>> GetAll(CancellationToken cancellationToken)
    {
        return await _dbContext.GetCollection<Funcionario>().AsQueryable().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Funcionario>> GetByFilter(Expression<Func<Funcionario, bool>> filter, CancellationToken cancellationToken)
    {
        var filtroBuild = Builders<Funcionario>.Filter;

        return await _dbContext.GetCollection<Funcionario>().Find(filtroBuild.Where(filter)).ToListAsync(cancellationToken);
    }

    public async Task<Funcionario?> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.GetCollection<Funcionario>()
            .Find(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task Update(Funcionario entity,CancellationToken cancellationToken)
    {
        var funcionarioFiltrado = await GetById(entity.Id, cancellationToken);

        entity.Id = funcionarioFiltrado!.Id;

        await _dbContext.UpdateDocument(entity, cancellationToken);
    }
}