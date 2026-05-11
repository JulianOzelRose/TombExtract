# Tomb Raider I-VI Remastered Savegame Manager
An open source, cross-platform savegame manager for Tomb Raider I-VI Remastered. Main features are listed below. For installation and use instructions, navigate to the next section.
For a savegame editor for Tomb Raider I-VI Remastered, check out [TRR-SaveMaster](https://github.com/JulianOzelRose/TRR-SaveMaster).

### Features
- 📥 Import Savegames
- 🔄 Platform Conversion (PC/PS4/Android/Nintendo Switch)
- 🔀 Patch Conversion (Pre-Patch -> Patch 5)
- 💾 Savegame Management (Deletion & Reordering)
- 📍 Level Selection / Savegame Creation

<br>
<img width="596" height="631" alt="TombExtract-UI" src="https://github.com/user-attachments/assets/75e81169-1a6b-44db-9b07-effbfaf3a09a" />

## Installation and use
To use this program, navigate to the [Releases](https://github.com/JulianOzelRose/TombExtract/releases)
page, then download the .exe of the latest version under "Assets". Once downloaded, click the browse button next to source file textbox to select the savegame source. Then click the browse button
next to the destination file to select your savegame file. Alternatively, you can just drag and drop your savegame file onto the savegame list box, and it will populate. Be sure to select the platform
of the source and destination savegames using the dropdowns to ensure they are read properly.

## Extracting and converting savegames
Use the checklist on the left to select which savegames you would like to import/convert. This program detects patch versions automatically and will apply the necessary conversions. If the source and
destination platforms are different, it will also apply the necessary conversions. Click "Extract"/"Convert" to transfer the savegames, and the program will begin extraction. The progress display will indicate how far along the process is. Platform conversion is not required for Tomb Raider IV-VI.

If you are trying to convert from PS4, you must first decrypt the savegame file using [Apollo Save Tool](https://github.com/bucanero/apollo-ps4). For Nintendo Switch savegames, you can either use
[EdZion](https://github.com/WerWolv/EdiZon) or [Goldleaf](https://github.com/XorTroll/Goldleaf) to extract the savegame file from your console. You can find more detailed information on how to do this
[here](https://github.com/JulianOzelRose/TombExtract/issues/1#issuecomment-1978837071). It is recommended that you check the "Backup before writing" option before transferring or converting.

For Android, accessing the savegame file requires a rooted device. Rooting your device may void your warranty and can introduce security risks,
so it is generally not recommended. However, Android savegames can still be transferred and converted if your device is rooted.

## Deleting and reordering savegames
<img width="298" height="399" alt="ManageSlotsForm-UI" src="https://github.com/user-attachments/assets/8b3526b9-c084-44ee-a2e1-beb31b61b101" />
<br>

To delete and/or reorder savegames, load the savegame file you'd like to modify as the destination file, then click "Manage Slots".
Use the up and down arrows at the top to reorder the savegames to different slots.
When you are done reordering your savegames, click "Reorder", and a progress display will appear to show how far along the process is.
You can also use the trash can button to delete a savegame.
Again, it is recommended that you check the "Backup before writing" option in the main window before deleting or reordering savegames as a precautionary measure.


## Level selection
<img width="378" height="211" alt="CreateSavegameForm-UI" src="https://github.com/user-attachments/assets/1daa2fb4-d1b8-4264-a9f6-f9033b8c8df6" />
<br>

To use level selection, load the savegame file you'd like to modify as the destination file, then click "Manage Slots".
Next, select the slot you would like to add the new savegame to, then click the large plus button on the right.
You can then specify the level, platform, game mode, and save number you'd like. All premade savegames start at the beginning
of the level, with the metadata (statistics, time taken, distance travelled, etc.) all wiped clean.

## Dark Mode
<img width="596" height="609" alt="TombExtract-DarkMode-UI" src="https://github.com/user-attachments/assets/80c2880c-a190-4cbf-8dea-1a86efac6fff" />
<br>

If you prefer a darker interface, you can enable Dark Mode from the Settings menu at the top of the program.
Please note that Dark Mode may not display correctly when using very high or very low DPI settings.
