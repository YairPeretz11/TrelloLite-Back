using System.Collections.Generic;

namespace TrelloClone.Api.Models
{
    public class Board
    {
        public int BoardId { get; set; }
        public string Name { get; set; }
        public ICollection<List> Lists { get; set; } = new List<List>();
    }
}
