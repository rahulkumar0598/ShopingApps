using AutoMapper;
using Discount.gRPC.Entities;
using Discount.gRPC.Protos;
using Discount.gRPC.Repository;
using Grpc.Core;

namespace Discount.gRPC.Services
{
    public class DiscountService:DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly ILogger<DiscountService> _logger;
        private readonly IMapper _mapper;

        public DiscountService(IDiscountRepository discountRepository, ILogger<DiscountService> logger,IMapper mapper)
        {
            _discountRepository = discountRepository ?? throw new ArgumentNullException(nameof(_discountRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _discountRepository.GetDiscount(request.ProductName);
            if(coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with productName = {request.ProductName} is not found"));
            }
            _logger.LogInformation("Discount is retrieved for ProductName:{productName},Amount:{amount}", coupon.ProductName, coupon.Amount);
            var couponModel=_mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
             await _discountRepository.CreateDiscount(coupon);
            _logger.LogInformation("Discount is successfully created. ProductName:{Productname}",coupon.ProductName);

            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
            
        }
        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            await _discountRepository.UpdateDiscount(coupon);
            _logger.LogInformation("Discount is successfully updated. ProductName:{Productname}", coupon.ProductName);

            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }
        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var deleted = await _discountRepository.DeleteDiscount(request.ProductName);
            var response = new DeleteDiscountResponse
            {
                Success = deleted
            };
            return response;
        }

    }
}
