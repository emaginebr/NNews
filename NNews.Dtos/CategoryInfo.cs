using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NNews.Dtos
{
    public class CategoryInfo
    {
        [JsonPropertyName("category_id")]
        public long CategoryId { get; set; }

        [JsonPropertyName("parent_id")]
        public long? ParentId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(240, ErrorMessage = "Title cannot exceed 240 characters")]
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("article_count")]
        public int ArticleCount { get; set; }
    }
}
