using Empresta.Dominio;
using Empresta.Infraestrutura.DbContext;
using Empresta.Infraestrutura.Repositorios.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace Empresta.Infraestrutura.Repositorios;

public sealed class ClienteRepositorio(IDbContext dbContext) : IClienteRepositorio
{
    public async Task Add(Cliente entity, CancellationToken cancellationToken)
    {
        await dbContext.InsertDocument(entity, cancellationToken);
    }

    public async Task<Cliente?> GetById(Guid id, CancellationToken cancellationToken)
    {

        return await dbContext.GetCollection<Cliente>()
            .Find(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Cliente>> GetAll(CancellationToken cancellationToken)
    {
        return await dbContext.GetCollection<Cliente>()
            .AsQueryable().ToListAsync(cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        var cliente = await GetById(id, cancellationToken);
        await dbContext.DeleteDocument(cliente, cancellationToken);
    }

    public async Task Update(Cliente entity, CancellationToken cancellationToken)
    {
        var clienteFiltrado = await GetById(entity.Id, cancellationToken);

        entity.Id = clienteFiltrado!.Id;

        await dbContext.UpdateDocument(entity, cancellationToken);
    }

    public async Task<List<Cliente>> GetByFilter(Expression<Func<Cliente, bool>> filter, CancellationToken cancellationToken)
    {
        var filtroBuild = Builders<Cliente>.Filter;

        return await dbContext.GetCollection<Cliente>().Find(filtroBuild.Where(filter)).ToListAsync(cancellationToken);
    }
}