namespace NoZeroDays.Api.Extensions;

public static class MapperExtension
{
    public static  void AddDataMapper(this IServiceCollection services)
    { 
       services.AddSingleton<HabitMapper>();
       services.AddSingleton<TagMapper>();
    }
}
