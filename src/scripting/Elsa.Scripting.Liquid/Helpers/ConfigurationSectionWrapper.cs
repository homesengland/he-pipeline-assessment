using Microsoft.Extensions.Configuration;

namespace Elsa.Scripting.Liquid.Helpers
{
    public class ConfigurationSectionWrapper
    {
        private readonly IConfigurationSection _section;

        public ConfigurationSectionWrapper(IConfigurationSection section)
        {
            _section = section;
        }

        public override string ToString() => _section.Value ?? string.Empty;

        public ConfigurationSectionWrapper GetSection(string name) => new(_section.GetSection(name));
    }
}