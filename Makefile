

# Compilation flags
MCS_FLAGS := -pkg:dotnet \
             -resource:DDLCreator/Resources/WWD_Logo.png,WWD_Logo

# DDLCreator source
SRC := DDLCreator/Program.cs \
       DDLCreator/MainForm.cs \
       DDLCreator/MainForm.Designer.cs \
       DDLCreator/IO.cs \
       DDLCreator/Data.cs \
       DDLCreator/About.cs \
       DDLCreator/About.Designer.cs \
       DDLCreator/IndexGenerator.cs \
       DDLCreator/IndexGenerator.Designer.cs \
       DDLCreator/UnionGenerator.cs \
       DDLCreator/UnionGenerator.Designer.cs \
       DDLCreator/ErrorReport.cs \
       DDLCreator/ErrorReport.Designer.cs \
       DDLCreator/Preview.cs \
       DDLCreator/Preview.Designer.cs

# Properties
SRC += DDLCreator/Properties/AssemblyInfo.cs \
       DDLCreator/Properties/Resources.Designer.cs \
       DDLCreator/Properties/Settings.Designer.cs




.PHONY: all
all: DDLCreator.exe


DDLCreator.exe: $(SRC)
	mcs $(MCS_FLAGS) $^ -out:$@


.PHONY: clean
clean:
	rm -f DDLCreator.exe
