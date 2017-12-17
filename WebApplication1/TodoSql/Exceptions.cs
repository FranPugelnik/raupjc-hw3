using System;

namespace TodoSql
{
    public class TodoAccessDeniedException : Exception
    {
        public TodoAccessDeniedException()
        {
        }

        public TodoAccessDeniedException(String message) : base(message)
        {
        }
    }

    internal class DuplicateTodoItemException : Exception
    {
        public DuplicateTodoItemException()
        {
        }

        public DuplicateTodoItemException(string message) : base(message)
        {
        }
    }
}