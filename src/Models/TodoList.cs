namespace TodoList.Models
{
    public class TodoList
    {   
        public int Id { get; set; }
        public string title { get; set; }
        public bool isDone { get; set; }
        public DateTime dueDate { get; set; }
        public string notes { get; set; }
    }
}