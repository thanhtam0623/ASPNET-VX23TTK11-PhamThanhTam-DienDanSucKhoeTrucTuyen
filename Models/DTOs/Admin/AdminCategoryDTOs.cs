using System.ComponentModel.DataAnnotations;

namespace ApiApplication.Models.DTOs.Admin
{
    public class AdminCategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int TopicCount { get; set; }
        public int PostCount { get; set; }
    }

    public class AdminCreateCategoryRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public string? Icon { get; set; }
        public string? Color { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
    }

    public class AdminUpdateCategoryRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public string? Icon { get; set; }
        public string? Color { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class AdminTagDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Color { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TopicCount { get; set; }
    }

    public class AdminCreateTagRequest
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public string? Color { get; set; }
    }

    public class AdminUpdateTagRequest
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public string? Color { get; set; }
    }

    public class AdminMergeTagsRequest
    {
        [Required]
        public List<int> TagIds { get; set; } = new List<int>();

        [Required]
        [MaxLength(50)]
        public string NewTagName { get; set; } = string.Empty;

        public string? Color { get; set; }
    }

    public class AdminCategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int TopicCount { get; set; }
        public int PostCount { get; set; }
    }
}
