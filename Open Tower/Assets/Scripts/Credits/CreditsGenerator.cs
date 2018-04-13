using UnityEngine;

public class CreditsGenerator : MonoBehaviour {

    [SerializeField]
    private Credit prefab;

    [SerializeField]
    private Gradient colors;

    [SerializeField]
    private Sprite[] possibleSprites;

    private void Start() {
        AddCredit("Win Godwin", "Writing", "Art");
        AddCredit("Kory Hunter", "Level Design");
        AddCredit("Alex Kochengin", "Level Design");
        AddCredit("James Lee", "Art", "Quality Assurance");
        AddCredit("Drew Teachout", "Level Design", "Art");
        AddCredit("Rachel Tierney", "Writing");
        AddCredit("Sarah Tsai", "Art");
        AddCredit("Andy Xue", "Project Lead", "Programming");
        AddCredit("Jeffrey Zhang", "Writing", "Level Design");
    }

    private void AddCredit(string name, params string[] roles) {
        Instantiate(prefab, this.transform)
            .Init(
            possibleSprites.PickRandom(),
            name,
            string.Join("\n", roles),
            Color.white,
            Color.yellow
            );
    }
}