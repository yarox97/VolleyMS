
using MediatR;
using Microsoft.AspNetCore.Mvc;
using VolleyMS.Core.Shared;

namespace VolleyMS.BusinessLogic.Features.ClubMembers.ChangeRole
{
    public sealed record ChangeRoleCommand(Guid clubId, Guid requestorId, ClubMemberRole newRole, Guid responserId) : IRequest<Result>
    {
    }
}
