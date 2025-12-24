using AvvalOnline.Shop.Api.Infrastructure;
using AvvalOnline.Shop.Api.Model.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace AvvalOnline.Shop.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class CustomAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string[] _requiredPermissions;
        private readonly string[] _requiredRoles;

        public CustomAuthorizeAttribute()
        {
            // فقط احراز هویت required است
        }

        public CustomAuthorizeAttribute(params string[] roles)
        {
            _requiredRoles = roles;
        }
        public CustomAuthorizeAttribute(string permission = null)
        {
            if (!string.IsNullOrEmpty(permission))
                _requiredPermissions = new[] { permission };
        }
        public CustomAuthorizeAttribute(string[] roles, string permission = null)
        {
            _requiredRoles = roles;
            if (!string.IsNullOrEmpty(permission))
                _requiredPermissions = new[] { permission };
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                // استخراج توکن از هدر
                var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token))
                {
                    context.Result = new JsonResult(new { message = "توکن احراز هویت ارسال نشده است" })
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized
                    };
                    return;
                }

                // اعتبارسنجی توکن
                var jwtUtils = context.HttpContext.RequestServices.GetRequiredService<IJwtUtils>();
                var principal = jwtUtils.ValidateToken(token);

                if (principal == null)
                {
                    context.Result = new JsonResult(new { message = "توکن نامعتبر است" })
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized
                    };
                    return;
                }

                // استخراج اطلاعات کاربر از توکن
                var userId = jwtUtils.GetUserIdFromToken(token);
                if (!userId.HasValue)
                {
                    context.Result = new JsonResult(new { message = "اطلاعات کاربر در توکن یافت نشد" })
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized
                    };
                    return;
                }

                var userRoles = jwtUtils.GetUserRolesFromToken(token);
                // بررسی نقش‌های required
                if (_requiredRoles != null && _requiredRoles.Any())
                {
                    if (!_requiredRoles.Any(userRoles.Contains))
                    {
                        context.Result = new JsonResult(new { message = "شما دسترسی لازم برای این عملیات را ندارید" })
                        {
                            StatusCode = (int)HttpStatusCode.Forbidden
                        };
                        return;
                    }
                }

                // بررسی دسترسی‌های required
                if (_requiredPermissions != null && _requiredPermissions.Any())
                {
                    var userPermissions = jwtUtils.GetUserPermissionsFromToken(token);
                    if (!_requiredPermissions.All(userPermissions.Contains))
                    {
                        context.Result = new JsonResult(new { message = "شما مجوز لازم برای این عملیات را ندارید" })
                        {
                            StatusCode = (int)HttpStatusCode.Forbidden
                        };
                        return;
                    }
                }
                // ذخیره اطلاعات کاربر در HttpContext برای استفاده در کنترلرها
                context.HttpContext.Items["UserId"] = userId.Value;
                context.HttpContext.Items["UserRoles"] = jwtUtils.GetUserRolesFromToken(token);
                context.HttpContext.Items["UserPermissions"] = jwtUtils.GetUserPermissionsFromToken(token);

            }
            catch (Exception ex)
            {
                var logger = context.HttpContext.RequestServices.GetService<ILogger<CustomAuthorizeAttribute>>();
                logger?.LogError(ex, "Error in authorization");

                context.Result = new JsonResult(new { message = "خطا در احراز هویت" })
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }
    }
}