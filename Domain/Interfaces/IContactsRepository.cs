using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IContactsRepository
    {
        Task<object> GetContact();
        Task UpdateContact(List<DiffRequest> diffs);
    }
}