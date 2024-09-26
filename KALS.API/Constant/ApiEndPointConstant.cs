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
        public const string UpdateProductRelationship = ProductById + "/product-relationship";
        public const string LabToProduct = ProductById + "/lab";
    }

    public class Lab
    {
        public const string LabEndPoint = ApiEndpoint + "/lab";
        public const string LabById = LabEndPoint + "/{id}";
    }

    public class Category
    {
        public const string CategoryEndPoint = ApiEndpoint + "/category";
        public const string CategoryById = CategoryEndPoint + "/{id}";
        public const string UpdateProductCategory = CategoryById + "/product-category";
    }
}