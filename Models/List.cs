using System.Text.Json.Serialization;

namespace TrelloClone.Api.Models
{
    public class List
    {
        public int ListId { get; set; }
        public string Name { get; set; }
        public int BoardId { get; set; }
        [JsonIgnore]
        public Board? Board { get; set; }
        public ICollection<TaskItem> Tasks { get; set; } = [];
    }
}
