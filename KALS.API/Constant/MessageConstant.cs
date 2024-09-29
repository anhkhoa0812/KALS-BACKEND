namespace KALS.API.Constant;

public class MessageConstant
{
    public static class User
    {
        public const string UserNotFound = "Username không tồn tại";
        public const string PasswordIncorrect = "Mật khẩu không chính xác";
    }

    public static class Product
    {
        public const string ChildProductNotFound = "Sản phẩm con không tồn tại";
        public const string CreateProductFail = "Tạo sản phẩm không thành công với tên";
        public const string ProductIdNotNull = "Id sản phẩm không được để trống";
        public const string ProductNotFound = "Sản phẩm không tồn tại";
        public const string UpdateProductFail = "Cập nhật sản phẩm không thành công";
        public const string UpdateProductRelationshipFail = "Cập nhật quan hệ sản phẩm không thành công";
        public const string ParentProductIdNotNull = "Id của sản phẩm kit không được để trống";
        public const string ChildProductIdNotNull = "Id của sản phẩm con không được để trống";
    }
    public static class Lab
    {
        public const string AssignLabToProductFail = "Gán lab cho sản phẩm không thành công";
        public const string LabIdNotNull = "Id lab không được để trống";
    }

    public static class Category
    {
        public const string  CategoryIdNotNull = "Id danh mục không được để trống";
        public const string CategoryNotFound = "Danh mục không tồn tại";
        public const string UpdateProductCategoryFail = "Cập nhật danh mục của sản phẩm không thành công";
        public const string UpdateCategoryFail = "Cập nhật danh mục không thành công";
        public const string CreateCategoryFail = "Tạo danh mục không thành công";
    }

    public static class Cart
    {
        public const string AddToCartFail = "Thêm sản phẩm vào giỏ hàng không thành công";
        public const string RemoveFromCartFail = "Xóa sản phẩm khỏi giỏ hàng không thành công";
    }
}