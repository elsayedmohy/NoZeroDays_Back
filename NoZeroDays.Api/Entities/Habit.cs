namespace NoZeroDays.Api.Entities;

public sealed class Habit
{
    public string Id { get; set; }
    public string Name { get; set; } =  string.Empty;
    public string? Description { get; set; } 
    public HabitType Type { get; set; } 
    public Frequency Frequency  { get; set; } 
    public Target Target  { get; set; } 
    public HabitStatus Status  { get; set; } 
    public bool IsArchived  { get; set; } 
    public DateOnly? EndDate  { get; set; }
    public Milestone? Milestone  { get; set; }
    public DateTime CreatedAt  { get; set; }
    public DateTime? UpdatedAt  { get; set; }
    public DateTime? LastCompletedAt  { get; set; }
    public List<HabitTag> HabitTags { get; set; }
    public List<Tag> Tags { get; set; } = [];
    
}
