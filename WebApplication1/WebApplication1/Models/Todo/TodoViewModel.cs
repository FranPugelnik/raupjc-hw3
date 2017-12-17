using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models.Todo
{
    public class TodoViewModel
    {
        public string Title { get; set; }
        public DateTime? DateDue { get; set; }
        public Guid Id { get; set; }
        public string LinkText { get; set; }
        public bool ShowOffset { get; set; }
    }
}
