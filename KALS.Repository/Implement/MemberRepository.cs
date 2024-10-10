using KALS.Domain.DataAccess;
using KALS.Domain.Entities;
using KALS.Domain.Filter;
using KALS.Domain.Paginate;
using KALS.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace KALS.Repository.Implement;

public class MemberRepository: GenericRepository<Member>, IMemberRepository
{
    public MemberRepository(KitAndLabDbContext context) : base(context)
    {
    }

    public Task<Member> GetMemberByUserId(Guid userId)
    {
        var member = SingleOrDefaultAsync(
            predicate: m => m.UserId == userId,
            include: m => m.Include(m => m.User)
                .Include(m => m.LabMembers)
                .ThenInclude(lm => lm.Lab)
        );
        return member;
    }

    public async Task<IPaginate<Member>> GetMembersPagingAsync(int page, int size, IFilter<Member> filter,
        string? sortBy, bool isAsc)
    {
        var members = await GetPagingListAsync(
            selector: m => new Member()
            {
                Id = m.Id,
                UserId = m.UserId,
                User = m.User,
                Address = m.Address,
                Ward = m.Ward,
                District = m.District,
                Province = m.Province,
                ProvinceCode = m.ProvinceCode,
                WardCode = m.WardCode,
                DistrictCode = m.DistrictCode,
                LabMembers = m.LabMembers
            },
            page: page,
            size: size,
            filter: filter,
            include: m => m.Include(m => m.User),
            sortBy: sortBy,
            isAsc: isAsc
        );
        return members;
    }
}