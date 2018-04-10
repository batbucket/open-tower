public class LevelParams {
    public readonly int WorldIndex;
    public readonly char StageIndex;
    public readonly string Name;

    public LevelParams(int worldIndex, int stageIndex, string name) : this(worldIndex, char.Parse(stageIndex.ToString()), name) {
    }

    public LevelParams(int worldIndex, string name) : this(worldIndex, '*', name) {
    }

    private LevelParams(int worldIndex, char stageIndex, string name) {
        WorldIndex = worldIndex;
        StageIndex = stageIndex;
        Name = name;
    }
}