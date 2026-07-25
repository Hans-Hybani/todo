using System;

namespace TodoList.Models
{
    public class TodoList
    {   
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsDone { get; set; }
        public DateTime DueDate { get; set; }
        public string Notes { get; set; }
    }
}