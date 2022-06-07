using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WestcoastEducation.API.Data.Entities;
using WestcoastEducation.API.Data.Repositories.Interfaces;
using WestcoastEducation.API.ViewModels.Category;

namespace WestcoastEducation.API.Controllers;

[ApiController]
[Route("api/v1/categories")]
public class CategoriesController : Controller
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoriesController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    [HttpGet("list")]
    public async Task<ActionResult<List<CategoryViewModel>>> ListCategories()
    {
        return Ok(await _categoryRepository.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryViewModel>> GetCategoryById(string id)
    {
        var response = await _categoryRepository.GetByIdAsync(id);

        if (response is null)
        {
            return NotFound($"No {nameof(Category).ToLower()} with id {id} could be found.");
        }

        return Ok(response);
    }
    
    [HttpPost]
    public async Task<ActionResult> AddCategory(PostCategoryViewModel model)
    {
        try
        {
            await _categoryRepository.AddAsync(model);

            if (await _categoryRepository.SaveAsync())
            {
                return StatusCode(201);
            }

            return StatusCode(500, $"Could not save {nameof(Category).ToLower()} to database.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCategory(string id, PostCategoryViewModel model)
    {
        try
        {
            await _categoryRepository.UpdateAsync(id, model);

            if (await _categoryRepository.SaveAsync())
            {
                return NoContent();
            }

            return StatusCode(500, $"Could not update {nameof(Category).ToLower()}.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpPatch("{id}")]
    public async Task<ActionResult> UpdateCategory(string id, PatchCategoryViewModel model)
    {
        try
        {
            await _categoryRepository.UpdateAsync(id, model);

            if (await _categoryRepository.SaveAsync())
            {
                return NoContent();
            }

            return StatusCode(500, $"Could not update {nameof(Category).ToLower()}.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCategory(string id)
    {
        try
        {
            await _categoryRepository.DeleteAsync(id);

            if (await _categoryRepository.SaveAsync())
            {
                return NoContent();
            }

            return StatusCode(500, $"Could not delete {nameof(Category).ToLower()} from database.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}