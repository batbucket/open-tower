using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpritePicker : MonoBehaviour {
    private static SpritePicker _instance;

    [SerializeField]
    private Sprite[] enemies;

    [SerializeField]
    private Sprite[] boosters;

    [SerializeField]
    private Transform enemyHolder;

    [SerializeField]
    private Transform boosterHolder;

    [SerializeField]
    private PickableSprite pickableSpritePrefab;

    private IList<PickableSprite> enemyPicks;

    private IList<PickableSprite> boosterPicks;

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
        int index = 0;
        foreach (Sprite sprite in enemies) {
            PickableSprite ps = Instantiate(pickableSpritePrefab, enemyHolder);
            ps.Init(index++, sprite);
            enemyPicks.Add(ps);
        }
        boosterPicks = new List<PickableSprite>();
        foreach (Sprite sprite in boosters) {
            PickableSprite ps = Instantiate(pickableSpritePrefab, boosterHolder);
            ps.Init(index++, sprite);
            boosterPicks.Add(ps);
        }
    }
}