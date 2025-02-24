using System.Linq.Expressions;
using Empresta.Dominio;
using Empresta.Dominio.Enums;
using Empresta.Dominio.Vo;
using Empresta.Infraestrutura.DbContext;
using Empresta.Infraestrutura.Repositorios.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Empresta.Infraestrutura.Repositorios;

public class CaixaRepositorio(IDbContext dbContext) : ICaixaRepositorio
{
    public async Task Add(Caixa entity, CancellationToken cancellationToken)
    {
        await dbContext.InsertDocument(entity, cancellationToken);
    }

    public async Task<Caixa?> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.GetCollection<Caixa>()
            .Find(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Caixa>> GetAll(CancellationToken cancellationToken)
    {
        return await dbContext.GetCollection<Caixa>()
            .AsQueryable().ToListAsync(cancellationToken);;
    }

    public async Task Update(Caixa entity, CancellationToken cancellationToken)
    {
        var caixaFiltrado = await GetById(entity.Id, cancellationToken);

        entity.Id = caixaFiltrado!.Id;

        await dbContext.UpdateDocument(entity, cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        var caixa = await GetById(id, cancellationToken);
        await dbContext.DeleteDocument(caixa, cancellationToken);
    }

    public async Task<List<Caixa>> GetByFilter(Expression<Func<Caixa, bool>> filter, CancellationToken cancellationToken)
    {
        var filtroBuild = Builders<Caixa>.Filter;

        return await dbContext.GetCollection<Caixa>().Find(filtroBuild.Where(filter)).ToListAsync(cancellationToken);
    }
}