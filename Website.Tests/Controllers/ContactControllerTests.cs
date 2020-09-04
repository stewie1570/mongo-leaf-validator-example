using System.Threading.Tasks;
using FluentAssertions;
using mongo_leaf_validator_example.Controllers;
using NSubstitute;
using Storage;
using Xunit;

namespace Website.Tests
{
    public class ContactControllerTests
    {
        [Fact]
        public async Task GetContacts_PassesThrough()
        {
            var contactsRepository = Substitute.For<IContactsRepository>();
            var controller = new ContactController(contactsRepository);
            object expectedContact = new { };
            contactsRepository.GetContact().Returns(expectedContact);

            (await controller.GetContact()).Should().Be(expectedContact);
        }
    }
}
