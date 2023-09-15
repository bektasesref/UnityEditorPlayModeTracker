# UnityEditorPlayModeTracker
Unity script &amp; simple database API link to track which user, in which network and project clicked play &amp; stop in editor

Step by Step;

1) Retrieve UnityRegistrar.cs to your project
2) In the Hosted_PHP folder, copy and paste UnityRegistrar.php to your website directory
3) In UnityRegistrar.cs, change invokeUrl string as hardcoded with your URL like. It should be something like;  invokeUrl = "https://www.[corrupted].com/UnityRegistrar.php";
4) Open phpMyAdmin (/or Workbench etc.) in your website and import Logger.sql file in Hosted_DB folder
5) Open UnityRegistar.php file and change the following values according to your database's credentials;
  $username = "esref_Editor";
  $password = "1wNF....9VG";
  $dbname = "stu..._UnityLogger";


Database log's should look like this;

![image](https://github.com/bektasesref/UnityEditorPlayModeTracker/assets/23198585/2b2df5c2-49d0-48fb-9840-9c2df5568ab6)



![image](https://github.com/bektasesref/UnityEditorPlayModeTracker/assets/23198585/201b3559-05a1-4c8b-be59-eb2b6112dd5d)

In the devices table, you can also ban user while in splashscreen. It's added as backtrack door/silent quit feature to prevent unwanted users to open project.

I suggest you create a .DLL file and name it something discreet.
