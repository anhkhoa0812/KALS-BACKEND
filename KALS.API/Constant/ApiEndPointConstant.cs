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
        public const string ProductEndpoint = ApiEndpoint + "/product";
        public const string ProductById = ProductEndpoint + "/{id}";
    }
}