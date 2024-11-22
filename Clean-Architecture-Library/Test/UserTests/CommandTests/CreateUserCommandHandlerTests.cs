using Application.Commands.Users.CreateUser;
using Infrastructure;

namespace Test.UserTests.CommandTests
{
    public class CreateUserCommandHandlerTests
    {
        private FakeDatabase _fakeDatabase;
        private CreateUserCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fakeDatabase = new FakeDatabase();
            _handler = new CreateUserCommandHandler(_fakeDatabase);
        }

        [Test]
        public async Task Handle_UserIsUnique_AddsUserToDatabase()
        {
            //Arrange
            var initialUsers = _fakeDatabase.Users.Count;
            var command = new CreateUserCommand("TestUser", "TestPassword");

            //Act
            var createdUser = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.That(createdUser.UserName, Is.EqualTo("TestUser"));
            Assert.That(_fakeDatabase.Users.Count, Is.EqualTo(initialUsers + 1));
        }

        [Test]
        public void Handle_UserNameAlreadyExists_ThrowsInvalidOperationException()
        {
            //Arrange
            var initialUsers = _fakeDatabase.Users.Count;
            var existingUser = _fakeDatabase.Users[0];
            var command = new CreateUserCommand(existingUser.UserName, existingUser.Password);

            //Act
            Task action() => _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.ThrowsAsync<InvalidOperationException>(action);
            Assert.That(_fakeDatabase.Users.Count, Is.EqualTo(initialUsers));
        }
    }
}
