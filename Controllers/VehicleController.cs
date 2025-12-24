using AvvalOnline.Shop.Api.Attributes;
using AvvalOnline.Shop.Api.Messaging.Vehicle;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]/[action]")]
[ApiController]
public class VehicleController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehicleController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    [HttpPost]
    [CustomAuthorize(["Admin", "User"])] // کاربر خودش یا ادمین می‌تواند وسیله ثبت کند
    public async Task<VehicleCreateRes> Create(VehicleCreateReq req)
    {
        return await _vehicleService.CreateVehicleAsync(req);
    }

    [HttpPost]
    [CustomAuthorize(["Admin", "User"])]
    public async Task<VehicleUpdateRes> Update(int id, VehicleUpdateReq req)
    {
        return await _vehicleService.UpdateVehicleAsync(id, req);
    }

    [HttpPost]
    [CustomAuthorize(["Admin"])]
    public async Task<VehicleDeleteRes> Delete(VehicleDeleteReq req)
    {
        return await _vehicleService.DeleteVehicleAsync(req.Id);
    }

    [HttpGet]
    [CustomAuthorize(["Admin", "User"])]
    public async Task<GetVehicleByIdRes> GetById(int id)
    {
        return await _vehicleService.GetVehicleByIdAsync(new GetVehicleByIdReq { Id = id });
    }

    [HttpGet]
    [CustomAuthorize(["Admin"])]
    public async Task<GetVehiclesRes> GetAll()
    {
        return await _vehicleService.GetVehiclesAsync(new GetVehiclesReq());
    }

    [HttpGet]
    [CustomAuthorize(["Admin", "User"])]
    public async Task<SearchVehiclesRes> Search(string term)
    {
        return await _vehicleService.SearchVehiclesAsync(term);
    }
}
