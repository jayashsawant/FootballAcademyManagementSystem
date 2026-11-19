using FAMS.Application.Common.Interfaces;
using FAMS.Application.Identity.Common.Interfaces;
using FAMS.Domain.Academy;
using FAMS.Domain.Billing;
using FAMS.Domain.Coaches;
using FAMS.Domain.Identity;
using FAMS.Domain.People;
using FAMS.Domain.Training;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FAMS.Infrastructure.Persistence;

public static class DataSeeder
{
    public static async Task SeedAsync(
        IServiceProvider services,
        CancellationToken cancellationToken = default)
    {
        var db = services.GetRequiredService<ApplicationDbContext>();

        var adminExists = await db.Users.AnyAsync(
            u => u.PhoneNumber == "1111111111",
            cancellationToken);

        var academyExists = await db.Academies.AnyAsync(
            cancellationToken);

        if (adminExists && academyExists)
        {
            return;
        }

        var passwordHasher =
            services.GetRequiredService<IPasswordHasher>();

        var numbers =
            services.GetRequiredService<IBusinessIdGenerator>();

        if (academyExists)
        {
            var coach = await db.Coaches
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            await SeedStaffUsersAsync(
                db,
                passwordHasher,
                coach?.Id,
                cancellationToken);

            return;
        }

        // =============================================================
        // ROLES
        // =============================================================

        var adminRole = await db.Roles
            .SingleAsync(
                r => r.Name == "Admin",
                cancellationToken);

        var branchManagerRole = await db.Roles
            .SingleAsync(
                r => r.Name == "BranchManager",
                cancellationToken);

        var coachRole = await db.Roles
            .SingleAsync(
                r => r.Name == "Coach",
                cancellationToken);

        var receptionistRole = await db.Roles
            .SingleAsync(
                r => r.Name == "Receptionist",
                cancellationToken);

        var financeRole = await db.Roles
            .SingleAsync(
                r => r.Name == "FinanceOfficer",
                cancellationToken);
        
        var contentEditorRole = await db.Roles
            .SingleAsync(
                r => r.Name == "ContentEditor",
                cancellationToken);

        // =============================================================
        // ACADEMY
        // =============================================================

        var academy = Academy.Create(
            "Mumbai Strikers Football Academy",
            "Premier youth football development academy in Mumbai.",
            "hello@mumbaistrikers.in",
            "+91 22 4890 1200",
            "Sports Complex, MULUND West, Mumbai",
            standardMonthlyFee: 3500);

        db.Academies.Add(academy);

        await db.SaveChangesAsync(cancellationToken);

        // =============================================================
        // BRANCHES
        // =============================================================

        var andheri = Branch.Create(
            academy.Id,
            "Andheri West HQ",
            "4th Floor, Sports Complex, Veera Desai Road",
            "Mumbai",
            "+91 98765 10001",
            "andheri@mumbaistrikers.in");

        var dadar = Branch.Create(
            academy.Id,
            "Dadar Branch",
            "Shivaji Park, Dadar West",
            "Mumbai",
            "+91 98765 10002",
            "dadar@mumbaistrikers.in");

        var powai = Branch.Create(
            academy.Id,
            "Powai Branch",
            "Hiranandani Gardens, Powai",
            "Mumbai",
            "+91 98765 10003",
            "powai@mumbaistrikers.in");

        db.Branches.AddRange(
            andheri,
            dadar,
            powai);

        await db.SaveChangesAsync(cancellationToken);

        // =============================================================
        // GROUNDS
        // =============================================================

        var andheriMainPitch = Ground.Create(
            andheri.Id,
            "Main Pitch (11v11)",
            "Full-size artificial turf");

        var andheriAstro = Ground.Create(
            andheri.Id,
            "Astro 5v5",
            "Small-sided training area");

        var dadarTurf = Ground.Create(
            dadar.Id,
            "Shivaji Park Turf",
            "Grass pitch");

        var dadarMini = Ground.Create(
            dadar.Id,
            "Mini 7v7",
            "Half-size turf with floodlights");

        var powaiArena = Ground.Create(
            powai.Id,
            "Powai Arena",
            "Futsal court + turf");

        var powaiTraining = Ground.Create(
            powai.Id,
            "Training Ground",
            "Drills & fitness area");

        db.Grounds.AddRange(
            andheriMainPitch,
            andheriAstro,
            dadarTurf,
            dadarMini,
            powaiArena,
            powaiTraining);

        await db.SaveChangesAsync(cancellationToken);

        // =============================================================
        // SEASONS
        // =============================================================

        var previousSeason = Season.Create(
            academy.Id,
            "2025-26 Season",
            new DateTime(2025, 6, 1),
            new DateTime(2026, 5, 31));

        var currentSeason = Season.Create(
            academy.Id,
            "2026-27 Season",
            new DateTime(2026, 6, 1),
            new DateTime(2027, 5, 31));

        currentSeason.MarkAsCurrent();

        db.Seasons.AddRange(
            previousSeason,
            currentSeason);

        await db.SaveChangesAsync(cancellationToken);

        // =============================================================
        // AGE GROUPS
        // =============================================================

        var under8 = AgeGroup.Create(
            academy.Id,
            "Under 8",
            minimumAge: 5,
            maximumAge: 8,
            sortOrder: 1);

        var under10 = AgeGroup.Create(
            academy.Id,
            "Under 10",
            minimumAge: 9,
            maximumAge: 10,
            sortOrder: 2);

        var under12 = AgeGroup.Create(
            academy.Id,
            "Under 12",
            minimumAge: 11,
            maximumAge: 12,
            sortOrder: 3);

        var under14 = AgeGroup.Create(
            academy.Id,
            "Under 14",
            minimumAge: 13,
            maximumAge: 14,
            sortOrder: 4);

        db.AgeGroups.AddRange(
            under8,
            under10,
            under12,
            under14);

        await db.SaveChangesAsync(cancellationToken);

        // =============================================================
        // BATCHES
        // =============================================================

        var andheriUnder8Batch = Batch.Create(
            academy.Id,
            andheri.Id,
            currentSeason.Id,
            under8.Id,
            "Under 8 - Evening Batch");

        var andheriUnder10Batch = Batch.Create(
            academy.Id,
            andheri.Id,
            currentSeason.Id,
            under10.Id,
            "Under 10 - Evening Batch");

        var andheriUnder12Batch = Batch.Create(
            academy.Id,
            andheri.Id,
            currentSeason.Id,
            under12.Id,
            "Under 12 - Evening Batch");

        var andheriUnder14Batch = Batch.Create(
            academy.Id,
            andheri.Id,
            currentSeason.Id,
            under14.Id,
            "Under 14 - Evening Batch");

        var dadarUnder8Batch = Batch.Create(
            academy.Id,
            dadar.Id,
            currentSeason.Id,
            under8.Id,
            "Under 8 - Evening Batch");

        var dadarUnder10Batch = Batch.Create(
            academy.Id,
            dadar.Id,
            currentSeason.Id,
            under10.Id,
            "Under 10 - Evening Batch");

        var dadarUnder12Batch = Batch.Create(
            academy.Id,
            dadar.Id,
            currentSeason.Id,
            under12.Id,
            "Under 12 - Evening Batch");

        var dadarUnder14Batch = Batch.Create(
            academy.Id,
            dadar.Id,
            currentSeason.Id,
            under14.Id,
            "Under 14 - Evening Batch");

        var powaiUnder8Batch = Batch.Create(
            academy.Id,
            powai.Id,
            currentSeason.Id,
            under8.Id,
            "Under 8 - Evening Batch");

        var powaiUnder10Batch = Batch.Create(
            academy.Id,
            powai.Id,
            currentSeason.Id,
            under10.Id,
            "Under 10 - Evening Batch");

        var powaiUnder12Batch = Batch.Create(
            academy.Id,
            powai.Id,
            currentSeason.Id,
            under12.Id,
            "Under 12 - Evening Batch");

        var powaiUnder14Batch = Batch.Create(
            academy.Id,
            powai.Id,
            currentSeason.Id,
            under14.Id,
            "Under 14 - Evening Batch");

        db.Batches.AddRange(
            andheriUnder8Batch,
            andheriUnder10Batch,
            andheriUnder12Batch,
            andheriUnder14Batch,
            dadarUnder8Batch,
            dadarUnder10Batch,
            dadarUnder12Batch,
            dadarUnder14Batch,
            powaiUnder8Batch,
            powaiUnder10Batch,
            powaiUnder12Batch,
            powaiUnder14Batch);

        await db.SaveChangesAsync(cancellationToken);

        // =============================================================
        // COACHES
        // =============================================================

        var coaches = new List<Coach>
        {
            Coach.Create(
                await numbers.GenerateAsync(
                    "COA",
                    cancellationToken),
                andheri.Id,
                "Rohit",
                "Dsouza",
                "+91 11111 11111",
                EmploymentType.FullTime,
                "rohit@mumbaistrikers.in",
                "B.P.Ed, NIS Coaching Diploma",
                "AFC B License",
                8),

            Coach.Create(
                await numbers.GenerateAsync(
                    "COA",
                    cancellationToken),
                andheri.Id,
                "Rohit",
                "Kulkarni",
                "+91 98200 11112",
                EmploymentType.FullTime,
                "rohit.kulkarni@mumbaistrikers.in",
                "AFC C Certificate",
                "AFC C License",
                5),

            Coach.Create(
                await numbers.GenerateAsync(
                    "COA",
                    cancellationToken),
                dadar.Id,
                "Daniel",
                "Fernandes",
                "+91 98200 11113",
                EmploymentType.FullTime,
                "daniel.fernandes@mumbaistrikers.in",
                "UEFA C, AFC B",
                "AFC B License",
                10),

            Coach.Create(
                await numbers.GenerateAsync(
                    "COA",
                    cancellationToken),
                dadar.Id,
                "Imran",
                "Shaikh",
                "+91 98200 11114",
                EmploymentType.PartTime,
                "imran.shaikh@mumbaistrikers.in",
                "AIFF D License",
                "AIFF D License",
                3),

            Coach.Create(
                await numbers.GenerateAsync(
                    "COA",
                    cancellationToken),
                powai.Id,
                "Suresh",
                "Patil",
                "+91 98200 11115",
                EmploymentType.Contract,
                "suresh.patil@mumbaistrikers.in",
                "AFC C Certificate",
                "AFC C License",
                4),

            Coach.Create(
                await numbers.GenerateAsync(
                    "COA",
                    cancellationToken),
                powai.Id,
                "Aditya",
                "Joshi",
                "+91 98200 11116",
                EmploymentType.FullTime,
                "aditya.joshi@mumbaistrikers.in",
                "Former ISL youth coach",
                "AFC A License",
                12)
        };

        db.Coaches.AddRange(coaches);

        await db.SaveChangesAsync(cancellationToken);

        // =============================================================
        // STAFF USERS
        // =============================================================

        await SeedStaffUsersAsync(
            db,
            passwordHasher,
            coaches[0].Id,
            cancellationToken);

        // =============================================================
        // PARENTS
        // =============================================================

        var parents = new List<Parent>
        {
            Parent.Create(
                "Ramesh",
                "Iyer",
                "+91 98200 20001",
                "ramesh.iyer@gmail.com"),

            Parent.Create(
                "Kavita",
                "Menon",
                "+91 98200 20002",
                "kavita.menon@gmail.com"),

            Parent.Create(
                "Sunil",
                "Gupta",
                "+91 98200 20003",
                "sunil.gupta@gmail.com"),

            Parent.Create(
                "Anita",
                "Rao",
                "+91 98200 20004",
                "anita.rao@gmail.com"),

            Parent.Create(
                "Farhan",
                "Qureshi",
                "+91 98200 20005",
                "farhan.q@gmail.com"),

            Parent.Create(
                "Deepa",
                "Sharma",
                "+91 98200 20006",
                "deepa.sharma@gmail.com"),

            Parent.Create(
                "Mahesh",
                "Reddy",
                "+91 98200 20007",
                "mahesh.reddy@gmail.com"),

            Parent.Create(
                "Pooja",
                "Kulkarni",
                "+91 98200 20008",
                "pooja.k@gmail.com")
        };

        db.Parents.AddRange(parents);

        await db.SaveChangesAsync(cancellationToken);

        // =============================================================
        // PLAYERS
        // =============================================================

        var playerRows =
            new (
                Parent Parent,
                Branch Branch,
                AgeGroup AgeGroup,
                Batch Batch,
                string First,
                string Last,
                DateTime Dob,
                PlayerPosition Position
            )[]
        {
            (
                parents[0],
                andheri,
                under12,
                andheriUnder12Batch,
                "Aryan",
                "Iyer",
                new DateTime(2014, 5, 10),
                PlayerPosition.Forward
            ),

            (
                parents[0],
                andheri,
                under10,
                andheriUnder10Batch,
                "Kabir",
                "Iyer",
                new DateTime(2016, 3, 2),
                PlayerPosition.Midfielder
            ),

            (
                parents[1],
                andheri,
                under14,
                andheriUnder14Batch,
                "Nisha",
                "Menon",
                new DateTime(2012, 8, 21),
                PlayerPosition.Defender
            ),

            (
                parents[2],
                dadar,
                under12,
                dadarUnder12Batch,
                "Arnav",
                "Gupta",
                new DateTime(2014, 1, 15),
                PlayerPosition.Goalkeeper
            ),

            (
                parents[3],
                dadar,
                under10,
                dadarUnder10Batch,
                "Rohan",
                "Rao",
                new DateTime(2016, 11, 4),
                PlayerPosition.Forward
            ),

            (
                parents[4],
                dadar,
                under8,
                dadarUnder8Batch,
                "Zara",
                "Qureshi",
                new DateTime(2018, 6, 30),
                PlayerPosition.Midfielder
            ),

            (
                parents[5],
                powai,
                under14,
                powaiUnder14Batch,
                "Vihaan",
                "Sharma",
                new DateTime(2012, 2, 12),
                PlayerPosition.Forward
            ),

            (
                parents[6],
                powai,
                under12,
                powaiUnder12Batch,
                "Ishaan",
                "Reddy",
                new DateTime(2014, 9, 18),
                PlayerPosition.Defender
            ),

            (
                parents[7],
                powai,
                under10,
                powaiUnder10Batch,
                "Ananya",
                "Kulkarni",
                new DateTime(2016, 4, 25),
                PlayerPosition.Midfielder
            ),

            (
                parents[0],
                andheri,
                under14,
                andheriUnder14Batch,
                "Dev",
                "Patel",
                new DateTime(2012, 12, 8),
                PlayerPosition.Goalkeeper
            ),

            (
                parents[2],
                dadar,
                under8,
                dadarUnder8Batch,
                "Myra",
                "Gupta",
                new DateTime(2018, 1, 22),
                PlayerPosition.Defender
            ),

            (
                parents[5],
                powai,
                under12,
                powaiUnder12Batch,
                "Kabir",
                "Joshi",
                new DateTime(2014, 7, 14),
                PlayerPosition.Midfielder
            )
        };

        var players = new List<Player>();

        foreach (var row in playerRows)
        {
            var player = Player.Create(
                await numbers.GenerateAsync(
                    "PLY",
                    cancellationToken),
                row.Parent.Id,
                row.First,
                row.Last,
                row.Dob,
                row.Position);

            player.AddEnrollment(
                currentSeason.Id,
                row.Branch.Id,
                row.AgeGroup.Id,
                row.Batch.Id);

            players.Add(player);
        }

        db.Players.AddRange(players);

        await db.SaveChangesAsync(cancellationToken);

        // =============================================================
        // TRAINING SESSIONS
        // =============================================================

        var today = DateTime.Today;

        var sessionRows =
            new (
                Branch Branch,
                Ground Ground,
                Coach Coach,
                DateTime Date,
                TimeSpan Start,
                TimeSpan End,
                string Notes
            )[]
        {
            (
                andheri,
                andheriMainPitch,
                coaches[0],
                today,
                new TimeSpan(16, 30, 0),
                new TimeSpan(18, 0, 0),
                "U-12 technical drills and small-sided games"
            ),

            (
                andheri,
                andheriAstro,
                coaches[1],
                today.AddDays(1),
                new TimeSpan(17, 0, 0),
                new TimeSpan(18, 30, 0),
                "U-10 ball mastery session"
            ),

            (
                dadar,
                dadarTurf,
                coaches[2],
                today,
                new TimeSpan(17, 30, 0),
                new TimeSpan(19, 0, 0),
                "U-12 match practice"
            ),

            (
                dadar,
                dadarMini,
                coaches[3],
                today.AddDays(2),
                new TimeSpan(18, 0, 0),
                new TimeSpan(19, 0, 0),
                "U-8 fun games and coordination"
            ),

            (
                powai,
                powaiArena,
                coaches[5],
                today.AddDays(1),
                new TimeSpan(16, 0, 0),
                new TimeSpan(17, 30, 0),
                "U-14 pressing and transitions"
            ),

            (
                powai,
                powaiArena,
                coaches[4],
                today.AddDays(3),
                new TimeSpan(17, 0, 0),
                new TimeSpan(18, 30, 0),
                "U-12 finishing drills"
            ),

            (
                andheri,
                andheriMainPitch,
                coaches[0],
                today.AddDays(3),
                new TimeSpan(16, 30, 0),
                new TimeSpan(18, 0, 0),
                "U-14 tactical block"
            )
        };

        var sessions = new List<TrainingSession>();

        foreach (var row in sessionRows)
        {
            var session = TrainingSession.Create(
                row.Branch.Id,
                row.Ground.Id,
                currentSeason.Id,
                row.Coach.Id,
                row.Date,
                row.Start,
                row.End,
                row.Notes);

            var roster = players
                .Where(p =>
                    p.Enrollments.Any(e =>
                        e.IsCurrent &&
                        e.BranchId == row.Branch.Id &&
                        e.SeasonId == currentSeason.Id))
                .ToList();

            for (var i = 0; i < roster.Count; i++)
            {
                var statuses = new[]
                {
                    AttendanceStatus.Present,
                    AttendanceStatus.Present,
                    AttendanceStatus.Late,
                    AttendanceStatus.Absent,
                    AttendanceStatus.Excused
                };

                session.MarkAttendance(
                    roster[i].Id,
                    statuses[
                        (i * 3 + row.Date.Day)
                        % statuses.Length
                    ]);
            }

            sessions.Add(session);
        }

        db.TrainingSessions.AddRange(sessions);

        await db.SaveChangesAsync(cancellationToken);

        // =============================================================
        // INVOICES
        // =============================================================

        var month = DateTime.Today.Month;
        var year = DateTime.Today.Year;

        var invoices = new List<Invoice>();

        foreach (var player in players)
        {
            var invoice = Invoice.Create(
                await numbers.GenerateAsync(
                    "INV",
                    cancellationToken),
                player.Id,
                currentSeason.Id,
                year,
                month,
                academy.StandardMonthlyFee,
                new DateTime(
                    year,
                    month,
                    Math.Min(
                        10,
                        DateTime.DaysInMonth(year, month))));

            if (player.Id == players[0].Id)
            {
                invoice.RecordPayment(
                    invoice.Amount,
                    PaymentMethod.Online,
                    DateTime.Today.AddDays(-3),
                    "UPI-8A3F2B");
            }
            else if (player.Id == players[3].Id)
            {
                invoice.RecordPayment(
                    invoice.Amount / 2,
                    PaymentMethod.Cash,
                    DateTime.Today.AddDays(-1),
                    null,
                    null,
                    "First installment");
            }

            invoices.Add(invoice);
        }

        db.Invoices.AddRange(invoices);

        await db.SaveChangesAsync(cancellationToken);
    }

