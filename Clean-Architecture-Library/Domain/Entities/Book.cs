﻿namespace Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        //Foreign key
        public int AuthorId { get; set; }

        public Book(int id, string title, string description, int authorId)
        {
            Id = id;
            Title = title;
            Description = description;
            AuthorId = authorId;
        }
    }
}