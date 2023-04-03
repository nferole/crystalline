
# Crystaline
Crystaline is a command line tool used to extract files which are stored in more mercurial ways.
The project started as a fun learning experience during code nights.

# Usage
Cry --help should get you started!

There are many good reasons to not specify usernames and passwords in plain text CLI arguments.
If you want you can pass a json-file with the arguments using the --config argument. The JSON needs to represent the config arguments.

If you want you can let cry create an example config file for you!
```console
cry [sqlserver/base64...] --make-config
```

## Example (SQL Server):

```console
cry sqlserver --endpoint https://db.test.com --username testuser --password testpassword --Database TestDB --Query "SELECT * FROM BLOBTable" --ColumnBLOB "BLOB" --ColumnFileName "FileName" --Output "./ExtractedFiles/"
```

OR

```console
cry sqlserver --config conf.json
```

```json
{
	"Endpoint" : "https://db.test.com",
	"Username": "testuser",
	"Password": "testpassword",
	"Database":  "TestDB",
	"Query": "SELECT * FROM BLOBTable",
	"ColumnBLOB": "BLOB",
	"ColumnFileName": "FileName",
	"Output": "./ExtractedFiles"
}
```
## Example (Base64 String):
```console
cry base64 --output "./image.png" --string "..." 
```

# Roadmap
## Extractors
 - [ ] MySQL
 - [ ] MariaDB
 - [ ] MongoDB
 - [ ] CosmosDB
 - [ ] JSON-files
 ## File Writers
 - [ ] Azure BLOB Storage File Writer
 - [ ] AWS BLOB Storage File Writer
 - [ ] GCP BLOB Storage
 - [ ] SFTP File Writer
 - [ ] Google Drive Writer

# History

Me and my brothers wanted a simple way to help solve a common problem faced by all of us in various projects:
Large Binary Objects stored in database tables.

Even though this has its usecases all of us usually entered projects where the BLOBs had become a problem of some sort:
Bottlenecks, tricky to debug, cost increases when migrating to the cloud, etc.

After trying various tools for BLOB exports we decided a gratifying project for our code nights would be a tool that made our lives easier.
Some parts of this CLI might be over-engineered, but at least we had alot of fun doing it!

Crystaline consists of a CommandLineUtils-project which have had many iterations by junior, medior and senior developers.
Why? Well, because it was fun, of course! There are many projects like it, and many are probably much better.

The CommandLineUtils settled as a dynamic binding tool which lets you specify a command by creating a new implementation of 
the ICLICommand interface. The properties of the implementation will (unless properly decorated) correspond to arguments.
You can decorate a property with [ConfigPath] to make it bind a target json-file to the properties.

This was (kind of) mainly done to show off reflection and seperation of concerns.





