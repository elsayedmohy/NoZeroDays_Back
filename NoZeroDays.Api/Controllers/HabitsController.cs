using NoZeroDays.Api.Service.Auth;

namespace NoZeroDays.Api.Controllers;

[Authorize]
[ApiController]
[Route("habits")]
public sealed class HabitsController(ApplicationDbContext dbContext,
    UserContext userContext
    ) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetHabits(
        [FromQuery] HabitsQuery query,
        SortMappingProvider sortMappingProvider
    )
    {
        
        string? userId = await userContext.GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        
        if (!sortMappingProvider.ValidateMappings<HabitResponse, Habit>(query.Sort))
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                detail: $"The provided sort parameter is  invalid : {query.Sort}"
            );
        }

        query.Search ??= query.Search?.Trim().ToLower();

        SortMapping[] sortMappings = sortMappingProvider.GetMappings<HabitResponse, Habit>();

        IQueryable<HabitResponse> result = dbContext.Habits
            .Where(habit => habit.UserId == userId)
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
        string? userId = await userContext.GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        HabitResponse result = await dbContext.Habits
            .AsNoTracking()
            .Where(habit => habit.UserId == userId)
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
        string? userId = await userContext.GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        await validator.ValidateAndThrowAsync(request);
        Habit habit = HabitMapping.ToEntity(request , userId);
        habit.Id = $"h_{Guid.CreateVersion7()}";
        dbContext.Habits.Add(habit);
        await dbContext.SaveChangesAsync();
        HabitResponse response =HabitMapping.ToDto(habit);
        return CreatedAtAction(
            nameof(GetHabit),
            new { id = response.Id },
            ApiResponse<HabitResponse>.Ok(response)
        );    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateHabit(string id, HabitRequest request)
    {
        string? userId = await userContext.GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        Habit? habit = await dbContext.Habits
            .Where(h => h.UserId == userId)
            .Where(h => h.Id == id)
            .FirstOrDefaultAsync();
        if (habit is null)
        {
            return NotFound();
        }

        habit.HabitToDto(request);
        await dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> PatchHabit(string id, JsonPatchDocument<HabitResponse> patchDocument)
    {
        string? userId = await userContext.GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        Habit? habit = await dbContext.Habits
            .Where(h => h.UserId == userId)
            .Where(h => h.Id == id)
            .FirstOrDefaultAsync();
        if (habit is null)
        {
            return NotFound();
        }

        HabitResponse habitDto = HabitMapping.ToDto(habit);
        patchDocument.ApplyTo(habitDto);
        if (!TryValidateModel(habitDto))
        {
            return ValidationProblem(ModelState);
        }

        habit.Name = habitDto.Name;
        habit.Description = habitDto.Description;
        habit.UpdatedAt = habitDto.UpdatedAt;

        await dbContext.SaveChangesAsync();
        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteHabit(string id)
    {
        string? userId = await userContext.GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        Habit? habit = await dbContext.Habits
            .Where(h => h.UserId == userId)
            .Where(h => h.Id == id)
            .FirstOrDefaultAsync();
        if (habit is null)
        {
            return NotFound();
        }

        dbContext.Habits.Remove(habit);
        await dbContext.SaveChangesAsync();
        return NoContent();
    }
}
