using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Configuration.Data;
using Telerik.Sitefinity.Data;

namespace SitefinityWebApp
{
    public partial class ConfigMigration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void MigrateButton_Click(object sender, EventArgs e)
        {
            var storageMode = this.GetConfigStorageMode();
            if (storageMode == ConfigStorageMode.Database)
            {
                this.MigrateAllConfigurations();
            }             
        }

        public void MigrateAllConfigurations()
        {
            //Backup
            string response = String.Empty;
            string configurationPath = HttpContext.Current.Server.MapPath("~/App_Data/Sitefinity/Configuration/");
            string backupPath = configurationPath + "_config_migration_backup_" + DateTime.UtcNow.ToLongTimeString().Replace(":", "_").Replace(" ", "_") + "/";
            Directory.CreateDirectory(backupPath);           

            string[] configFilesList = System.IO.Directory.GetFiles(configurationPath);

            foreach (string configFile in configFilesList)
            {
                string fileName = Path.GetFileName(configFile);
                string copyTo = backupPath + fileName;

                File.Copy(configFile, copyTo);
            }

            //Migration
            var manager = ConfigManager.GetManager();
            foreach (var tagName in this.GetConfigurations())
            {
                var section = manager.GetSection(tagName);

                var parentType = typeof(XmlConfigProvider);
                var migrationStorageModeRegionType = parentType.GetNestedType("MigrationStorageModeRegion", BindingFlags.NonPublic);

                var provider = this.GetInstanceField(typeof(ConfigElement), section, "provider");

                using ((IDisposable)Activator.CreateInstance(migrationStorageModeRegionType, provider, ConfigStorageMode.FileSystem))
                using (new ElevatedModeRegion(manager))
                {
                    manager.SaveSection(section);
                }
            }
        }

        public IEnumerable<string> GetConfigurations()
        {
            var manager = ConfigManager.GetManager();

            return manager
                .GetAllConfigSections()
                .Select(c => c.TagName);
        }


        internal object GetInstanceField(Type type, object instance, string fieldName)
        {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                | BindingFlags.Static;
            FieldInfo field = type.GetField(fieldName, bindFlags);
            return field.GetValue(instance);
        }

        private ConfigStorageMode GetConfigStorageMode()
        {
            var storageMode = ConfigStorageMode.FileSystem;
            if (Config.SectionHandler.Settings.StorageMode != null)
            {
                storageMode = Config.SectionHandler.Settings.StorageMode;
            }

            return storageMode;
        }
    }
}