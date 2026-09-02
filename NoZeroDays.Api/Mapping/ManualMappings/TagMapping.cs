namespace NoZeroDays.Api.Mapping.ManualMappings;

public static class TagMapping
{
    public static void UpdateTag(this Tag tag,  TagRequest request)
    {
        tag.Name = request.Name;
        tag.Description = request.Description;
        tag.UpdatedAt = DateTime.Now;
    }
}
