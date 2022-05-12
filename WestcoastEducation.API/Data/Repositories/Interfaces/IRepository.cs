namespace WestcoastEducation.API.Data.Repositories.Interfaces;

public interface IRepository<TViewModel, TPostViewModel, TPatchViewModel>
{
    Task<List<TViewModel>> GetAllAsync();
    Task<TViewModel?> GetByIdAsync(int id);
    Task AddAsync(TPostViewModel model);
    Task UpdateAsync(int id, TPostViewModel model);
    Task UpdateAsync(int id, TPatchViewModel model);
    Task DeleteAsync(int id);
    Task<bool> SaveAsync();
}