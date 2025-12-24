using AvvalOnline.Shop.Api.Infrastructure;
using AvvalOnline.Shop.Api.Messaging.Vehicle;

public interface IVehicleService
{
    Task<GetVehicleByIdRes> GetVehicleByIdAsync(GetVehicleByIdReq req);
    Task<GetVehiclesRes> GetVehiclesAsync(GetVehiclesReq req);
    Task<VehicleCreateRes> CreateVehicleAsync(VehicleCreateReq request);
    Task<VehicleUpdateRes> UpdateVehicleAsync(int id, VehicleUpdateReq request);
    Task<VehicleDeleteRes> DeleteVehicleAsync(int id);
    Task<SearchVehiclesRes> SearchVehiclesAsync(string searchTerm);
}



