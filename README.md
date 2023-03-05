How to start:
  - Create a sql server and edit the connection strings in appsettings.json
  - Create migration and update database
  - Start app and load data from json file or use system directory
 
How to use:
  - Click "Get data from file" -> Input path and name of file (preferably .json) -> Click "Continue" | Your data will be load to db
  - Click "Get data from system" -> Input path and name of folder -> Click "Continue" | Your data will be load to db
  
  On main page you can see the name of parent folder, if data has been loaded in db
  - Click on this name to see child folder
  
  On opened page you can see parent folder and child folder names
  - Click "Home" to go to the main page
  - Click "Save" -> Input path and name of file (preferably .json) -> Click "Continue" | Your data from db will be loaded to file
  - Click on child folder name to see the child folder
  
  You can use "example.json" from this repository for gеtting data from file.
  
  If you see "HTTP 404 Not Found" after having inputed path and name of file or folder, you should go back and check inputed data.   
