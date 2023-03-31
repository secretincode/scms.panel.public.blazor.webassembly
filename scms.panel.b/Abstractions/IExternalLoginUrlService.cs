using scms.panel.b.Enums;

namespace scms.panel.b.Abstractions;

public interface IExternalLoginUrlService
{
    string? CreateAndGetUrl(ExternalLoginType type);
}
