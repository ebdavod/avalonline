using System.ComponentModel.DataAnnotations;

namespace AvvalOnline.Shop.Api.Model.Entites
{
    // دسترسی‌های پیش‌فرض سیستم
    public static class SystemPermissions
    {
        // Product Permissions
        public const string ProductView = "Product.View";
        public const string ProductCreate = "Product.Create";
        public const string ProductEdit = "Product.Edit";
        public const string ProductDelete = "Product.Delete";

        // Order Permissions
        public const string OrderView = "Order.View";
        public const string OrderCreate = "Order.Create";
        public const string OrderEdit = "Order.Edit";
        public const string OrderCancel = "Order.Cancel";

        // User Permissions
        public const string UserView = "User.View";
        public const string UserCreate = "User.Create";
        public const string UserEdit = "User.Edit";
        public const string UserDelete = "User.Delete";

        // Customer Permissions
        public const string CustomerView = "Customer.View";
        public const string CustomerCreate = "Customer.Create";
        public const string CustomerEdit = "Customer.Edit";
        public const string CustomerDelete = "Customer.Delete";

        // Vehicle Permissions
        public const string VehicleView = "Vehicle.View";
        public const string VehicleCreate = "Vehicle.Create";
        public const string VehicleEdit = "Vehicle.Edit";
        public const string VehicleDelete = "Vehicle.Delete";

        // Category Permissions
        public const string CategoryView = "Category.View";
        public const string CategoryCreate = "Category.Create";
        public const string CategoryEdit = "Category.Edit";
        public const string CategoryDelete = "Category.Delete";

        // Report Permissions
        public const string ReportView = "Report.View";
        public const string ReportExport = "Report.Export";
    }
}