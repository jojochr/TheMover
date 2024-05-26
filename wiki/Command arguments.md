# Whats in here?

In this file I want to collect all the command Ideas i have.  
Some will get thrown out over time, some will be added over time.  


## Higher order commands

### **add \<PresetName\> \<--SourcePaths\> \<--DestinationPath\> [\<-i\>]**

Adds a preset.  
For this command a Name, a SourcePath and a DestinationPath must be supplied in order to create a valid Preset.  

**Presetnames must be unique.**  

---

#### **--SourcePaths**

Multiple full-filepaths (seperated by a ";") can be specified here.  
These files will be copied to the DestinationPath once this the Preset is run.  

---

#### **--DestinationPath**

A filepath where files will be copied to, when the preset is run.  

---

#### **-i**

is optional.  
It opens the "Open File"-Dialog from windows and enables the user to choose a few files instead of typing them into the Command-Line.  

**Specifying -i means that the specified Source- and DestinationPath will be ignored**

---

### **remove \<PresetName\>**

Removes a preset.  
The Presetname is the only identifier on this command since they are unique.  

### **update \<PresetName\> [--renameTo] [--SourcePaths] [--DestinationPath]**

Updates a preset.  
The Presetname, Sourcepaths and Destinationpaths can be adjusted.  

---

#### **--SourcePaths**

Multiple full-filepaths (seperated by a ";") can be specified here.  
These files will be copied to the DestinationPath once this the Preset is run.  

---

#### **--DestinationPath**

A filepath where files will be copied to, when the preset is run.  

---

#### **--renameTo**

This option should always preceed the new Presetname which must be specified in between ""s.  

**Presetnames must be unique.**  

---

## **"--help" is seperate because it always works**

--help is absolutely neccesary, and also very simple.  
Using help as an option means to not execute anything and to display a manual.  

The help flag will tack onto the first higher order command it finds and will display information on all its options and syntax.  
