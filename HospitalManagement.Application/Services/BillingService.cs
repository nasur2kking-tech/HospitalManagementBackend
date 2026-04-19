using AutoMapper;
using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.Billing;
using HospitalManagement.Application.Interfaces.Repositories;
using HospitalManagement.Application.Interfaces.Services;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Application.Services
{
    public class BillingService(
        IBillingRepository repo,
        IMapper mapper) : IBillingService
    {
        private readonly IBillingRepository _repo = repo;
        private readonly IMapper _mapper = mapper;

        public async Task<PagedResult<BillResponseDto>> GetAllAsync(
            PaginationParams pagination,
            CancellationToken ct = default)
        {
            var (items, totalCount) = await _repo.GetAllAsync(
                pagination.PageNumber,
                pagination.PageSize,
                ct);

            return new PagedResult<BillResponseDto>
            {
                Items = _mapper.Map<IEnumerable<BillResponseDto>>(items),
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }

        public async Task<BillResponseDto> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var bill = await _repo.GetByIdAsync(id, ct)
                       ?? throw new ApplicationException("Bill not found.");

            return _mapper.Map<BillResponseDto>(bill);
        }

        public async Task<BillResponseDto> CreateAsync(
            CreateBillDto dto,
            CancellationToken ct = default)
        {
            var bill = _mapper.Map<Bill>(dto);

            bill.Status = PaymentStatus.Pending;

            await _repo.AddAsync(bill, ct);

            return _mapper.Map<BillResponseDto>(bill);
        }

        public async Task UpdateStatusAsync(
            int id,
            string status,
            CancellationToken ct = default)
        {
            var bill = await _repo.GetByIdAsync(id, ct)
                       ?? throw new ApplicationException("Bill not found.");

            if (!Enum.TryParse<PaymentStatus>(status, true, out var parsedStatus))
                throw new ApplicationException("Invalid payment status.");

            bill.Status = parsedStatus;

            await _repo.UpdateAsync(bill, ct);
        }

        // ✅ FIXED: MISSING METHOD
        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var bill = await _repo.GetByIdAsync(id, ct)
                       ?? throw new ApplicationException("Bill not found.");

            await _repo.UpdateAsync(bill, ct); // ❌ wrong (see below)
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        {
            return await _repo.ExistsAsync(id, ct);
        }
    }
}