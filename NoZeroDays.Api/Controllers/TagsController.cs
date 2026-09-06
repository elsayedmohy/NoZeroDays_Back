using NoZeroDays.Api.Service.Auth;

namespace NoZeroDays.Api.Controllers;

[Authorize]
[ApiController]
[Route("tags")]
public sealed class TagsController(
    ApplicationDbContext context,
    UserContext userContext
    ) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTags()
    {
        string? userId = await userContext.GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        List<TagResponse> result = await context.Tags
            .Where(t => t.UserId  == userId) 
            .AsNoTracking()
            .Select(TagProjections.ToResponse)
            .ToListAsync();
        return Ok(ApiResponse<List<TagResponse>>.Ok(result));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TagResponse>> GetTag(string id)
    {
        string? userId = await userContext.GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        TagResponse result = await context.Tags
            .AsNoTracking()
            .Where(t => t.UserId  == userId) 
            .Where(t => t.Id == id)
            .Select(TagProjections.ToResponse)
            .FirstOrDefaultAsync();
        if (result == null)
        {
            return NotFound();
        }

        return Ok(ApiResponse<TagResponse>.Ok(result));
    }

    [HttpPost]
    public async Task<ActionResult<TagResponse>> CreateTag(
        TagRequest request,
        IValidator<TagRequest> validator)
    {
        string? userId = await userContext.GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        await validator.ValidateAndThrowAsync(request);
        if (await context.Tags.AnyAsync(t => t.Name == request.Name))
        {
            return Conflict($"Tag with name '{request.Name}' already exists.");
        }

        Tag tag = TagMapping.ToEntity(request,userId);

        tag.Id = $"t_{Guid.CreateVersion7()}";
        tag.CreatedAt = DateTime.UtcNow;

        context.Tags.Add(tag);

        await context.SaveChangesAsync();

        TagResponse response = TagMapping.ToResponse(tag);

        return CreatedAtAction(
            nameof(GetTag),
            new { id = response.Id },
            ApiResponse<TagResponse>.Ok(response));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTag(string id, TagRequest request)
    {
        string? userId = await userContext.GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        Tag? tag = await context.Tags
            .Where(t => t.UserId  == userId) 
            .Where(h => h.Id == id)
            .FirstOrDefaultAsync();
        if (tag is null)
        {
            return NotFound();
        }

        tag.UpdateTag(request);
        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> PatchTag(string id, JsonPatchDocument<TagResponse> patchDocument)
    {
        string? userId = await userContext.GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        Tag? tag = await context.Tags
            .Where(t => t.UserId  == userId) 
            .Where(h => h.Id == id)
            .FirstOrDefaultAsync();
        if (tag is null)
        {
            return NotFound();
        }

        TagResponse tagDto = TagMapping.ToResponse(tag);
        patchDocument.ApplyTo(tagDto);
        if (!TryValidateModel(tagDto))
        {
            return ValidationProblem(ModelState);
        }

        tag.Name = tagDto.Name;
        tag.Description = tagDto.Description;
        tag.UpdatedAt = tagDto.UpdatedAt;

        await context.SaveChangesAsync();
        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTag(string id)
    {
        string? userId = await userContext.GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }
        Tag? tag = await context.Tags
            .Where(t => t.UserId  == userId) 
            .Where(tag => tag.Id == id)
            .FirstOrDefaultAsync();
        if (tag is null)
        {
            return NotFound();
        }

        context.Tags.Remove(tag);
        await context.SaveChangesAsync();
        return NoContent();
    }
}
