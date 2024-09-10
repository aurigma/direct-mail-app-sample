using Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;
using Aurigma.DirectMail.Sample.App.Exceptions.Category;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.DomainServices;
using Aurigma.DirectMail.Sample.App.Models.Category;
using Aurigma.DirectMail.Sample.DomainEntities.Entities.Category;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Aurigma.DirectMail.Sample.App.Services.AppServices;

public class CategoryAppService(
    ICategoryService categoryService,
    ILogger<CategoryAppService> logger,
    IMapper mapper
) : ICategoryAppService
{
    private readonly ICategoryService _categoryService = categoryService;
    private readonly ILogger<CategoryAppService> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task<Category> CreateCategoryAsync(CategoryCreationModel model)
    {
        try
        {
            var category = await _categoryService.CreateCategoryAsync(_mapper.Map<Category>(model));
            LogSuccessCreation(category);

            return category;
        }
        catch (Exception ex)
        {
            LogErrorCreation(ex, model);
            throw;
        }
    }

    public async Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        try
        {
            var categories = await _categoryService.GetCategoriesAsync();
            LogSuccessGetting(categories.Count);

            return categories;
        }
        catch (Exception ex)
        {
            LogErrorGetting(ex);
            throw;
        }
    }

    public async Task<Category> GetCategoryById(Guid id)
    {
        try
        {
            var category = await _categoryService.GetCategoryAsync(id);
            LogSuccessGettingById(category);

            return category;
        }
        catch (CategoryNotFoundException ex)
        {
            LogGettingNotFound(ex, id);
            throw new NotFoundAppException(ex.Message, ex);
        }
        catch (Exception ex)
        {
            LogErrorGettingById(ex, id);
            throw;
        }
    }

    #region Logging
    private void LogSuccessCreation(Category category)
    {
        _logger.LogDebug(
            $"The category was created. Id: {category.Id}, product={Serialize(category)}"
        );
    }

    private void LogErrorCreation(Exception ex, CategoryCreationModel model)
    {
        _logger.LogError(ex, $"Failed to create a category. Request model={Serialize(model)}");
    }

    private void LogSuccessGetting(int categoriesCount)
    {
        _logger.LogDebug($" Categories was returned. categories count={categoriesCount}");
    }

    private void LogErrorGetting(Exception ex)
    {
        _logger.LogError(ex, $"Failed to request a categories");
    }

    private void LogSuccessGettingById(Category model)
    {
        _logger.LogDebug(
            $"Category was returned by ID. Entity id ={model.Id}. category={Serialize(model)}"
        );
    }

    private void LogErrorGettingById(Exception ex, Guid id)
    {
        _logger.LogError(ex, $"Failed to request a category by ID. Entity id ={id}");
    }

    private void LogGettingNotFound(Exception ex, Guid id)
    {
        _logger.LogError(ex, $"The category was not found. id={id}");
    }

    #endregion Logging

    private static string Serialize<T>(T source)
        where T : class
    {
        return StringHelper.Serialize(source);
    }
}

internal class CategoryAppServiceMapperProfile : Profile
{
    public CategoryAppServiceMapperProfile()
    {
        CreateMap<CategoryCreationModel, Category>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
    }
}
