# Migrate Sitefinity CMS configurations from the Database to the FileSystem

You migrate Sitefinity CMS configurations from the database to the file system, using an ASPX page, which helps to extract the configurations. The code runs only on applications, which are configured to store configurations in the database:

  ```<sitefinityConfig storageMode="Database" />```

**IMPORTANT**: Before executing the code, you must backup project files and database.

## Procedure:

1. Host ConfigMigration.aspx page in your application under ~/Sitefinity and integrate it in your project.

2. Navigate to ~/Sitefinity/ConfigMigration.aspx page. 

   You should be logged as administrator.

3. Click *Migrate*

  * The system creates an automatic back up of your current configurations, which are stored in ~\App_Data\Sitefinity\Configuration folder, and stores it in the same folder with format _config_migration_backup"+"Time stamp"

  * All existing Sitefinity CMS configurations are extracted and saved in ~\App_Data\Sitefinity\Configuration folder.

4. Update in web.config by replacing

  ```<sitefinityConfig storageMode="Database" />``` with ```<sitefinityConfig storageMode="FileSystem" />```

5. Recycle the application pool of the website.

   Validate that your site is working correctly.
   
   After confirming that the application functions as expected, perform the following:
   
        a. In the database, delete all rows from table [sf_xml_config_items]
    
        b. Clean up the backups created in ~\App_Data\Sitefinity\Configuration folder.
