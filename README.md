A training mod for Mount & Blade II: Bannerlord.

Currently, there is only one availabe training project: Blocking Training.

## Install

In order to use this mod, you need to *compile* and *deploy* the project.

It is assumed that the Bannerlord game is located at
`C:\Program Files (x86)\Steam\steamapps\common\Mount & Blade II Bannerlord`.

If you use a different location,
you may do the following modification to keep everything working:

First, change the `GamePath` property in `XYZTrainer\XYZTrainer.csproj`.
This is where Visual Studio finds the game libraries.

```xml
<PropertyGroup>
  <GamePath>C:\Program Files (x86)\Steam\steamapps\common\Mount &amp; Blade II Bannerlord\</GamePath>
</PropertyGroup>
```

Second, change the path in `simple_deploy.bat`. This is a simple script which
makes the deployment easier. 

Open `XYZTrainer.sln` with Visual Studio (I use VS 2019).
Select `Build -> Build Solution`. Check if it is compiled without error.

You should see two new folders: `obj` and `bin` created in `XYZTrainer`. 
In `bin`, a directory named `XYZTrainer` contains the finished mod.

Now, copy this directory to `Mount & Blade II Bannerlord\Modules`.
You can either do this manually or use `simple_deploy.bat` to do it for you.

Open `Mount & Blade II Bannerlord` and you should see a new option
"XYZ Training Fields" in the main menu.

## Blocking Training

Go to the trainer and fight her.
A score is given after either you or the trainer is defeated, roughly calculated as
the ratio of the damage you have blocked and the damage you haven't.
The scores are logged to `<UserDir>\Mount and Blade II Bannerlord\XYZTrainer\blocking_score.txt`

