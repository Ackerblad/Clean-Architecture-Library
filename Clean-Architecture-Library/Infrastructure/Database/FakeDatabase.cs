using Domain.Entities;

namespace Infrastructure
{
    public class FakeDatabase 
    {
        public List<Book> Books { get { return booksInDb; } set { booksInDb = value; } }
        public List<Author> Authors { get { return authorsInDb; } set { authorsInDb = value; } }

        private static List<Book> booksInDb = new()
        {
            new Book(1, "American Gods", "A battle between old gods and new unfolds in modern America.", 1),
            new Book(2, "The Graveyard Book", "A boy raised by ghosts uncovers the mystery of his past.", 1),
            new Book(3, "Murder on the Orient Express", "Poirot solves a murder aboard a stranded luxury train.", 2),
            new Book(4, "And Then There Were None", "Ten strangers are killed one by one on a remote island.", 2),
            new Book(5, "1984", "A dystopian world where Big Brother controls everything.", 3),
            new Book(6, "Animal Farm", "Farm animals revolt, exposing the dangers of power.", 3)
        };

        private static List<Author> authorsInDb = new()
        {
            new Author(1, "Neil", "Gaiman"),
            new Author(2, "Agatha", "Christie"),
            new Author(3, "George", "Orwell")
        };
    }
}