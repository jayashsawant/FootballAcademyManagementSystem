namespace FAMS.Application.Common.Interfaces;

public interface IBusinessIdGenerator
{
    Task<string> GenerateAsync(string prefix, CancellationToken cancellationToken = default);
}