namespace TombExtract
{
    public class Globals
    {
        // Application metadata & configuration
        public const string CONFIG_FILE_NAME = "TombExtract.ini";
        public const string WINDOW_TITLE_MANAGE_SLOTS = "Manage Slots";
        public const string WINDOW_TITLE_CREATE_SAVEGAME = "Create Savegame";
        public const string VERSION = "4.74";

        // Config file keys
        public const string CONFIG_KEY_STATUS_BAR = "StatusBar=";
        public const string CONFIG_KEY_DARK_MODE = "DarkMode=";

        // Tab IDs
        public const int TAB_TR1 = 0;
        public const int TAB_TR2 = 1;
        public const int TAB_TR3 = 2;
        public const int TAB_TR4 = 3;
        public const int TAB_TR5 = 4;
        public const int TAB_TR6 = 5;

        // Savefile sizes
        public const int SAVEFILE_SIZE_TRX_PREPATCH = 0x152004;
        public const int SAVEFILE_SIZE_TRX_PATCH5 = 0x272004;
        public const int SAVEFILE_SIZE_TRX2 = 0x3DCA04;

        // Savegame slot sizes and constants
        public const int MAX_SAVEGAMES = 32;
        public const int SAVEGAME_SIZE_TRX_PREPATCH = 0x3800;
        public const int SAVEGAME_SIZE_TRX_PATCH5 = 0x6800;
        public const int SAVEGAME_SIZE_TRX2 = 0xA470;

        // Savefile & savegame header
        public const int SAVEFILE_VERSION_OFFSET = 0x000;
        public const int SLOT_STATUS_OFFSET = 0x004;
        public const byte SAVEFILE_TRX_PREPATCH = 0x3B;
        public const byte SAVEFILE_TRX_PATCH5 = 0x3C;
        public const byte SAVEFILE_TRX2_FORMAT = 0x28;

        // Links
        public const string GITHUB_LINK = "https://github.com/JulianOzelRose";
        public const string GITHUB_README_LINK = "https://github.com/JulianOzelRose/TombExtract/blob/master/README.md";
        public const string GITHUB_REPORT_BUG_LINK = "https://github.com/JulianOzelRose/TombExtract/issues";

        // Display labels
        public const string EMPTY_SLOT_TEXT = "Empty Slot";

        // Dialog messages & titles
        public const string DIALOG_MSG_CONFIRM_SAVEGAME_DELETE = "Are you sure you wish to delete";
        public const string DIALOG_MSG_SAVEGAME_FILE_NOT_FOUND = "Could not find savegame file.";
        public const string DIALOG_MSG_INVALID_SAVEGAME_FILE_TRX = "Not a valid Tomb Raider I–III Remastered savegame file.";
        public const string DIALOG_MSG_INVALID_SAVEGAME_FILE_TRX2 = "Not a valid Tomb Raider IV–VI Remastered savegame file.";
        public const string DIALOG_MSG_WRITE_IN_PROGRESS_EXIT_CONFIRM = "Exiting in the middle of a write operation could result in a corrupted savegame file. Are you sure you wish to exit?";
        public const string DIALOG_MSG_WRITE_IN_PROGRESS_PLEASE_WAIT = "A savegame write operation is in progress. Please wait until it completes.";
        public const string DIALOG_MSG_NO_SAVEGAMES_SELECTED = "Please select at least one savegame to convert.";
        public const string DIALOG_MSG_NO_SAVEGAME_SELECTED = "Please select a savegame first.";
        public const string DIALOG_MSG_SAVEGAME_BUFFER_NOT_FOUND = "Premade savegame buffer not found for this level or mode.";
        public const string DIALOG_MSG_SAVEGAME_SOURCE_FILE_NOT_FOUND = "Could not find savegame source file.";
        public const string DIALOG_MSG_SAVEGAME_DESTINATION_FILE_NOT_FOUND = "Could not find savegame destination file.";
        public const string DIALOG_MSG_NO_LEVEL_SELECTED = "Please select a level before creating the savegame.";
        public const string DIALOG_MSG_OPERATION_CANCELED = "Operation was canceled.";
        public const string DIALOG_MSG_CANNOT_DELETE_EMPTY_SLOTS = "Empty slots cannot be deleted.";
        public const string DIALOG_TITLE_CONFIRMATION = "Confirmation";
        public const string DIALOG_TITLE_ERROR = "Error";
        public const string DIALOG_TITLE_CANCELED = "Canceled";
        public const string DIALOG_TITLE_SUCCESS = "Success";
        public const string DIALOG_TITLE_UNABLE_TO_CONVERT = "Unable to Convert";
        public const string DIALOG_TITLE_PLATFORM_NOT_SUPPORTED = "Platform Not Supported";
        public const string DIALOG_TITLE_SAVEGAME_FILE_VERSION_NOT_SUPPORTED = "Unsupported Savegame File Version";
        public const string DIALOG_TITLE_INVALID_SAVEGAME_FILE = "Invalid Savegame File";
        public const string DIALOG_TITLE_WRITE_IN_PROGRESS = "Write In Progress";
        public const string DIALOG_TITLE_NO_SAVEGAMES_SELECTED = "No Savegames Selected";
        public const string DIALOG_TITLE_NO_SAVEGAME_SELECTED = "No Savegame Selected";
        public const string DIALOG_TITLE_NO_LEVEL_SELECTED = "No Level Selected";
        public const string DIALOG_TITLE_CREATE_SAVEGAME = "Create Savegame";
        public const string DIALOG_TITLE_CONVERSION_WARNING = "Conversion Warning";
        public const string DIALOG_TITLE_INVALID_ACTION = "Invalid Action";

        // Status messages
        public const string STATUS_MSG_SAVEGAME_FILE_BACKUP_SUCCESS = "Created backup:";
        public const string STATUS_MSG_DESTINATION_FILE_OPEN_SUCCESS = "Opened destination file:";
        public const string STATUS_MSG_SAVEGAME_CREATE_SUCCESS = "Successfully created savegame:";
        public const string STATUS_MSG_SAVEGAME_CREATE_ERROR = "Error creating savegame";
        public const string STATUS_MSG_CONVERSION_CANCELED = "Conversion canceled";
        public const string STATUS_MSG_CONVERSION_ERROR = "Error converting savegame(s)";
        public const string STATUS_MSG_CONVERSION_IN_PROGRESS = "Converting savegame(s)...";
        public const string STATUS_MSG_TRANSFER_CANCELED = "Transfer canceled";
        public const string STATUS_MSG_TRANSFER_ERROR = "Error transferring savegame(s)";
        public const string STATUS_MSG_TRANSFER_IN_PROGRESS = "Transferring savegame(s)...";
        public const string STATUS_MSG_DELETION_IN_PROGRESS = "Deleting savegame...";
        public const string STATUS_MSG_DELETION_ERROR = "Error deleting savegame";
        public const string STATUS_MSG_DELETION_CANCEL = "Savegame deletion canceled";
        public const string STATUS_MSG_DELETION_SUCCESS = "Successfully deleted savegame:";
        public const string STATUS_MSG_REORDER_SUCCESS = "Successfully reordered savegames";
        public const string STATUS_MSG_REORDER_CANCELED = "Savegame reordering canceled";
        public const string STATUS_MSG_REORDER_ERROR = "Error reordering savegames";
        public const string STATUS_MSG_REORDER_IN_PROGRESS = "Reordering savegames...";
    }
}
