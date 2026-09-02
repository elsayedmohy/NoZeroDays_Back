using NoZeroDays.Api.DTO.Tag;
using NoZeroDays.Api.Entities;
using Riok.Mapperly.Abstractions;

namespace NoZeroDays.Api.Mapping.Mapperly;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public partial class  TagMapper
{
    public partial TagResponse ToDto(Tag tag);
    public partial Tag ToEntity(TagRequest dto);
}
