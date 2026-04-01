namespace dotnet_backend.Modules.Todos.Entities
{
    public class TodoItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } = "pending";
        public string Priority { get; set; } = "medium";
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
    }
}
