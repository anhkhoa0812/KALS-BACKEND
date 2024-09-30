namespace KALS.API.Constant;

public class ApiEndPointConstant
{
    static ApiEndPointConstant() {}
    
    public const string RootEndPoint = "/api";
    public const string ApiVersion = "/v1";
    public const string ApiEndpoint = RootEndPoint + ApiVersion;
    
    public class User
    {
        public const string UserEndpoint = ApiEndpoint + "/user";
        public const string Login = UserEndpoint + "/login";
    }

    public class Product
    {
        public const string ProductEndpoint = ApiEndpoint + "/products";
        public const string ProductById = ProductEndpoint + "/{id}";
        public const string UpdateProductRelationship = ProductById + "/product-relationship";
        public const string LabToProduct = ProductById + "/lab";
    }

    public class Lab
    {
        public const string LabEndPoint = ApiEndpoint + "/labs";
        public const string LabById = LabEndPoint + "/{id}";
    }

    public class Category
    {
        public const string CategoryEndPoint = ApiEndpoint + "/categories";
        public const string CategoryById = CategoryEndPoint + "/{id}";
        public const string UpdateProductCategory = CategoryById + "/product-category";
    }
    public class Cart
    {
        public const string CartEndPoint = ApiEndpoint + "/carts";
    }

    public class Auth
    {
        public const string SendOtp = ApiEndpoint + "/otp";
        public const string Signup = ApiEndpoint + "/signup";
        public const string Login = ApiEndpoint + "/login";
    }
}