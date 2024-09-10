using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;
using Aurigma.DirectMail.Sample.WebApi.Dtos.Category;
using Aurigma.DirectMail.Sample.WebApi.Models.Category;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CategoryAppModels = Aurigma.DirectMail.Sample.App.Models.Category;
using DomainEntity = Aurigma.DirectMail.Sample.DomainEntities.Entities.Category;

namespace Aurigma.DirectMail.Sample.WebApi.Controllers;

/// <summary>
/// Used to perform operations with categories.
/// </summary>
/// <param name="mapper"></param>
/// <param name="categoryAppService"></param>
[ApiController]
[Route("api/direct-mail/v1/categories")]
public class CategoryController(IMapper mapper, ICategoryAppService categoryAppService)
    : ControllerBase
{
    private readonly ICategoryAppService _categoryAppService = categoryAppService;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Returns a list of all categories.
    /// </summary>
    /// <returns>List of categories.</returns>
    [HttpGet("", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CategoryDto>))]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
        var categories = await _categoryAppService.GetCategoriesAsync();

        var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
        return Ok(categoryDtos);
    }

    /// <summary>
    /// Creates a new category.
    /// </summary>
    /// <param name="model">Creation model <see cref="CategoryCreationModel"/></param>
    /// <returns>Created category.</returns>
    [HttpPost("", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CategoryDto))]
    public async Task<ActionResult<CategoryDto>> CreateCategory(
        [FromBody] CategoryCreationModel model
    )
    {
        var creationAppModel = new CategoryAppModels.CategoryCreationModel()
        {
            Title = model.Title,
        };

        var category = await _categoryAppService.CreateCategoryAsync(creationAppModel);
        var categoryDto = _mapper.Map<CategoryDto>(category);

        return CreatedAtAction(nameof(CreateCategory), categoryDto);
    }

    /// <summary>
    /// Returns a category by id.
    /// </summary>
    /// <param name="id">Category's id.</param>
    /// <returns>The found category.</returns>
    [HttpGet("{id}", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryDto>> GetCategory([FromRoute, Required] Guid id)
    {
        try
        {
            var category = await _categoryAppService.GetCategoryById(id);
            var categoryDto = _mapper.Map<CategoryDto>(category);

            return Ok(categoryDto);
        }
        catch (NotFoundAppException)
        {
            return NotFound();
        }
    }
}

internal class CategoryControllerMapperProfile : Profile
{
    public CategoryControllerMapperProfile()
    {
        CreateMap<DomainEntity.Category, CategoryDto>();
    }
}
