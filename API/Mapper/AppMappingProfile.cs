using AutoMapper;

namespace Mapper;

public class AppMappingProfile : Profile
{
	public AppMappingProfile()
	{
		CreateMap<RequestObjects.Supply, DAL.Models.Supply>();
	}
}
