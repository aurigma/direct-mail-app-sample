using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Aurigma.DirectMail.Sample.App.Exceptions.AppExceptions;
using Aurigma.DirectMail.Sample.App.Interfaces.Services.AppServices;
using Aurigma.DirectMail.Sample.WebApi.Dtos.Category;
using Aurigma.DirectMail.Sample.WebApi.Dtos.Product;
using Aurigma.DirectMail.Sample.WebApi.Models.Products;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DomainEntity = Aurigma.DirectMail.Sample.DomainEntities.Entities;
using ProductAppModels = Aurigma.DirectMail.Sample.App.Models.Product;

namespace Aurigma.DirectMail.Sample.WebApi.Controllers;

/// <summary>
/// Used to perform operations with products.
/// </summary>
/// <param name="productAppService"></param>
/// <param name="mapper"></param>
[ApiController]
[Route("api/direct-mail/v1/products")]
public class ProductController(IProductAppService productAppService, IMapper mapper)
    : ControllerBase
{
    private readonly IProductAppService _productAppService = productAppService;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Returns a list of all products.
    /// </summary>
    /// <param name="model">Request filter <see cref="ProductRequestModel"/></param>
    /// <returns>List of products.</returns>
    [HttpGet("", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductDto>))]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts(
        [FromQuery] ProductRequestModel model
    )
    {
        var requestAppModel = new ProductAppModels.ProductRequestModel()
        {
            CategoryId = model.CategoryId,
        };

        var products = await _productAppService.GetProductsAsync(requestAppModel);
        var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);

        return Ok(productDtos);
    }

    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="model">Creation model <see cref="ProductCreationModel"/></param>
    /// <returns></returns>
    [HttpPost("", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductDto))]
    public async Task<ActionResult<ProductDto>> CreateProduct(
        [FromBody, Required] ProductCreationModel model
    )
    {
        var creationAppModel = new ProductAppModels.ProductCreationModel()
        {
            Title = model.Title,
            Price = model.Price,
            CategoryId = model.CategoryId ?? null,
        };

        var product = await _productAppService.CreateProductAsync(creationAppModel);
        var productDto = _mapper.Map<ProductDto>(product);

        return CreatedAtAction(nameof(CreateProduct), productDto);
    }

    /// <summary>
    /// Returns a product by id.
    /// </summary>
    /// <param name="id">Product's id.</param>
    /// <returns>The found product.</returns>
    [HttpGet("{id}", Name = "[controller]_[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetProduct([FromRoute] Guid id)
    {
        try
        {
            var product = await _productAppService.GetProductByIdAsync(id);
            var productDto = _mapper.Map<ProductDto>(product);

            return Ok(productDto);
        }
        catch (NotFoundAppException)
        {
            return NotFound();
        }
    }
}

internal class ProductControllerMapperProfile : Profile
{
    public ProductControllerMapperProfile()
    {
        CreateMap<DomainEntity.Category.Category, CategoryDto>();
        CreateMap<DomainEntity.Product.Product, ProductDto>();
    }
}
