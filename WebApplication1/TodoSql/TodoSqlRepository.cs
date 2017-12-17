using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoSql
{
    public class TodoSqlRepository : ITodoRepository
    {
        private readonly TodoDbContext _context;

        public TodoSqlRepository(TodoDbContext context)
        {
            _context = context;
        }

        public void Add(TodoItem todoItem)
        {
            if(_context.TodoItems.Any(x => x.Id == todoItem.Id))
            {
                throw new DuplicateTodoItemException("already exists");
            }
            _context.TodoItems.Add(todoItem);
            _context.SaveChanges();
        }

        public TodoItem Get(Guid todoId, Guid userId)
        {
            if (!_context.TodoItems.FirstOrDefault(t => t.Id == todoId).UserId.Equals(userId))
            {
                throw new TodoAccessDeniedException("The User is not the owner of this TodoItem!");
            }
            TodoItem item = _context.TodoItems.FirstOrDefault(todoItem => todoItem.Id == todoId);
            if (item != null && item.UserId != userId)
            {
                throw new TodoAccessDeniedException($"User with id {userId} can not access item with id {todoId}.");
            }

            return item;
        }

        public List<TodoItem> GetActive(Guid userId)
        {
            return _context.TodoItems.Where(item => item.UserId == userId && item.DateCompleted == null).OrderByDescending(i=>i.DateCreated).ToList();
        }

        public List<TodoItem> GetAll(Guid userId)
        {
            return _context.TodoItems.Where(item => item.UserId == userId).ToList();
        }

        public List<TodoItem> GetCompleted(Guid userId)
        {
            return _context.TodoItems.Where(item => item.UserId == userId && item.DateCompleted != null).OrderByDescending(i=>i.DateCompleted).ToList();
        }

        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            return GetAll(userId).Where(filterFunction).ToList();
        }

        public bool MarkAsCompleted(Guid todoId, Guid userId)
        {
            TodoItem item = Get(todoId, userId);
            if (item == null)
            {
                return false;
            }

            item.MarkAsCompleted();
            _context.SaveChanges();
            return true;
        }

        public bool Remove(Guid todoId, Guid userId)
        {
            TodoItem item = Get(todoId, userId);
            if (item == null)
            {
                return false;
            }

            _context.TodoItems.Remove(item);
            _context.SaveChanges();
            return true;
        }

        public void Update(TodoItem todoItem, Guid userId)
        {
            TodoItem oldItem = Get(todoItem.Id, userId);
            if (oldItem == null)
            {
                Add(todoItem);
                return;
            }

            _context.Entry(oldItem).CurrentValues.SetValues(todoItem);
            _context.SaveChanges();
        }

        public void AddLabel(string labelText, Guid itemID)
        {
            TodoItem item = _context.TodoItems.Where(i => i.Id == itemID).FirstOrDefault();
            TodoItemLabel label = _context.TodoLabels.Where(l => l.Value == labelText).FirstOrDefault();
            if (label == null)
            {
                label = new TodoItemLabel(labelText);
                _context.TodoLabels.Add(label);
                label.LabelTodoItems.Add(item);
                item.Labels.Add(label);
            }
            else
            {
                item.Labels.Add(label);
                label.LabelTodoItems.Add(item);
            }
            _context.SaveChanges();
        }
    }
}



namespace TodoSql
{
    public interface ITodoRepository
    {
        /// <summary >
        /// Gets TodoItem for a given id. Throw TodoAccessDeniedException
        /// with appropriate message if user is not the owner of the Todo item
        /// </summary >
        /// <param name = "todoId" > Todo Id </param >
        /// <param name = "userId" >Id of the user that is trying to fetch the data </param>
        /// <returns> TodoItem if found, null otherwise </returns>
        TodoItem Get(Guid todoId, Guid userId);

        /// <summary>
        /// Adds new TodoItem object in database.
        /// If object with the same id already exists,
        /// method should throw DuplicateTodoItemException with the message "duplicate id: {id }".
        /// </summary>
        void Add(TodoItem todoItem);

        /// <summary>
        /// Tries to remove a TodoItem with given id from the database. 
        /// Throw TodoAccessDeniedException with appropriate message if user is not 
        /// the owner of the Todo item
        /// </summary >
        /// <param name = "todoId"> Todo Id </param>
        /// <param name = "userId" >Id of the user that is trying to remove the data</param>
        /// <returns>True if success, false otherwise</returns>
        bool Remove(Guid todoId, Guid userId);

        /// <summary>
        /// Updates given TodoItem in database.
        /// If TodoItem does not exist, method will add one. 
        /// Throw TodoAccessDeniedException with appropriate message if user 
        /// is not the owner of the Todo item
        /// </summary>
        /// <param name = "todoItem"> Todo item </param>
        /// <param name = "userId">Id of the user that is trying to update the data</param>
        void Update(TodoItem todoItem, Guid userId);

        /// <summary>
        /// Tries to mark a TodoItem as completed in database. 
        /// Throw TodoAccessDeniedException with appropriate message if user is not
        ///  the owner of the Todo item
        /// </summary>
        /// <param name = "todoId"> Todo Id </param>
        /// <param name = "userId">Id of the user that is trying to mark as completed</param>
        /// <returns> True if success, false otherwise </returns>
        bool MarkAsCompleted(Guid todoId, Guid userId);

        /// <summary>
        /// Gets all TodoItem objects in database for user , sorted by date created(descending )
        /// </summary>
        List<TodoItem> GetAll(Guid userId);

        /// <summary>
        /// Gets all incomplete TodoItem objects in database for user
        /// </summary>
        List<TodoItem> GetActive(Guid userId);

        /// <summary>
        /// Gets all completed TodoItem objects in database for user
        /// </summary>
        List<TodoItem> GetCompleted(Guid userId);

        /// <summary>
        /// Gets all TodoItem objects in database for user that apply to the filter
        /// </summary>
        List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId);

        void AddLabel(string labelText, Guid itemID);
    }

}