# Tomb Raider I-VI Remastered Savegame Manager
This is a savegame manager for Tomb Raider I-VI Remastered. It lets you transfer individual savegames between files, convert between platforms, delete or reorder savegames, and create savegames for any level.
Useful for downloading savegames from the Internet without having to replace your entire savegame file, and for level selection. For installation and use instructions, simply navigate to the next section below.
For a savegame editor for Tomb Raider I-VI Remastered, check out [TRR-SaveMaster](https://github.com/JulianOzelRose/TRR-SaveMaster).

<img width="596" height="611" alt="TombExtract-UI" src="https://github.com/user-attachments/assets/994d7600-1415-4564-a2c5-5cc09f9f2532" />


## Installation and use
To use this program, simply navigate to the [Releases](https://github.com/JulianOzelRose/TombExtract/releases)
page, then download the .exe of the latest version under "Assets". Once downloaded, click the browse button next to source file textbox to select the savegame source. Then click the browse button
next to the destination file to select your savegame file. Alternatively, you can just drag and drop your savegame file onto the savegame list box, and it will populate. Your main savegame file should be located in:

#### Tomb Raider I-III Remastered
`C:\Users\USERNAME\AppData\Roaming\TRX\77777777777777777\savegame.dat`

#### Tomb Raider IV-VI Remastered
`C:\Users\USERNAME\AppData\Roaming\TRX\77777777777777777\savegame.dat`

Just replace "USERNAME" with your username, and "77777777777777777" with whatever numeric ID you see in that folder. The numeric ID is your Steam Community ID, so if you have multiple Steam
accounts with Tomb Raider Remastered, there may be multiple folders.

## Extracting and converting savegames
Use the checklist on the left to select which savegames you would like to transfer. The combo box below the check list allows you to specify whether or not you would like to convert your savegames.
Select the relevant conversion, or leave it at "No" to transfer them as is. Click "Extract" to transfer the savegames, and the program will begin extraction. The progress display will indicate how far along the process is.
Platform conversion is not required for Tomb Raider IV-VI.

If you are trying to convert from PS4, you must first decrypt the savegame file using [Apollo Save Tool](https://github.com/bucanero/apollo-ps4). For Nintendo Switch savegames, you can either use
[EdZion](https://github.com/WerWolv/EdiZon) or [Goldleaf](https://github.com/XorTroll/Goldleaf) to extract the savegame file from your console. You can find more detailed information on how to do this
[here](https://github.com/JulianOzelRose/TombExtract/issues/1#issuecomment-1978837071). It is recommended that you check the "Backup before writing" option before transferring or converting.

## Deleting and reordering savegames
To delete and/or reorder savegames, load the savegame file you'd like to modify as the destination file, then click "Manage Slots".
Use the up and down arrows at the top to reorder the savegames to different slots.
When you are done reordering your savegames, click "Reorder", and a progress display will appear to show how far along the process is.
You can also use the trash can button to delete a savegame.
Again, it is recommended that you check the "Backup before writing" option in the main window before deleting or reordering savegames as a precautionary measure.

<img width="298" height="398" alt="ManageSlots-UI" src="https://github.com/user-attachments/assets/8bf21e8b-8aae-441c-9b0f-35c8f346ec42" />


## Level selection
To use level selection, load the savegame file you'd like to modify as the destination file, then click "Manage Slots".
Next, select the slot you would like to add the new savegame to, then click the large plus button on the right.
You can then specify the level, platform, game mode, and save number you'd like. All premade savegames start at the beginning
of the level, with the metadata (statistics, time taken, distance travelled, etc.) all wiped clean.

<img width="378" height="211" alt="CreateSavegame-UI" src="https://github.com/user-attachments/assets/61757f57-ee82-41be-b154-2fb861c00bec" />
