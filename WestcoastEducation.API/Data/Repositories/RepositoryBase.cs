using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.API.Data.Entities;
using WestcoastEducation.API.Data.Repositories.Interfaces;

namespace WestcoastEducation.API.Data.Repositories;

public abstract class RepositoryBase<TEntity, TViewModel, TPostViewModel, TPatchViewModel> 
    : IRepository<TViewModel, TPostViewModel, TPatchViewModel> 
    where TEntity : class, IEntity
{
    protected ApplicationContext Context { get; }
    protected IMapper Mapper { get; }

    protected RepositoryBase(ApplicationContext context, IMapper mapper)
    {
        Context = context;
        Mapper = mapper;
    }
    
    public async Task<List<TViewModel>> GetAllAsync()
    {
        return await Context
            .Set<TEntity>()
            .ProjectTo<TViewModel>(Mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<TViewModel?> GetByIdAsync(string id)
    {
        return await Context
            .Set<TEntity>()
            .Where(e => e.Id == id)
            .ProjectTo<TViewModel>(Mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    public abstract Task AddAsync(TPostViewModel model);

    public abstract Task UpdateAsync(string id, TPostViewModel model);
    
    public abstract Task UpdateAsync(string id, TPatchViewModel model);

    public async Task DeleteAsync(string id)
    {
        var entity = await Context
            .Set<TEntity>()
            .FindAsync(id);

        if (entity is null)
        {
            throw new Exception($"Could not find {typeof(TEntity).Name} with id {id}.");
        }

        Context
            .Set<TEntity>()
            .Remove(entity);
    }

    public async Task<bool> SaveAsync()
    {
        return await Context.SaveChangesAsync() > 0;
    }
}