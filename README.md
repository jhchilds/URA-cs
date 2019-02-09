# URA-cs
This is comprehensive RFID reader software built from the C# version of the Mercury API which should be downloaded here. 
[Mercury API Download](https://www.jadaktech.com/documentation/rfid/mercuryapi/)
## Setting Up Proper Dependencies

1. Create a directory called software_cs on your PC.
2. Copy and paste the entire cs version of the Mercury API in this directory. (this may take some time)
3. If you have not created an SSH key for your device, follow the steps on this page [SSH Keys](https://help.github.com/articles/checking-for-existing-ssh-keys/) (all the way to end! Last step is to add ssh key to github). First, check for existing keys, then generate new ssh key, then add key to github. Make sure the email you specify is an email you have linked to your github account. 
4. Navigate to this directory in Git Bash/Terminal.
5. Use command `git init` to initialize git repository.
6. Use command `git remote add origin git@github.com:jhchilds/URA-cs.git` 
7. Use command `git pull origin master`
8. If the steps are followed correctly you should be able pull without any conflicts.

## Setting Up Proper Dependencies Alternative
1. Clone the repository by downloading the zip file. You'll need to initialize the repo locally and add the remote correctly as described above. Test this connection to the remote by making a small edit to this README and pushing, make no other changes unless contacting me first.  
2. This will set up proper directory path to URA, but will not contain Mercury API contents.
3. You will need to do this manually.
4. Compare the mercuryapi-1.31.2.40/cs and URA-cs-master/cs and copy and paste ALL files in each directory, as there are many dependencies to be satisfied.
5. After completing this task, you should be able to push and pull files from URA-cs-master/cs/Samples/UniversalReaderAssistant2.0/UniversalReaderAssistant2.0/UI


### This project only involves editing UniversalReaderAssistant2.0.csproj 
Do not add any other files to git. 

## DO NOT USE COMMAND `git add -A` 
There are too many files in the API for git to handle and it WILL crash if you try to force adding all of these files. We will only be using version control for specific files concerning the UniversalReaderAssistant. 

## Branching

Create branch:

`git checkout -b name_of_branch`

*EDIT, ADD AND COMMIT FILES*

`git push -u origin name_of_branch`

DELETE branch:
```
git checkout master

git branch -d name_of_branch
```

OR 

FORCE DELETE:

`git branch -D name_of_branch`

### Documentation

# UI/UserControls/ucTagResults.xaml.cs
Within file UI/UserControls/ucTagResults.xaml.cs, line 266, there is `TagReadRecord tr = (TagReadRecord)row.Item;` This may be where tag data is being updated to the tag results table. 

# UI/Main.xaml.cs
Within UI/Main.xaml.cs, line 2263, lies the for creating and writing tag data to a csv file. 

# TODO:
- [ ] Test ` TagReadRecord tr = (TagReadRecord)row.Item;` with reader and print out contents to the console to see what this returns.

- [ ] Explore Main.xaml, specifically SaveTagResults section (line 2263). 

- [ ] Filter out tag data on tag results table without VTrans prefix in EPC

- [ ] Filter out tag data when saving to a csv table without VTrans prefix in EPC






