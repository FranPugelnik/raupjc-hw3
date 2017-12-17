using System;
using System.Collections.Generic;

namespace TodoSql
{
    public class TodoItemLabel
    {
        public Guid Id { get; set; }

        public string Value { get; set; }

        public List<TodoItem> LabelTodoItems { get; set; }

        public TodoItemLabel(string value) : this()
        {
            Id = Guid.NewGuid();
            Value = value;
        }

        public TodoItemLabel()
        {
            LabelTodoItems = new List<TodoItem>();
        }

        public override bool Equals(object obj)
        {
            return obj is TodoItemLabel && ((TodoItemLabel) obj).Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

}