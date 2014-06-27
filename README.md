Load Balancer Syncronizer
==============

What it is?
--------------
It's simply a file syncronizing program where you have couple of servers/files to sync.
These files can be in different machines but they should be linked in one server as a shared folder.

Provides a background override operation with check to a database table consists of a path, sync status and publish time.

Dependesies
--------------
7-Zip.exe [LISENCE](http://www.7-zip.org/license.txt)

Prerequisites
--------------
- 7zip: 
  Needs to be downloaded and placed in **bin** forder with **7zip** filename. The path to 7zip should look like ...\bin\Release\7-Zip or ...\bin\Debug\7-Zip

###### If you want to use background syncronization;
- A database connection(can be SQLServer or MySql) and a table: 
The table that is use for file checks is named **ApplicationSyncPath** and its columns are as follows:

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
2. Place your installed **7-Zip** folder either in **bin/Debug** directory or in **bin/Release** directory of the project.    Depending on which mode you are running your application in Visual Studio.
3. Open the solution in Visual Studio, then from the menu *Build* > *Rebuild Solution*
4. Open the **bin/Debug** or **bin/Release** directory of the project, and run **LoadBalancerSyncronizer.exe**

How To Use
--------------
After opening the application, click on the buttons to setup paths of each director for override operation.

`Main Dir` button denotes, path to copy from.
`Override Dir` buttons are for overriding to those directories.

###### If you want to use background syncronization;
- `DB Settings` button is for those who want to sync server files from database.
  - Background operation is checking **ApplicationSyncPath** table every 10 seconds.
  - It syncs paths with isSycned set to false/0 and PublishDate is over due.
- `Sync via Server Root Paths` button works as follows:

  TextBoxes are for server roots `ie. *C:\wamp\www*`.
  - First textbox is your main server. 
  - It's relative paths `ie */images/logo.png*` in **ApplicationSyncPath** table 
    will be to copied to other textboxes' directories.
  - Other TextBoxes are for your other servers root paths, to override to.
