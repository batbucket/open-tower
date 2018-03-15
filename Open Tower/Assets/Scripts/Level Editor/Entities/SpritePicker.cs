using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpritePicker : MonoBehaviour {
    private static SpritePicker _instance;

    [SerializeField]
    private Transform enemyHolder;

    [SerializeField]
    private Transform boosterHolder;

    [SerializeField]
    private PickableSprite pickableSpritePrefab;

    private IList<PickableSprite> enemyPicks;

    private IList<PickableSprite> boosterPicks;

    private IDictionary<int, PickableSprite> spriteIDs;

    public static SpritePicker Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<SpritePicker>();
            }
            return _instance;
        }
    }

    public void Activate(TileType type, Action<PickableSprite> callback) {
        boosterHolder.gameObject.SetActive(false);
        enemyHolder.gameObject.SetActive(false);
        switch (type) {
            case TileType.BOOSTER:
                boosterHolder.gameObject.SetActive(true);
                foreach (PickableSprite sprite in boosterPicks) {
                    sprite.SetButtonAction(() => {
                        callback(sprite);
                        boosterHolder.gameObject.SetActive(false);
                    });
                }
                break;

            case TileType.ENEMY:
                enemyHolder.gameObject.SetActive(true);
                foreach (PickableSprite sprite in enemyPicks) {
                    sprite.SetButtonAction(() => {
                        callback(sprite);
                        enemyHolder.gameObject.SetActive(false);
                    });
                }
                break;
        }
    }

    private void Start() {
        enemyPicks = new List<PickableSprite>();
        foreach (KeyValuePair<int, Sprite> pair in SpriteList.GetEnemyIDs()) {
            PickableSprite ps = Instantiate(pickableSpritePrefab, enemyHolder);
            ps.Init(pair.Key, pair.Value);
            enemyPicks.Add(ps);
        }
        boosterPicks = new List<PickableSprite>();
        foreach (KeyValuePair<int, Sprite> pair in SpriteList.GetBoosterIDs()) {
            PickableSprite ps = Instantiate(pickableSpritePrefab, boosterHolder);
            ps.Init(pair.Key, pair.Value);
            boosterPicks.Add(ps);
        }
    }
}