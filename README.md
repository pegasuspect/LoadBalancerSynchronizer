Load Balancer Syncronizer
==============

What is it?
--------------
It's simply a file synchronizing program where you have couple of servers/files to sync.
These files can be in different machines but they should be linked in one server as a shared folder.

Provides a background override operation with check to a database table consists of a path, sync status and publish time.

Dependencies
--------------
7-Zip.exe [LICENCE](http://www.7-zip.org/license.txt)

Prerequisites
--------------
- 7zip: 
  Needs to be downloaded and placed in **bin\Debug** or **bin\Release** folder with **7-Zip** filename. The path to 7-Zip should look like ...\bin\Release\7-Zip or ...\bin\Debug\7-Zip

###### If you want to use background synchronization;
- A database connection(can be SQLServer or MySql) and a table:
The table that is used for file checks is named **ApplicationSyncPath** and its columns are as follows:

  | Field Name        | Type        |
  | ------------      | :----:        |
  | **isSynced**      | *tinyint*|
  | **path**          | *varchar*|
  | **CreatedAt**     | *datetime*|
  | **UpdatedAt**     | *datetime*|
  | **ErrorMessage**  | *varchar*|
  | **Id**            | *int*|
  | **PublishTime**   | *datetime*|

How To Install
--------------
1. Download 7-zip either from [FileHippo](http://filehippo.com/search?q=7zip "7-Zip FileHippo") or from [7-Zip.org](http://www.7-zip.org/download.html "7-Zip.org") and install 7-Zip.
2. Place your installed **7-Zip** folder either in **bin/Debug** directory or in **bin/Release** directory of the project.    *Depending on which mode you are running your application in Visual Studio.*
3. Open the solution in Visual Studio, then from the menu *Build* > *Rebuild Solution*
4. Open the **bin/Debug** or **bin/Release** directory of the project, and run **LoadBalancerSyncronizer.exe**

How To Use
--------------
- When application is launched, click on the buttons to setup paths of each directory.

  Right click any button for overriding contents from main folder to target folder.
  ![Override right click.](../../raw/master/LoadBalancerSynchronizer/ScreenShots/Untitled-2.png)
  
- Override does not empty the target folder only overrides the files with the same name.
  ![Override copying.](../../raw/master/LoadBalancerSynchronizer/ScreenShots/Untitled-3.png)

  ![Override completed.](../../raw/master/LoadBalancerSynchronizer/ScreenShots/Untitled-1.png)

How It Works
--------------
- `Main Dir` button denotes, path to copy from.
- `Override Dir` buttons are for overriding to those directories: First files are zipped into tar format, then copied in big chunks(4MB), then extracted at the target folder.

###### If you want to use background syncronization;
- `DB Settings` button is for those who want to sync server files from database.
  - Background operation is checking **ApplicationSyncPath** table every 10 seconds.
  - It syncs paths of which isSycned property is set to false/0 and PublishDate property is over due.
- `Sync via Server Root Paths` button works as follows:

  TextBoxes are for server roots *ie. C:\wamp\www*.
  - First textbox is your main server. 
  - It's relative paths *ie /images/logo.png* in **ApplicationSyncPath** table 
    will be to copied to other textboxes' directories.
  - Other TextBoxes are for your other servers root paths, to override to.
