using AutoMapper;
using Paybliss.Models;
using Paybliss.Models.DataResponse;
using Paybliss.Models.Dto;
using Paybliss.Models.HttpResp;

namespace Paybliss
{
    public class AuthoMapperProfiler: Profile
    {
        public AuthoMapperProfiler()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<User, RegisterDto>();
            CreateMap<User, UserItems>();
            CreateMap<UserItems, User>();
            CreateMap<RegisterDto, User>();
            CreateMap<CablesDto, CableValues>();
            CreateMap<CableValues, CablesDto>();
        }
    }
}
