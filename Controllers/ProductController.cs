using AvvalOnline.Shop.Api.Attributes;
using AvvalOnline.Shop.Api.Messaging.Product;
using AvvalOnline.Shop.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AvvalOnline.Shop.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        [CustomAuthorize(["Admin"])]
        public async Task<CreateProductRes> Create(CreateProductReq req)
        {
            return await _productService.CreateProductAsync(req);
        }

        [HttpPost]
        [CustomAuthorize(["Admin"])]
        public async Task<UpdateProductRes> Update(UpdateProductReq req)
        {
            return await _productService.UpdateProductAsync(req);
        }

        [HttpPost]
        [CustomAuthorize(["Admin"])]
        public async Task<DeleteProductRes> Delete(DeleteProductReq req)
        {
            return await _productService.DeleteProductAsync(req);
        }

        [HttpGet]
        [CustomAuthorize(["Admin", "User"])]
        public async Task<GetAllProductsRes> GetAll(int index, int size)
        {
            return await _productService.GetAllProductsAsync(new GetAllProductsReq
            {
                Page = index,
                PageSize = size
            });
        }
        [HttpGet]
        public async Task<GetAllProductsRes> Search(string searchText,int index, int size)
        {
            return await _productService.GetAllProductsAsync(new GetAllProductsReq
            {
                Page = index,
                PageSize = size
            });
        }

        [HttpGet]
        //[CustomAuthorize(["Admin", "User"])] 
        public async Task<GetProductByCodeRes> GetByCode(string code)
        {
            return await _productService.GetProductByCodeAsync(new GetProductByCodeReq
            {
                Entity = new GetProductByCodeDTO { Code = code }
            });
        }

        [HttpGet]
        [CustomAuthorize(["Admin", "User"])] 
        public async Task<GetProductByIdRes> GetById(int id)
        {
            return await _productService.GetProductByIdAsync(new GetProductByIdReq { Id = id });
        }
    }
}
