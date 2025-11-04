namespace TrelloClone.Api.Models
{
    public class TaskItem
    {
        public int TaskItemId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ListId { get; set; }
        public List List { get; set; }
    }
}
