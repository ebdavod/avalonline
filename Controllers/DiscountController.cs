using AvvalOnline.Shop.Api.Attributes;
using AvvalOnline.Shop.Api.Messaging.Discount;
using AvvalOnline.Shop.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AvvalOnline.Shop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpPost("create")]
        [CustomAuthorize(["Admin"])]
        public async Task<IActionResult> Create([FromBody] CreateDiscountReq req)
        {
            var res = await _discountService.CreateDiscountAsync(req);
            return Ok(res);
        }

        [HttpPost("update")]
        [CustomAuthorize(["Admin"])]
        public async Task<IActionResult> Update([FromBody] UpdateDiscountReq req)
        {
            var res = await _discountService.UpdateDiscountAsync(req);
            return Ok(res);
        }

        [HttpPost("delete/{id}")]
        [CustomAuthorize(["Admin"])]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _discountService.DeleteDiscountAsync(new DeleteDiscountReq { Id = id });
            return Ok(res);
        }

        [HttpGet("code/{code}")]
        [CustomAuthorize(["Admin", "User"])]
        public async Task<IActionResult> GetByCode(string code)
        {
            var res = await _discountService.GetDiscountByCodeAsync(new GetDiscountByCodeReq { Entity = code });
            return Ok(res);
        }

        [HttpGet("all")]
        [CustomAuthorize(["Admin"])]
        public async Task<IActionResult> GetAll([FromQuery] GetAllDiscountsReq req)
        {
            var res = await _discountService.GetAllDiscountsAsync(req);
            return Ok(res);
        }
    }
}
