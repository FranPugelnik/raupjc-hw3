using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models.Todo
{
    public class CompletedViewModel
    {
        public IList<TodoViewModel> TodoList { get; }

        public CompletedViewModel(IList<TodoViewModel> models)
        {
            TodoList = models;
        }

    }
}
