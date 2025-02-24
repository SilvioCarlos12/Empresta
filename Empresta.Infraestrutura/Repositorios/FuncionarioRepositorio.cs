using Empresta.Dominio;
using Empresta.Infraestrutura.DbContext;
using Empresta.Infraestrutura.Repositorios.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace Empresta.Infraestrutura.Repositorios;

public sealed class FuncionarioRepositorio(IDbContext dbContext) : IFuncionarioRepositorio
{
    public async Task Add(Funcionario entity, CancellationToken cancellationToken)
    {
         await dbContext.InsertDocument(entity, cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        var funcionario = await GetById(id, cancellationToken);
        await dbContext.DeleteDocument(funcionario, cancellationToken);

    }

    public async Task<List<Funcionario>> GetAll(CancellationToken cancellationToken)
    {
        return await dbContext.GetCollection<Funcionario>().AsQueryable().ToListAsync(cancellationToken);
    }

    public async Task<List<Funcionario>> GetByFilter(Expression<Func<Funcionario, bool>> filter, CancellationToken cancellationToken)
    {
        var filtroBuild = Builders<Funcionario>.Filter;

        return await dbContext.GetCollection<Funcionario>().Find(filtroBuild.Where(filter)).ToListAsync(cancellationToken);
    }

    public async Task<Funcionario?> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.GetCollection<Funcionario>()
            .Find(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task Update(Funcionario entity,CancellationToken cancellationToken)
    {
        var funcionarioFiltrado = await GetById(entity.Id, cancellationToken);

        entity.Id = funcionarioFiltrado!.Id;

        await dbContext.UpdateDocument(entity, cancellationToken);
    }
}