using System;

namespace Hydra.Models
{
    public class Comment
    {
        public int ID { get; set; }
        public User Publisher { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
