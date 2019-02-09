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

### This project only involves editing UniversalReaderAssistant2.0.csproj
Do not add any other files to git. 

## DO NOT USE COMMAND `git add -A` 
There are too many files in the API for git to handle.




