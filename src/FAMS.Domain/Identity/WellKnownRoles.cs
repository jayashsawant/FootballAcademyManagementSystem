namespace FAMS.Domain.Identity;

public static class WellKnownRoles
{
    public static readonly Guid AdminId = new("11111111-1111-1111-1111-111111111111");
    public static readonly Guid BranchManagerId = new("22222222-2222-2222-2222-222222222222");
    public static readonly Guid CoachId = new("33333333-3333-3333-3333-333333333333");
    public static readonly Guid StaffId = new("44444444-4444-4444-4444-444444444444");
    public static readonly Guid FinanceOfficerId = new("55555555-5555-5555-5555-555555555555");
    public static readonly Guid ReceptionistId = new("66666666-6666-6666-6666-666666666666");
    public static readonly Guid ParentId = new("77777777-7777-7777-7777-777777777777");
    public static readonly Guid ContentEditorId = new("88888888-8888-8888-8888-888888888888");

    public const string Admin = "Admin";
    public const string BranchManager = "BranchManager";
    public const string Coach = "Coach";
    public const string Staff = "Staff";
    public const string FinanceOfficer = "FinanceOfficer";
    public const string Receptionist = "Receptionist";
    public const string Parent = "Parent";
    public const string ContentEditor = "ContentEditor";
}