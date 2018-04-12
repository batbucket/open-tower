public class LevelParams {
    private const int NO_TROPHY_ID = 0;
    private const int NO_SCORES_ID = 0;

    public readonly int WorldIndex;
    public readonly char StageIndex;
    public readonly string Name;
    public readonly int ScoresID;
    public readonly int TrophyID;

    public bool HasTrophy {
        get {
            return TrophyID != NO_TROPHY_ID;
        }
    }

    public LevelParams(int worldIndex, int stageIndex, int scoresID, int trophyID, string name) : this(worldIndex, char.Parse(stageIndex.ToString()), name, scoresID, trophyID) {
    }

    public LevelParams(int worldIndex, int stageIndex, int scoresID, string name) : this(worldIndex, char.Parse(stageIndex.ToString()), name, scoresID, NO_TROPHY_ID) {
    }

    public LevelParams(int worldIndex, string name) : this(worldIndex, '*', name, NO_SCORES_ID, NO_TROPHY_ID) {
    }

    private LevelParams(int worldIndex, char stageIndex, string name, int scoresID, int trophyID) {
        WorldIndex = worldIndex;
        StageIndex = stageIndex;
        Name = name;
        ScoresID = scoresID;
        TrophyID = trophyID;
    }
}