using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Storage;

namespace mongo_leaf_validator_example.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IContactsRepository contacts;

        public ContactController(IContactsRepository contacts)
        {
            this.contacts = contacts;
        }

        [HttpGet]
        public async Task<object> GetContact()
        {
            return await contacts.GetContact();
        }

        [HttpPost]
        public async Task UpdateContact([FromBody] List<DiffRequest> diffs)
        {
            await contacts.UpdateContact(diffs);
        }
    }
}
