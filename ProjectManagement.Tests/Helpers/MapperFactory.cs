using AutoMapper;
using ProjectManagement.API.Mappings;

namespace ProjectManagement.Tests.Helpers;

public static class MapperFactory
{
    public static IMapper Create()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(EmployeeProfile).Assembly);
        });

        configuration.AssertConfigurationIsValid();

        return configuration.CreateMapper();
    }
}