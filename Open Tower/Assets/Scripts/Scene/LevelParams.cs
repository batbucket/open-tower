public class LevelParams {
    public readonly int WorldIndex;
    public readonly char StageIndex;
    public readonly string Name;
    public readonly int ScoresID;

    public LevelParams(int worldIndex, int stageIndex, int scoresID, string name) : this(worldIndex, char.Parse(stageIndex.ToString()), name, scoresID) {
    }

    public LevelParams(int worldIndex, string name) : this(worldIndex, '*', name, 0) {
    }

    private LevelParams(int worldIndex, char stageIndex, string name, int scoresID) {
        WorldIndex = worldIndex;
        StageIndex = stageIndex;
        Name = name;
        ScoresID = scoresID;
    }
}