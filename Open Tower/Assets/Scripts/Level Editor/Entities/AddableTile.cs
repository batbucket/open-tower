using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddableTile : MonoBehaviour {
    private const int MAX_VALUE_FOR_ENEMY_STATS = 99999;
    private const int MAX_VALUE_FOR_BOOSTER_STATS = 9999;

    private static readonly IDictionary<int, StatType> MAPPING = new Dictionary<int, StatType>() {
        { 0, StatType.LIFE },
        { 1, StatType.POWER },
        { 2, StatType.DEFENSE },
        { 3, StatType.EXPERIENCE }
    };

    private static EntitiesPanel _parentPanel;

    [SerializeField]
    private Outline outline;

    [SerializeField]
    private TileType tile;

    // NOT used if tile is not of key type
    [SerializeField]
    private KeyType key;

    [SerializeField]
    private Button changeSprite;

    [SerializeField]
    private Button delete;

    [SerializeField]
    private InputField life;

    [SerializeField]
    private InputField power;

    [SerializeField]
    private InputField defense;

    [SerializeField]
    private InputField experience;

    [SerializeField]
    private InputField boostedAmount;

    /// <summary>
    /// The booster stat icons, MUST be parallel to StatType!
    /// </summary>
    [SerializeField]
    private Sprite[] boosterStatIcons;

    [SerializeField]
    private Image boosterStatIcon;

    [SerializeField]
    private Image image;

    [SerializeField]
    private Element elementPrefab;

    private StatType boostedStatType;

    private int spriteID;

    public TileType TileType {
        get {
            return tile;
        }
    }

    public Sprite Sprite {
        get {
            return image.sprite;
        }
    }

    public StatType BoostedStatType {
        get {
            Util.Assert(tile == TileType.BOOSTER, "Expected Booster type, was {0} instead.", this.tile);
            return boostedStatType;
        }
    }

    private static EntitiesPanel ParentPanel {
        get {
            if (_parentPanel == null) {
                _parentPanel = FindObjectOfType<EntitiesPanel>();
            }
            return _parentPanel;
        }
    }

    // Toggle through all boostable stats
    public void IterateBoosterStat() {
        boostedStatType = boostedStatType.Next();
        boosterStatIcon.sprite = boosterStatIcons[(int)BoostedStatType];
    }

    // TODO
    public void Delete() {
        Destroy(gameObject);
        if (EntitiesPanel.Instance.LastSelected == this) {
            EntitiesPanel.Instance.LastSelected = null;
        }
    }

    public void SelectThisTile() {
        EntitiesPanel.Instance.LastSelected = this;
    }

    public void OnBoosterValueChange(string value) {
        Util.Assert(tile == TileType.BOOSTER, "Expected Booster type but was {0}", tile);
        ClampStats(boostedAmount, 1);
    }

    public void OnLifeChange(string value) {
        Util.Assert(tile == TileType.ENEMY, "Expected Enemy type but was {0}", tile);
        ClampStats(life, 1);
    }

    public void OnPowerChange(string value) {
        Util.Assert(tile == TileType.ENEMY, "Expected Enemy type but was {0}", tile);
        ClampStats(power, 0);
    }

    public void OnDefenseChange(string value) {
        Util.Assert(tile == TileType.ENEMY, "Expected Enemy type but was {0}", tile);
        ClampStats(defense, 0);
    }

    public void OnExperienceChange(string value) {
        Util.Assert(tile == TileType.ENEMY, "Expected Enemy type but was {0}", tile);
        ClampStats(experience, 0);
    }

    public void CreateElement(Transform parent) {
        Element e = Instantiate(elementPrefab, parent);
        e.Init(this);
    }

    private void ClampStats(InputField field, int min) {
        Util.ClampField(field, min, MAX_VALUE_FOR_ENEMY_STATS);
    }

    private int GetEnemyStatCount(StatType stat) {
        Util.Assert(tile == TileType.ENEMY, "Expected Enemy type, was {0} instead.", this.tile);
        InputField field = null;
        switch (stat) {
            case StatType.LIFE:
                field = life;
                break;

            case StatType.POWER:
                field = power;
                break;

            case StatType.DEFENSE:
                field = defense;
                break;

            case StatType.EXPERIENCE:
                field = experience;
                break;
        }
        int count = 0;
        Util.Assert(int.TryParse(field.text, out count), "Failed to parse {0}", field.text);
        return count;
    }

    private void Start() {
        if (changeSprite != null) {
            changeSprite.onClick.AddListener(new UnityEngine.Events.UnityAction(
                () => {
                    SpritePicker.Instance.Activate(this.TileType, pickable => {
                        image.sprite = pickable.Sprite;
                        spriteID = pickable.ID;
                    });
                }
                ));
        }
    }

    private void Update() {
        outline.effectColor = (ParentPanel.LastSelected == this) ? Color.green : Color.white;
    }
}