namespace NoZeroDays.Api.Controllers;

[ApiController]
[Route("habits")]
public sealed class HabitsController(ApplicationDbContext context, HabitMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetHabits(
        [FromQuery] HabitsQuery query,
        SortMappingProvider sortMappingProvider
    )
    {
        if (!sortMappingProvider.ValidateMappings<HabitResponse, Habit>(query.Sort))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                detail: $"The provided sort parameter is  invalid : {query.Sort}"
            );
        }

        query.Search ??= query.Search?.Trim().ToLower();

        SortMapping[] sortMappings = sortMappingProvider.GetMappings<HabitResponse, Habit>();

        IQueryable<HabitResponse> result = context.Habits
            .Where(habit => query.Search == null
                            || habit.Name.Contains(query.Search)
                            || habit.Description != null && habit.Description.Contains(query.Search))
            .Where(habit => query.Status == null || habit.Status == query.Status)
            .Where(habit => query.Type == null || habit.Type == query.Type)
            .ApplySort(query.Sort, sortMappings)
            .Select(HabitProjections.ToResponse);

        var paginationResult = await PaginationResult<HabitResponse>
            .CreateAsync(result,
                query.Page,
                query.PageSize);

        return Ok(ApiResponse<PaginationResult<HabitResponse>>.Ok(paginationResult));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HabitResponse>> GetHabit(string id)
    {
        HabitResponse result = await context.Habits
            .AsNoTracking()
            .Where(h => h.Id == id)
            .Select(HabitProjections.ToResponse)
            .FirstOrDefaultAsync();
        if (result == null)
        {
            return NotFound();
        }

        return Ok(ApiResponse<HabitResponse>.Ok(result));
    }

    [HttpPost]
    public async Task<ActionResult<HabitResponse>> CreateHabit(
        HabitRequest request,
        IValidator<HabitRequest> validator)
    {
        await validator.ValidateAndThrowAsync(request);
        Habit habit = mapper.ToEntity(request);
        habit.Id = $"h_{Guid.CreateVersion7()}";
        context.Habits.Add(habit);
        await context.SaveChangesAsync();
        HabitResponse response = mapper.ToDto(habit);
        return CreatedAtAction(nameof(GetHabit), new { id = response.Id }, Ok(ApiResponse<HabitResponse>.Ok(response)));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateHabit(string id, HabitRequest request)
    {
        Habit? habit = await context.Habits.Where(h => h.Id == id)
            .FirstOrDefaultAsync();
        if (habit is null)
        {
            return NotFound();
        }

        habit.HabitToDto(request);
        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> PatchHabit(string id, JsonPatchDocument<HabitResponse> patchDocument)
    {
        Habit? habit = await context.Habits.Where(h => h.Id == id)
            .FirstOrDefaultAsync();
        if (habit is null)
        {
            return NotFound();
        }

        HabitResponse habitDto = mapper.ToDto(habit);
        patchDocument.ApplyTo(habitDto);
        if (!TryValidateModel(habitDto))
        {
            return ValidationProblem(ModelState);
        }

        habit.Name = habitDto.Name;
        habit.Description = habitDto.Description;
        habit.UpdatedAt = habitDto.UpdatedAt;

        await context.SaveChangesAsync();
        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteHabit(string id)
    {
        Habit? habit = await context.Habits.Where(h => h.Id == id)
            .FirstOrDefaultAsync();
        if (habit is null)
        {
            return NotFound();
        }

        context.Habits.Remove(habit);
        await context.SaveChangesAsync();
        return NoContent();
    }
}
