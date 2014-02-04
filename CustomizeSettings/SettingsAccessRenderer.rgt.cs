using System.Diagnostics;

namespace CustomizeSettings
{
    using System;

    public partial class SettingsAccessRenderer
    {
        public const string ReadOnlySuffix = "_ReadOnly";
        /// <summary>
        /// The settings file element as deserialized from the settings file.
        /// </summary>
        private SettingsFile _settingsFile = null;

        public override void PreRender()
        {
            base.PreRender();
            this._settingsFile = SettingsSchemaFactory.Create<SettingsFile>(base.ProjectItem);
       
        }

        /// <summary>
        /// Render all the settings defined in the settings file.
        /// </summary>
        private void RenderSettings()
        {
            foreach (SettingsFileSetting setting in this._settingsFile.Settings)
            {
                switch (setting.Type)
                {
                    case "(Connection string)":
                        RenderConnectionString(setting);
                        break;

                    case "(Web Service URL)":
                        RenderWebServiceUrl(setting);
                        break;

                    default:
                   
                        RenderSetting(setting);
                        break;
                }
            }
        }

        /// <summary>
        /// Transforms the default value of a setting into a string that can be used as an
        /// attribute value in the rendered code.
        /// </summary>
        private string GetDefaultSetting(SettingsFileSetting setting)
        {
            if (setting.Value == null || string.IsNullOrEmpty(setting.Value.Value))
                return "\"\""; // value not specified, just return an empty string.

            // massage the string so that it can be as an attribute value
            // (the value sometimes does not contain \r\n but only \r or only \n.
            string value = setting.Value.Value;
            if (!value.Contains("\n") && !value.Contains("\r"))
                return string.Concat("\"", value.Replace(@"\", @"\\").Replace("\"", "\\\""), "\"");

            value = value.Replace(Environment.NewLine, "\n");
            value = value.Replace("\r", "\n");
            value = value.Replace("\n", Environment.NewLine);
            return string.Concat("@\"", value.Replace("\"", "\"\""), "\"");
        }
    }
}