using Grease.Identifiers;

namespace Swivel.Formatters
{
    public interface IFormatter
    {
        IFormatter Format(Assessment assessment);
    }
}