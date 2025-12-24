using AvvalOnline.Shop.Api.Messaging.Vehicle;
using AvvalOnline.Shop.Api.Model;
using AvvalOnline.Shop.Api.Model.Entites;
using Microsoft.EntityFrameworkCore;

namespace AvvalOnline.Shop.Api.Services.Implementations
{
    public class VehicleService : IVehicleService
    {
        private readonly ShopDbContext _db;

        public VehicleService(ShopDbContext db)
        {
            _db = db;
        }

        public async Task<VehicleCreateRes> CreateVehicleAsync(VehicleCreateReq request)
        {
            var entity = new Vehicle
            {
                UserId = request.Entity.CustomerId,
                PlateNumber = request.Entity.PlateNumber,
                VIN = request.Entity.VIN,
                Brand = request.Entity.Brand,
                Model = request.Entity.Model,
                Year = request.Entity.Year,
                Color = request.Entity.Color,
                EngineNumber = request.Entity.EngineNumber,
                IsActive = request.Entity.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            await _db.Vehicles.AddAsync(entity);
            await _db.SaveChangesAsync();

            return new VehicleCreateRes
            {
                IsSuccess = true,
                Message = "وسیله نقلیه با موفقیت ثبت شد",
                Id = entity.Id
            };
        }

        public async Task<VehicleDeleteRes> DeleteVehicleAsync(int id)
        {
            var vehicle = await _db.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return new VehicleDeleteRes { IsSuccess = false, Message = "وسیله نقلیه یافت نشد" };
            }

            _db.Vehicles.Remove(vehicle);
            await _db.SaveChangesAsync();

            return new VehicleDeleteRes { IsSuccess = true, Message = "وسیله نقلیه حذف شد" };
        }

        public async Task<GetVehicleByIdRes> GetVehicleByIdAsync(GetVehicleByIdReq req)
        {
            var vehicle = await _db.Vehicles.FindAsync(req.Id);
            if (vehicle == null)
                return new GetVehicleByIdRes { IsSuccess = false, Message = "وسیله نقلیه یافت نشد" };

            var dto = new VehicleDTO
            {
                Id = vehicle.Id,
                CustomerId = vehicle.UserId,
                PlateNumber = vehicle.PlateNumber,
                VIN = vehicle.VIN,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                Year = vehicle.Year ?? 0,
                Color = vehicle.Color,
                EngineNumber = vehicle.EngineNumber,
                IsActive = vehicle.IsActive
            };

            return new GetVehicleByIdRes { IsSuccess = true, Entity = dto };
        }

        public async Task<GetVehiclesRes> GetVehiclesAsync(GetVehiclesReq req)
        {
            var vehicles = await _db.Vehicles.ToListAsync();
            var dtos = vehicles.Select(v => new VehicleDTO
            {
                Id = v.Id,
                CustomerId = v.UserId,
                PlateNumber = v.PlateNumber,
                VIN = v.VIN,
                Brand = v.Brand,
                Model = v.Model,
                Year = v.Year ?? 0,
                Color = v.Color,
                EngineNumber = v.EngineNumber,
                IsActive = v.IsActive
            }).ToList();

            return new GetVehiclesRes
            {
                IsSuccess = true,
                Entities = dtos,
                Message = "لیست وسایل نقلیه"
            };
        }

        public async Task<SearchVehiclesRes> SearchVehiclesAsync(string searchTerm)
        {
            var vehicles = await _db.Vehicles
                .Where(v => v.PlateNumber.Contains(searchTerm) ||
                            v.Brand.Contains(searchTerm) ||
                            v.Model.Contains(searchTerm))
                .ToListAsync();

            var dtos = vehicles.Select(v => new VehicleDTO
            {
                Id = v.Id,
                CustomerId = v.UserId,
                PlateNumber = v.PlateNumber,
                VIN = v.VIN,
                Brand = v.Brand,
                Model = v.Model,
                Year = v.Year ?? 0,
                Color = v.Color,
                EngineNumber = v.EngineNumber,
                IsActive = v.IsActive
            }).ToList();

            return new SearchVehiclesRes
            {
                IsSuccess = true,
                Entities = dtos,
                TotalCount = dtos.Count,
                Page = 1,
                PageSize = dtos.Count,
                Message = "نتایج جستجو"
            };
        }

        public async Task<VehicleUpdateRes> UpdateVehicleAsync(int id, VehicleUpdateReq request)
        {
            var vehicle = await _db.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return new VehicleUpdateRes { IsSuccess = false, Message = "وسیله نقلیه یافت نشد" };
            }

            vehicle.PlateNumber = request.Entity.PlateNumber;
            vehicle.VIN = request.Entity.VIN;
            vehicle.Brand = request.Entity.Brand;
            vehicle.Model = request.Entity.Model;
            vehicle.Year = request.Entity.Year;
            vehicle.Color = request.Entity.Color;
            vehicle.EngineNumber = request.Entity.EngineNumber;
            vehicle.IsActive = request.Entity.IsActive;

            await _db.SaveChangesAsync();

            return new VehicleUpdateRes { IsSuccess = true, Message = "وسیله نقلیه بروزرسانی شد" };
        }
    }
}
