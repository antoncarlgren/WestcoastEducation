namespace WestcoastEducation.API.Data.Repositories.Interfaces;

public interface IRepository<TViewModel, TPostViewModel, TPatchViewModel>
{
    Task<List<TViewModel>> GetAllAsync();
    Task<TViewModel?> GetByIdAsync(string id);
    Task AddAsync(TPostViewModel model);
    Task UpdateAsync(string id, TPostViewModel model);
    Task UpdateAsync(string id, TPatchViewModel model);
    Task DeleteAsync(string id);
    Task<bool> SaveAsync();
}