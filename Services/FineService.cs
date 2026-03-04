using Library.Models;
using Library.Repositories.Interfaces;
using Library.Services.Interfaces;
using Library.ViewModels;

namespace Library.Services
{
    public class FineService : IFineService
    {
        private readonly IFineRepository _fineRepo;
        private readonly IBorrowRepository _borrowRepo;

        public FineService(IFineRepository fineRepo, IBorrowRepository borrowRepo)
        {
            _fineRepo = fineRepo;
            _borrowRepo = borrowRepo;
        }

        public async Task<IEnumerable<Fine>> GetAllFinesAsync() => await _fineRepo.GetAllAsync();

        public async Task<IEnumerable<Fine>> GetUnpaidFinesAsync() => await _fineRepo.GetUnpaidAsync();

        public async Task<IEnumerable<Fine>> GetFinesByUserAsync(string userName) =>
            await _fineRepo.GetByUserAsync(userName);

        public async Task<ServiceResult> MarkAsPaidAsync(int fineId)
        {
            var fine = (await _fineRepo.GetAllAsync()).FirstOrDefault(f => f.Id == fineId);
            if (fine == null) return ServiceResult.Fail("罰款記錄不存在。");
            fine.IsPaid = true;
            fine.PaidDate = DateTime.Now;
            await _fineRepo.UpdateAsync(fine);
            return ServiceResult.Ok();
        }

        public async Task ScanAndCreateFinesAsync()
        {
            var activeRecords = await _borrowRepo.GetAllOverdueAsync();

            foreach (var record in activeRecords)
            {
                var existing = await _fineRepo.GetByBorrowRecordAsync(record.Id);
                if (existing != null) continue;

                if (record.DueDate == null) continue;
                var overdueDays = (int)(DateTime.Now - record.DueDate.Value).TotalDays;
                if (overdueDays <= 0) continue;

                var fine = new Fine
                {
                    BorrowRecordId = record.Id,
                    UserName = record.UserName,
                    Amount = overdueDays * 5m,
                    CreatedAt = DateTime.Now
                };
                await _fineRepo.AddAsync(fine);
            }
        }
    }
}
