using Discount.Grpc.Protos;
using System.Threading.Tasks;

namespace Basket.API.GRPCServices
{
    public class DiscountServices
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _client;

        public DiscountServices(DiscountProtoService.DiscountProtoServiceClient client)
        {
            this._client = client;
        }

        public async Task<CouponModel> GetDiscount(string productName)
        {
            GetDiscountRequest request = new GetDiscountRequest { ProductName = productName };
            return await _client.GetDiscountAsync(request);
        }
    }
}
