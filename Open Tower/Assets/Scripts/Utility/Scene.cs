using System.ComponentModel;

public enum Scene {

    [Description("Main_Menu")]
    MAIN_MENU,

    [Description("Credits")]
    CREDITS,

    [Description("Tutorial")]
    TUTORIAL,

    [Description("Story_Mode")]
    STORY_MODE,

    LEVEL_SELECT,
    LEVEL_EDITOR,
    USER_LEVELS,
}