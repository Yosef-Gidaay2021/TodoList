
namespace TodoListAPI.Models{
public class ToDoItem{

    public long Id { get; set; }
    public string ? Description { get; set;}
    public DateTime DueDate { get; set; }
    public DateTime ? CompletedDate { get; set;} 
   }
}
