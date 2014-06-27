Load Balancer Syncronizer
==============

What it is?
--------------
It's simply a file syncronizing program where you have couple of servers/files to sync.
Provides a background operation for checking what files needs to be sycned.

Prerequisites
--------------
- 7zip: 
  Needs to be downloaded and placed in bin forder with **7zip** filename. The path to 7zip should look like ...\bin\7zip

###### If you want to use background syncronization;
- A database connection(can be SQLServer or MySql) and a table: 
The table that is use for file checks is named **ApplicationSyncPath** and its columns is as follows:

| Field Name        | Type        |
| ------------      | ----        |
| **isSynced**      | *tinyint   *|
| **path**          | *varchar   *|
| **CreatedAt**     | *datetime  *|
| **UpdatedAt**     | *datetime  *|
| **ErrorMessage**  | *varchar   *|
| **Id**            | *int       *|
| **PublishTime**   | *datetime  *|

How To Install
--------------
1. First download 7-zip either from [FileHippo](http://filehippo.com/search?q=7zip "7-Zip FileHippo") or from [7-Zip](http://www.7-zip.org/download.html "7-Zip.org")
2. Simply open the solution in Visual Studio, then from the menu *Build* > *Rebuild Solution*
3. Open the bin directory of the soluntions directory, and run **LoadBalancerSyncronizer**

How To Use
--------------
After opening the application, click on the buttons to setup paths of each director for override operation.
`Main Dir` button denotes, path to copy from. and `Override Dir` buttons are for overriding to those directories.
###### If you want to use background syncronization;
- `DB Button` is for those who want to sync their files throug an insert to the table on database.
- `Sync via Server Root Paths` button works as follows:
⋅⋅1  First TexBbox is your main server root directory `ie. *www* or *wwwroot*` to copy files whose paths are relative to server. `ie */images/logo.png*`
⋅⋅2  Other TextBoxes are for your other servers root pathsto override into, can be used for admin panel's file uploads.
