using System;
using System.Collections.Generic;

namespace TodoSql
{
    public class TodoItem
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public bool IsCompleted
        {
            get => DateCompleted.HasValue;
        }

        public DateTime? DateCompleted { get; set; }

        public DateTime DateCreated { get; set; }

        public TodoItem(string text)
        {
            Id = Guid.NewGuid();
            DateCreated = DateTime.UtcNow;
            Text = text;
            Labels = new List<TodoItemLabel>();
        }

        public bool MarkAsCompleted()
        {
            if (!IsCompleted)
            {
                DateCompleted = DateTime.UtcNow;
                return true;
            }
            return false;
        }

        public Guid UserId { get; set; }

        public List<TodoItemLabel> Labels { get; set; }

        public DateTime? DateDue { get; set; }

        public TodoItem(string text, Guid userId)
        {
            Id = Guid.NewGuid();
            Text = text;
            DateCreated = DateTime.UtcNow;
            UserId = userId;
            Labels = new List<TodoItemLabel>();
        }

        public TodoItem()
        {
        }

        public override bool Equals(object obj)
        {
            return obj is TodoItem && ((TodoItem)obj).Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}