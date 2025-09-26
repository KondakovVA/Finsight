using System;

namespace Finsight.Client.Utils
{
    public static class EnumLocalizationProvider
    {
        public static string GetLocalizedString(Enum enumValue)
        {
            var typeName = enumValue.GetType().Name;
            var valueName = enumValue.ToString();
            var key = $"{typeName}_{valueName}";

            return Properties.Localization.ResourceManager.GetString(key)
                   ?? valueName.Replace("_", " ");
        }
    }
}
