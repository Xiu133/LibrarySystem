using Library.Models;
using Library.ViewModels;

namespace Library.Services.Interfaces
{
    public interface IFineService
    {
        Task<IEnumerable<Fine>> GetAllFinesAsync();
        Task<IEnumerable<Fine>> GetUnpaidFinesAsync();
        Task<IEnumerable<Fine>> GetFinesByUserAsync(string userName);
        Task<ServiceResult> MarkAsPaidAsync(int fineId);
        Task ScanAndCreateFinesAsync();
    }
}
