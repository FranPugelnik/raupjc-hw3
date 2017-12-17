using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models.Todo
{
    public class IndexViewModel
    {
        public IList<TodoViewModel> TodoList { get; }

        public IndexViewModel(IList<TodoViewModel> models)
        {
            TodoList = models;
        }
    }
}
