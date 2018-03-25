using System.IO;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace Kuaiyipai.Localization
{
    public static class KuaiyipaiLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    KuaiyipaiConsts.LocalizationSourceName,
                    new XmlFileLocalizationDictionaryProvider(
                        Path.Combine(Path.GetDirectoryName(typeof(KuaiyipaiCoreModule).GetAssembly().Location), "Localization", "Kuaiyipai")
                    )
                )
            );
        }
    }
}