    // =============================================================
    // STAFF USERS
    // =============================================================

    private static async Task SeedStaffUsersAsync(
        ApplicationDbContext db,
        IPasswordHasher passwordHasher,
        Guid? coachPersonReferenceId,
        CancellationToken cancellationToken)
    {
        var adminRole = await db.Roles
            .SingleAsync(
                r => r.Name == "Admin",
                cancellationToken);

        var branchManagerRole = await db.Roles
            .SingleAsync(
                r => r.Name == "BranchManager",
                cancellationToken);

        var coachRole = await db.Roles
            .SingleAsync(
                r => r.Name == "Coach",
                cancellationToken);

        var receptionistRole = await db.Roles
            .SingleAsync(
                r => r.Name == "Receptionist",
                cancellationToken);

        var financeRole = await db.Roles
            .SingleAsync(
                r => r.Name == "FinanceOfficer",
                cancellationToken);

        // =========================================================
        // ADMIN
        // =========================================================

        if (!await db.Users.AnyAsync(
                u => u.Email ==
                     "rahul.sharma@mumbaistrikers.in",
                cancellationToken))
        {
            db.Users.Add(
                User.Create(
                    "1111111111",
                    "rahul.sharma@mumbaistrikers.in",
                    passwordHasher.Hash("Admin@123"),
                    "Rahul",
                    "Sharma",
                    adminRole.Id));
        }

        // =========================================================
        // BRANCH MANAGER
        // =========================================================

        if (!await db.Users.AnyAsync(
                u => u.Email ==
                     "priya.nair@mumbaistrikers.in",
                cancellationToken))
        {
            db.Users.Add(
                User.Create(
                    "9876500002",
                    "priya.nair@mumbaistrikers.in",
                    passwordHasher.Hash("Pass@123"),
                    "Priya",
                    "Nair",
                    branchManagerRole.Id));
        }

        // =========================================================
        // COACH
        // =========================================================

        if (!await db.Users.AnyAsync(
                u => u.Email ==
                     "vikram.singh@mumbaistrikers.in",
                cancellationToken))
        {
            db.Users.Add(
                User.Create(
                    "9876500003",
                    "vikram.singh@mumbaistrikers.in",
                    passwordHasher.Hash("Pass@123"),
                    "Vikram",
                    "Singh",
                    coachRole.Id,
                    coachPersonReferenceId));
        }

        // =========================================================
        // RECEPTIONIST
        // =========================================================

        if (!await db.Users.AnyAsync(
                u => u.Email ==
                     "sneha.desai@mumbaistrikers.in",
                cancellationToken))
        {
            db.Users.Add(
                User.Create(
                    "9876500004",
                    "sneha.desai@mumbaistrikers.in",
                    passwordHasher.Hash("Pass@123"),
                    "Sneha",
                    "Desai",
                    receptionistRole.Id));
        }

        // =========================================================
        // FINANCE OFFICER
        // =========================================================

        if (!await db.Users.AnyAsync(
                u => u.Email ==
                     "arjun.mehta@mumbaistrikers.in",
                cancellationToken))
        {
            db.Users.Add(
                User.Create(
                    "9876500005",
                    "arjun.mehta@mumbaistrikers.in",
                    passwordHasher.Hash("Pass@123"),
                    "Arjun",
                    "Mehta",
                    financeRole.Id));
        }

        await db.SaveChangesAsync(cancellationToken);
    }
}