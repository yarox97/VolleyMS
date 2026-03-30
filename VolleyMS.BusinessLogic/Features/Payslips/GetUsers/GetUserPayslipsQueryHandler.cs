using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VolleyMS.BusinessLogic.Contracts.DTOs;
using VolleyMS.Core.Enums;
using VolleyMS.Core.Errors;
using VolleyMS.Core.Models.Payslips;
using VolleyMS.Core.Repositories;
using VolleyMS.Core.Services;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.Payslips.GetUsers
{
    public sealed class GetUserPayslipsQueryHandler : IRequestHandler<GetUserPayslipsQuery, Result<PaginatedResult<PayslipDto>>>
    {
        private readonly IPayslipRepository _payslipRepository;
        private readonly IUserRepository _userRepository;

        public GetUserPayslipsQueryHandler(IPayslipRepository payslipRepository, IUserRepository userRepository)
        {
            _payslipRepository = payslipRepository;
            _userRepository = userRepository;
        }

        public async Task<Result<PaginatedResult<PayslipDto>>> Handle(GetUserPayslipsQuery request, CancellationToken cancellationToken)
        {
            var requestor = await _userRepository.GetByIdWithClubsAsync(request.UserId);
            if (requestor == null)
                return Result.Failure<PaginatedResult<PayslipDto>>(DomainErrors.User.UserNotFound);

            PaginatedResult<Payslip> paginatedPayslips;

            // СЦЕНАРИЙ 1: Запрошены расчетные листы по конкретному контракту
            if (request.ContactId.HasValue && request.ContactId.Value != Guid.Empty)
            {
                paginatedPayslips = await _payslipRepository.GetByContractIdWithDetailsAsync(
                    request.ContactId.Value, request.page, request.pageSize, cancellationToken);

                var payslip = paginatedPayslips.Items.FirstOrDefault();

                if (payslip == null)
                {
                    return Result.Success(new PaginatedResult<PayslipDto>(new List<PayslipDto>(), 0, request.page, request.pageSize));
                }

                // Проверка прав (только если смотрим конкретный контракт)
                bool isOwner = requestor.Id == payslip.Contract.UserId;
                bool hasManagerPermission = false;

                var userRoleResult = requestor.GetRoleInClub(payslip.Contract.ClubId);
                if (userRoleResult.IsSuccess && userRoleResult.Value != null)
                {
                    hasManagerPermission = ClubRoleConfig.HasPermission(userRoleResult.Value, ClubPermission.ViewContracts);
                }

                if (!isOwner && !hasManagerPermission)
                {
                    return Result.Failure<PaginatedResult<PayslipDto>>(DomainErrors.Club.InvalidPermission);
                }
            }
            // СЦЕНАРИЙ 2: Запрошены ВСЕ расчетные листы текущего пользователя (например, для графика)
            else
            {
                // Используем метод репозитория для поиска всех пейслипов по ID юзера
                // (Убедитесь, что сигнатура метода совпадает с вашей. Если ContactId там опционален, передайте null)
                paginatedPayslips = await _payslipRepository.GetByUserIdWithDetailsAsync(
                    request.UserId, request.page, request.pageSize, null, cancellationToken);
            }

            // МАППИНГ в DTO (общий для обоих сценариев)
            var dtoList = paginatedPayslips.Items.Select(p => new PayslipDto(
                Id: p.Id,
                Month: p.Month,
                Year: p.Year,
                BaseSalary: p.BaseSalary,
                TotalAmount: p.TotalAmount,
                Status: p.Status.ToString(),
                ClubName: p.Contract?.Club?.Name ?? "Unknown Club"
            )).ToList();

            var paginatedResultDto = new PaginatedResult<PayslipDto>(
                dtoList,
                paginatedPayslips.TotalCount,
                paginatedPayslips.PageNumber,
                paginatedPayslips.PageSize
            );

            return Result.Success(paginatedResultDto);
        }
    }
}