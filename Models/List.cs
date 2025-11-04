using System.Collections.Generic;

namespace TrelloClone.Api.Models
{
    public class List
    {
        public int ListId { get; set; }
        public string Name { get; set; }
        public int BoardId { get; set; }
        public Board Board { get; set; }
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
