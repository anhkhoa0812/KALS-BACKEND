using AutoMapper;
using KALS.API.Models.Cart;
using KALS.API.Models.Product;
using KALS.Domain.Entities;

namespace KALS.API.Mapper;

public class ProductMapper: Profile
{
    public ProductMapper()
    {
        CreateMap<CreateProductRequest, Product>();
        CreateMap<Product, GetProductResponse>();
        CreateMap<Product, CartModelResponse>();
    }
}