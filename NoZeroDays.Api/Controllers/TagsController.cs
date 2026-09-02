using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoZeroDays.Api.Database;
using NoZeroDays.Api.DTO;
using NoZeroDays.Api.DTO.Tag;
using NoZeroDays.Api.Entities;
using NoZeroDays.Api.Mapping.ManualMappings;
using NoZeroDays.Api.Mapping.Projections;
using NoZeroDays.Api.Service;

namespace NoZeroDays.Api.Controllers;

[ApiController]
[Route("tags")]
public sealed class TagsController(ApplicationDbContext context, Mapping.Mapperly.TagMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTags()
    {
        List<TagResponse> result = await context.Tags
            .AsNoTracking()
            .Select(TagProjections.ToResponse)
            .ToListAsync();
        return Ok(ApiResponse<List<TagResponse>>.Ok(result));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TagResponse>> GetTag(string id)
    {
        TagResponse result = await context.Tags
            .AsNoTracking()
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
        await validator.ValidateAndThrowAsync(request);
        if (await context.Tags.AnyAsync(t => t.Name == request.Name))
        {
            return Conflict($"Tag with name '{request.Name}' already exists.");
        }

        Tag tag = mapper.ToEntity(request);

        tag.Id = $"t_{Guid.CreateVersion7()}";
        tag.CreatedAt = DateTime.UtcNow;

        context.Tags.Add(tag);

        await context.SaveChangesAsync();

        TagResponse response = mapper.ToDto(tag);

        return CreatedAtAction(
            nameof(GetTag),
            new { id = response.Id },
            ApiResponse<TagResponse>.Ok(response));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTag(string id, TagRequest request)
    {
        Tag? tag = await context.Tags.Where(h => h.Id == id)
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
        Tag? tag = await context.Tags.Where(h => h.Id == id)
            .FirstOrDefaultAsync();
        if (tag is null)
        {
            return NotFound();
        }

        TagResponse tagDto = mapper.ToDto(tag);
        patchDocument.ApplyTo(tagDto);
        if (!TryValidateModel(tagDto))
        {
            return ValidationProblem(ModelState);
        }
        tag.Name = tagDto.Name;
        tag.Description = tagDto.Description;
        tag.UpdatedAt  = tagDto.UpdatedAt;
        
        await context.SaveChangesAsync();
        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTag(string id)
    {
        Tag? tag = await context.Tags.Where(tag => tag.Id == id)
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
