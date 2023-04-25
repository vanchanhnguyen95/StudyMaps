using AutoMapper;
using CpGeoService.Model;

namespace CpGeoService.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<DataMerg, DataPNC>();
            CreateMap<DataPNC, DataMerg>();
            //CreateMap<OrderDto, Order>();
            //CreateMap<Customer, CustomerDto>();
        }
    }
}
