using AutoMapper;
using KALS.API.Models.Product;
using KALS.Domain.Entity;

namespace KALS.API.Mapper;

public class ProductMapper: Profile
{
    public ProductMapper()
    {
        CreateMap<CreateProductRequest, Product>();
        CreateMap<Product, GetProductResponse>();
    }
}