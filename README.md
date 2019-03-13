# URA-cs
This is comprehensive RFID reader software built from the C# version of the Mercury API which should be downloaded here. 
[Mercury API Download](https://www.jadaktech.com/documentation/rfid/mercuryapi/)
## Setting Up Proper Dependencies

1. Create an empty directory on your PC.
2. Follow instructions below on cloning:
3. If you have not created an SSH key for your device, follow the steps on this page [SSH Keys](https://help.github.com/articles/checking-for-existing-ssh-keys/) (all the way to end! Last step is to add ssh key to github). First, check for existing keys, then generate new ssh key, then add key to github. Make sure the email you specify is an email you have linked to your github account. 
4. Navigate to this directory in Git Bash/Terminal.
5. Use command `git init` to initialize git repository.
6. Use command `git remote add origin git@github.com:jhchilds/URA-cs.git` 
7. Use command `git pull origin master`
8. If the steps are followed correctly you should be able pull without any conflicts.

## Proper Workflow
1. There are a lot of files in the URA that we will not be modifying. 
2. Therefore, we keep a fully functioning UniversalReaderAssistant2.0/UniversalReaderAssistant2.0 directory in order to properly build and run in Visual Studio. 
3. We will copy and paste files into our initialized repository, only modifying files connected to git.
4. Edit files, paste/replace files in full directory for test and build. 


### This project only involves editing a few files in the UniversalReaderAssistant Application
Do not add any other files to git. 

## DO NOT USE COMMAND `git add -A` 
There are too many files in the URA for git to handle and it WILL crash if you try to force adding all of these files. We will only be using version control for specific files concerning the UniversalReaderAssistant. 

### MICROSOFT VISUAL STUDIO BUG 
There is a bug in Visual Studio that I researched thoroughly. Although the project will build and run smoothly, VS will say there is an error with an XAML file "Busy Indicator". In reality, there is no issue, but it gets annoying because VS will not permit you to look at the Design Windows for UI modification.
# To fix this
1. Change the target build to  "Release x64". 
2. Clean and Rebuild the project. 
3. Change back to our desired target, which for now is "Debug x86". 
4. Clean and Rebuild. 
This should solve the problem as I've successfully handled the issue in this manner twice. It seems to be a mystery as to why this works on several online forums.

## Branching

Create branch:
```
git checkout -b name_of_branch
```

*EDIT, ADD AND COMMIT FILES*
```
git push -u origin name_of_branch
```

DELETE branch:
```
git checkout master

git branch -d name_of_branch
```

OR 

FORCE DELETE:
```
git branch -D name_of_branch
```

### Documentation

# UI/UserControls/ucTagResults.xaml.cs
Within file UI/UserControls/ucTagResults.xaml.cs, line 266, there is `TagReadRecord tr = (TagReadRecord)row.Item;` This may be where tag data is being updated to the tag results table. 

# UI/Main.xaml.cs
Within UI/Main.xaml.cs, line 2263, lies the for creating and writing tag data to a csv file. 

# TODO:
- [ ] Test ` TagReadRecord tr = (TagReadRecord)row.Item;` with reader and print out contents to the console to see what this returns.

- [ ] Explore Main.xaml, specifically SaveTagResults section (line 2263). 

- [ ] Filter out tag data on tag results table without VTrans prefix in EPC

- [ ] Connecting URA to MS SQL SERVER via TCP/IP Data Streaming

- [ ] Edit URA GUI 






