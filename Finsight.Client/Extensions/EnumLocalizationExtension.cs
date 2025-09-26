using System.Windows.Markup;
using Finsight.Client.Utils;

namespace Finsight.Client.Extensions
{
    internal class EnumLocalizationExtension : MarkupExtension
    {
        public Type EnumType { get; }

        public EnumLocalizationExtension(Type enumType)
        {
            EnumType = enumType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (!EnumType.IsEnum)
            {
                throw new ArgumentException("EnumType должен быть перечислением.");
            }

            var values = Enum.GetValues(EnumType)
                .Cast<Enum>()
                .Select(value => new
                {
                    Value = value,
                    Display = EnumLocalizationProvider.GetLocalizedString(value)
                })
                .ToList();

            return values;
        }
    }
}
