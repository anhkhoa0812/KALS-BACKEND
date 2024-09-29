using AutoMapper;
using KALS.API.Constant;
using KALS.API.Models.Cart;
using KALS.API.Models.Product;
using KALS.API.Services.Interface;
using KALS.API.Utils;
using KALS.Domain.DataAccess;
using KALS.Domain.Entities;
using KALS.Repository.Interface;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace KALS.API.Services.Implement;

public class CartService: BaseService<CartService>, ICartService
{
    public CartService(IUnitOfWork<KitAndLabDbContext> unitOfWork, ILogger<CartService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(unitOfWork, logger, mapper, httpContextAccessor, configuration)
    {
    }

    public async Task<ICollection<CartModel>> AddToCartAsync(CartModel model)
    {
        var userId = JwtUtil.GetUserIdFromToken(_httpContextAccessor);
        if (userId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.User.UserNotFound);
        var user = await _unitOfWork.GetRepository<User>().SingleOrDefaultAsync(
            predicate:x => x.Id == userId
        );
        if (user == null) throw new BadHttpRequestException(MessageConstant.User.UserNotFound);

        var product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
            predicate: x => x.Id == model.ProductId
        );
        if(product == null) throw new BadHttpRequestException(MessageConstant.Product.ProductNotFound);
        var redis = ConnectionMultiplexer.Connect(_configuration.GetConnectionString("Redis"));
        var db = redis.GetDatabase();
        
        var key = "Cart:" + userId;
        var cartData =  await db.StringGetAsync(key);
        List<CartModel> cart = new();

        if (cartData.IsNullOrEmpty)
        {
            cart = new List<CartModel>();
        }
        else
        {
            cart = JsonConvert.DeserializeObject<List<CartModel>>(cartData);
        }
        var existedProduct = cart.FirstOrDefault(x => x.ProductId == model.ProductId);
        if (existedProduct != null)
        {
            existedProduct.Quantity += model.Quantity;
        }
        else
        {
            cart.Add(model);
        }
        var updatedCart = JsonConvert.SerializeObject(cart);
        await db.StringSetAsync(key, updatedCart);
        return cart;
    }

    public async Task<ICollection<CartModel>> GetCartAsync()
    {
        var userId = JwtUtil.GetUserIdFromToken(_httpContextAccessor);
        if (userId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.User.UserNotFound);
        var redis = ConnectionMultiplexer.Connect(_configuration.GetConnectionString("Redis"));
        var db = redis.GetDatabase();
        var key = "Cart:" + userId;
        
        var cartData = await db.StringGetAsync(key);

        if (cartData.IsNullOrEmpty)
        {
            return new List<CartModel>();
        }
        var cart = JsonConvert.DeserializeObject<List<CartModel>>(cartData);
        return cart;
    }

    public async Task<ICollection<CartModel>> RemoveFromCartAsync(Guid productId)
    {
        var userId = JwtUtil.GetUserIdFromToken(_httpContextAccessor);
        if (userId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.User.UserNotFound);

        if (productId == Guid.Empty) throw new BadHttpRequestException(MessageConstant.Product.ProductIdNotNull);
        var redis = ConnectionMultiplexer.Connect(_configuration.GetConnectionString("Redis"));
        var db = redis.GetDatabase();
        var key = "Cart:" + userId;
        var cartData = await db.StringGetAsync(key);
        
        if (cartData.IsNullOrEmpty)
        {
            return new List<CartModel>();
        }
        var cart = JsonConvert.DeserializeObject<List<CartModel>>(cartData);
        var product = cart.FirstOrDefault(x => x.ProductId == productId);

        if (product == null) throw new BadHttpRequestException(MessageConstant.Product.ProductNotFound);
        cart.Remove(product);
        if (!cart.Any())
        {
            await db.KeyDeleteAsync(key);
        }
        else
        {
            var updatedCart = JsonConvert.SerializeObject(cart);
            await db.StringSetAsync(key, updatedCart);
        }

        return cart;
    }
}