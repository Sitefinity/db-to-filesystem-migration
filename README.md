# Migration of Sitefinity configurations from Database to FileSystem

Aspx page, which helps to extract Sitefinity configurations which are stored in the Database to the FileSystem. The code runs only on applications which are configured to store configurations in the Database: 

  ```<sitefinityConfig storageMode="Database" />```

Before executing the code, backup of project files and database should be created.



# Steps to execute the migration:

1. Host ConfigMigration.aspx page in your application under ~/Sitefinity and integrate it in your project.

2. Navigate to ~/Sitefinity/ConfigMigration.aspx page (you should be logged with Sitefinity administrator user) 

3. Click on Migrate button

  - automatic back up of your current configurations stored in ~\App_Data\Sitefinity\Configuration will be created in the Configuration folder with format "_config_migration_backup"+"Time stamp"

  - all existing Sitefinity configurations are extracted and saved in ~\App_Data\Sitefinity\Configuration

4. Update in web.config 

  ```<sitefinityConfig storageMode="Database" />``` to ```<sitefinityConfig storageMode="FileSystem" />```

5. Recycle the application pool of the website.

6. Make sure your site is working correctly.



After confirming all is OK.

1. Delete all rows from [sf_xml_config_items]

2. Clean up the backups created in ~\App_Data\Sitefinity\Configuration
