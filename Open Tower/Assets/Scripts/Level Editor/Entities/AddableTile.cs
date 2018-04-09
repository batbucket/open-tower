using Scripts.LevelEditor.Serialization;
using System.Collections.Generic;
using System.Linq;
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

    private static AddableTile currentPickingSprite;
    private static EntitiesPanel _parentPanel;
    private static FloorPanel _floorPanel;

    [SerializeField]
    private Outline outline;

    [SerializeField]
    private TileType tile;

    [SerializeField]
    private PlaceType placementType;

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
    private Button[] boosterStatChooserButtons;

    [SerializeField]
    private Element elementPrefab;

    private StatType boostedStatType;

    private int spriteID;

    public bool IsStaticTileType {
        get {
            return SerializationUtil.IsStaticType(this.TileType);
        }
    }

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

    public int EnemyLife {
        get {
            Util.Assert(tile == TileType.ENEMY, "Expected Enemy type, was {0} instead.", this.tile);
            return int.Parse(life.text);
        }
    }

    public int EnemyPower {
        get {
            Util.Assert(tile == TileType.ENEMY, "Expected Enemy type, was {0} instead.", this.tile);
            return int.Parse(power.text);
        }
    }

    public int EnemyDefense {
        get {
            Util.Assert(tile == TileType.ENEMY, "Expected Enemy type, was {0} instead.", this.tile);
            return int.Parse(defense.text);
        }
    }

    public int EnemyStars {
        get {
            Util.Assert(tile == TileType.ENEMY, "Expected Enemy type, was {0} instead.", this.tile);
            return int.Parse(experience.text);
        }
    }

    public StatType BoostedStatType {
        get {
            Util.Assert(tile == TileType.BOOSTER, "Expected Booster type, was {0} instead.", this.tile);
            return boostedStatType;
        }
    }

    public int BoostedAmount {
        get {
            Util.Assert(tile == TileType.BOOSTER, "Expected Booster type, was {0} instead.", this.tile);
            return int.Parse(boostedAmount.text);
        }
        set {
            boostedAmount.text = value.ToString();
        }
    }

    public int SpriteID {
        get {
            Util.Assert(!IsStaticTileType, "Tile type is not dynamic.");
            return spriteID;
        }
    }

    private static EntitiesPanel ParentPanel {
        get {
            if (_parentPanel == null) {
                _parentPanel = EntitiesPanel.Instance;
            }
            return _parentPanel;
        }
    }

    private static FloorPanel FloorPanel {
        get {
            if (_floorPanel == null) {
                _floorPanel = FloorPanel.Instance;
            }
            return _floorPanel;
        }
    }

    private IEnumerable<Element> AllAssociatedElementsInLevel {
        get {
            return FloorPanel.FloorParent.GetComponentsInChildren<Element>(true).Where(e => e.IsSource(this));
        }
    }

    private IEnumerable<Element> AllAssociatedElementsInFloor {
        get {
            return FloorPanel.Selected.Associated.GetComponentsInChildren<Element>(true).Where(e => e.IsSource(this));
        }
    }

    public void InitEnemy(int life, int power, int defense, int stars) {
        Util.Assert(this.TileType == TileType.ENEMY);
        this.life.text = life.ToString();
        this.power.text = power.ToString();
        this.defense.text = defense.ToString();
        this.experience.text = stars.ToString();
    }

    public void SetSprite(int id, AddableType type) {
        Util.Assert(type == AddableType.BOOSTER || type == AddableType.ENEMY);
        Sprite sprite = null;
        if (type == AddableType.BOOSTER) {
            sprite = SpriteList.GetBooster(id);
        } else { // == Enemy
            sprite = SpriteList.GetEnemy(id);
        }
        this.image.sprite = sprite;
        this.spriteID = id;
    }

    public void ChooseBoostedStat(StatType stat) {
        ChooseBoostedStat((int)stat);
    }

    public void ChooseBoostedStat(int index) {
        boosterStatIcon.sprite = boosterStatIcons[index];
        boostedStatType = MAPPING[index];
        for (int i = 0; i < boosterStatChooserButtons.Length; i++) {
            Button button = boosterStatChooserButtons[i];
            if (i == index) {
                button.interactable = false;
            } else {
                button.interactable = true;
            }
        }
    }

    public void Delete() {
        Destroy(gameObject);
        if (EntitiesPanel.Instance.LastSelected == this) {
            EntitiesPanel.Instance.LastSelected = null;
        }
        foreach (Element e in AllAssociatedElementsInLevel) {
            Destroy(e.gameObject);
        }
        if (currentPickingSprite == this) {
            SpritePicker.Instance.Deactivate();
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
        switch (placementType) {
            case PlaceType.NO_RESTRICTION:
                // do nothing
                break;

            case PlaceType.UNIQUE_PER_FLOOR:
                foreach (Element e in AllAssociatedElementsInFloor) {
                    Destroy(e.gameObject);
                }
                break;

            case PlaceType.UNIQUE_PER_LEVEL:
                foreach (Element e in AllAssociatedElementsInLevel) {
                    Destroy(e.gameObject);
                }
                break;
        }
        Element newElement = Instantiate(elementPrefab, parent);
        newElement.Init(this);
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
        if (this.TileType == TileType.BOOSTER) {
            this.image.sprite = SpriteList.GetBooster(spriteID);
        } else if (this.TileType == TileType.ENEMY) {
            this.image.sprite = SpriteList.GetEnemy(spriteID);
        }
        if (changeSprite != null) {
            changeSprite.onClick.AddListener(new UnityEngine.Events.UnityAction(
                () => {
                    SelectThisTile();
                    if (!SpritePicker.Instance.IsOpen || currentPickingSprite != this) {
                        currentPickingSprite = this;
                        SpritePicker.Instance.Activate(this, this.TileType, pickable => {
                            image.sprite = pickable.Sprite;
                            spriteID = pickable.ID;
                            foreach (Element e in AllAssociatedElementsInLevel) {
                                e.Sprite = pickable.Sprite;
                            }
                        });
                    } else {
                        SpritePicker.Instance.Deactivate();
                        currentPickingSprite = null;
                    }
                }
                ));
        }
    }

    private void DeleteAllPlacedElementsInLevel() {
        foreach (Element e in AllAssociatedElementsInLevel) {
            if (e.IsSource(this)) {
                Destroy(e.gameObject);
            }
        }
    }

    private void DeleteAllPlacedElementsInCurrentFloor() {
        foreach (Element e in AllAssociatedElementsInFloor) {
            if (e.IsSource(this)) {
                Destroy(e.gameObject);
            }
        }
    }

    private void Update() {
        outline.effectColor = (ParentPanel.LastSelected == this) ? Color.green : Color.white;
    }
}