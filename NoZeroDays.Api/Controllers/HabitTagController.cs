namespace NoZeroDays.Api.Controllers;

[Authorize]
[ApiController]
[Route("habits/{habitId}/tags")]
public class HabitTagController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpPut()]
    public async Task<IActionResult> UpdateHabitTags(
        string habitId,
        UpdateHabitTagsRequest request)
    {
        bool habitExists = await dbContext.Habits
            .AnyAsync(h => h.Id == habitId);

        if (!habitExists)
        {
            return NotFound("Habit not found.");
        }

        var tagIds = request.TagIds
            .Distinct()
            .ToList();

        List<string> validTagIds = await dbContext.Tags
            .Where(t => tagIds.Contains(t.Id))
            .Select(t => t.Id)
            .ToListAsync();

        if (validTagIds.Count != tagIds.Count)
        {
            return BadRequest("One or more tags do not exist.");
        }

        var requestedTagIds = tagIds.ToHashSet();

        List<HabitTag> existingHabitTags = await dbContext.HabitTags
            .Where(x => x.HabitId == habitId)
            .ToListAsync();

        var existingTagIds = existingHabitTags
            .Select(x => x.TagId)
            .ToHashSet();

        // Remove
        var tagsToRemove = existingHabitTags
            .Where(x => !requestedTagIds.Contains(x.TagId))
            .ToList();

        dbContext.HabitTags.RemoveRange(tagsToRemove);

        // Add
        IEnumerable<HabitTag> tagsToAdd = tagIds
            .Where(tagId => !existingTagIds.Contains(tagId))
            .Select(tagId => new HabitTag
            {
                HabitId = habitId,
                TagId = tagId,
                CreatedAt = DateTime.Now
            });

        await dbContext.HabitTags.AddRangeAsync(tagsToAdd);

        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}
