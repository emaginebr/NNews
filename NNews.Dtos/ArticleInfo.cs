using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NNews.Dtos
{
    public class ArticleInfo
    {
        [JsonPropertyName("article_id")]
        public long ArticleId { get; set; }

        [Required(ErrorMessage = "CategoryId is required")]
        [Range(1, long.MaxValue, ErrorMessage = "CategoryId must be greater than 0")]
        [JsonPropertyName("category_id")]
        public long CategoryId { get; set; }

        [JsonPropertyName("author_id")]
        public long? AuthorId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters")]
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Content is required")]
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        [Range(0, 2, ErrorMessage = "Status must be 0 (Draft), 1 (Published), or 2 (Archived)")]
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("category")]
        public CategoryInfo? Category { get; set; }

        [JsonPropertyName("tags")]
        public List<TagInfo> Tags { get; set; } = new();

        [JsonPropertyName("roles")]
        public List<RoleInfo> Roles { get; set; } = new();
    }
}
