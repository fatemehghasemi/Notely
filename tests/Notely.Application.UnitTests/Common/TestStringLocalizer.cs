using Microsoft.Extensions.Localization;

namespace Notely.Application.UnitTests.Common;

internal sealed class TestStringLocalizer<T> : IStringLocalizer<T>
{
    public LocalizedString this[string name] => new(name, name, resourceNotFound: false);

    public LocalizedString this[string name, params object[] arguments]
        => new(name, string.Format(name, arguments), resourceNotFound: false);

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) => [];

    public IStringLocalizer WithCulture(System.Globalization.CultureInfo culture) => this;
}
