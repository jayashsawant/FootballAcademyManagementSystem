using FAMS.Application.Common.Interfaces;
using FAMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FAMS.Infrastructure.Common;

public class BusinessIdGenerator : IBusinessIdGenerator
{
    private readonly ApplicationDbContext _context;

    public BusinessIdGenerator(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateAsync(string prefix, CancellationToken cancellationToken = default)
    {
        var year = DateTime.UtcNow.Year;

        var sql = @"
            INSERT INTO ""NumberSequences"" (""Prefix"", ""Year"", ""LastNumber"")
            VALUES ({0}, {1}, 1)
            ON CONFLICT (""Prefix"", ""Year"")
            DO UPDATE SET ""LastNumber"" = ""NumberSequences"".""LastNumber"" + 1
            RETURNING ""LastNumber"";";

        var results = await _context.Database
            .SqlQueryRaw<int>(sql, prefix, year)
            .ToListAsync(cancellationToken);

        var nextNumber = results.First();

        return $"{prefix}-{year}-{nextNumber:D6}";
    }
}


//using FAMS.Application.Common.Interfaces;
//using FAMS.Infrastructure.Persistence;
//using Microsoft.EntityFrameworkCore;

//namespace FAMS.Infrastructure.Common;

//public class BusinessIdGenerator : IBusinessIdGenerator
//{
//    private readonly ApplicationDbContext _context;

//    public BusinessIdGenerator(ApplicationDbContext context)
//    {
//        _context = context;
//    }

//    public async Task<string> GenerateAsync(string prefix, CancellationToken cancellationToken = default)
//    {
//        var year = DateTime.UtcNow.Year;

//        var sql = @"
//            MERGE INTO NumberSequences AS target
//            USING (SELECT {0} AS Prefix, {1} AS Year) AS source
//            ON target.Prefix = source.Prefix AND target.Year = source.Year
//            WHEN MATCHED THEN
//                UPDATE SET LastNumber = target.LastNumber + 1
//            WHEN NOT MATCHED THEN
//                INSERT (Prefix, Year, LastNumber) VALUES (source.Prefix, source.Year, 1)
//            OUTPUT inserted.LastNumber;";

//        var results = await _context.Database
//    .SqlQueryRaw<int>(sql, prefix, year)
//    .ToListAsync(cancellationToken);

//        var nextNumber = results.First();

//        return $"{prefix}-{year}-{nextNumber:D6}";
//    }
//}