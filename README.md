DDL Creator
===========

Application for creating and editing DDL (device definition language) files for the MLC-16 lighting console.


Building
--------

### on linux

Install `Mono` from your favorite package manager, and run:

```shell
mcs -pkg:dotnet DDLCreator/*.cs DDLCreator/Properties/*.cs -out:DDLCreator.exe
```

