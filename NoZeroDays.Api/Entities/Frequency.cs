using NoZeroDays.Api.Enums;

namespace NoZeroDays.Api.Entities;

public sealed class Frequency
{
    public FrequencyType Type { get; set; }
   public int TimesPerPeriod { get; set; } 
}
