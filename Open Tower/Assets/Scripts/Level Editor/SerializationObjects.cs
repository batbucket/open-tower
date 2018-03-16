using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.LevelEditor.Serialization {

    public enum AddableType {
        NONE,
        STATIC,
        BOOSTER,
        ENEMY
    }

    [Serializable]
    public struct Upload {
        public string LevelJson;
        public string LevelName;
        public int AuthorID;
        public string DateCreated;
        public List<int> AttemptedUserIds; // gamejolt ids
        public List<int> CompletedUserIds; // gamejolt ids
        public List<Score> Leaderboards;

        public Upload(string levelJson, string levelName, int authorID, string dateCreated) : this() {
            LevelJson = levelJson;
            LevelName = levelName;
            AuthorID = authorID;
            DateCreated = dateCreated;
            AttemptedUserIds = new List<int>();
            CompletedUserIds = new List<int>();
            Leaderboards = new List<Score>();
        }

        public void AddToAttempt(int userID) {
            if (userID != AuthorID) {
                AttemptedUserIds.Add(userID);
            }
        }

        public void AddToComplete(int userID) {
            if (userID != AuthorID) {
                CompletedUserIds.Add(userID);
            }
        }

        public void AddToLeaderboard(GameJolt.API.Objects.User currentUser, int stepCount, DateTime dateCompleted, out int calculatedRank, out Score previousScore, out int previousRank, Action<bool> onSet) {
            Score scoreToAdd = new Score(stepCount, dateCompleted.ToString(), currentUser.Name, currentUser.ID);
            bool isFindRanking = false;
            bool isUserAuthor = (currentUser.ID == this.AuthorID);
            previousScore = null;
            calculatedRank = -1;

            // Update successful attempts
            AddToComplete(currentUser.ID);

            // Leaderboard updating

            // Find existing score by user
            previousRank = this.Leaderboards.FindIndex(score => score.UserID == currentUser.ID);
            if (previousRank >= 0) {
                previousScore = this.Leaderboards[previousRank];
            }

            for (int i = 0; i < this.Leaderboards.Count + 1 && !isFindRanking; i++) {
                Score current = null;
                if (i < this.Leaderboards.Count) {
                    current = this.Leaderboards[i];
                }

                if (current == null || scoreToAdd.CompareTo(current) < 0) {
                    if (!isUserAuthor && (previousScore == null || previousScore.CompareTo(scoreToAdd) > 0)) {
                        this.Leaderboards.Insert(i, scoreToAdd);
                        if (previousScore != null) {
                            this.Leaderboards.Remove(previousScore);
                        }
                    }
                    calculatedRank = i;
                    isFindRanking = true;
                }
            }
            Util.Assert(isFindRanking, "Ranking not found.");

            if (!isUserAuthor) {
                UpdateDataStore(onSet);
            }
        }

        public void UpdateDataStore(Action<bool> onSet = null) {
            GameJolt.API.DataStore.Set(this.LevelName, JsonUtility.ToJson(this), true, isSuccess => {
                if (onSet != null) {
                    onSet(isSuccess);
                }
            });
        }
    }

    [Serializable]
    public class Score : IComparable<Score> {
        public int Steps;
        public string DateAchieved;
        public string Username;
        public int UserID;

        public Score(int steps, string dateAchieved, string user, int userID) {
            Steps = steps;
            DateAchieved = dateAchieved;
            Username = user;
            UserID = userID;
        }

        public int CompareTo(Score other) {
            DateTime otherDate = DateTime.Parse(other.DateAchieved);
            DateTime ourDate = DateTime.Parse(this.DateAchieved);
            int stepComparison = this.Steps.CompareTo(other.Steps);
            Debug.Log("step comparison: " + stepComparison);
            if (stepComparison == 0) {
                return ourDate.CompareTo(otherDate);
            }
            return stepComparison;
        }
    }

    [Serializable]
    public struct Dungeon {
        public Addable[] Addables;
        public Floor[] Floors;
        public StartingValues StartingValues;

        public Dungeon(Addable[] addables, Floor[] floors, StartingValues startingValues) {
            Addables = addables;
            Floors = floors;
            StartingValues = startingValues;
        }
    }

    [Serializable]
    public struct Floor {
        public int[] Indices; // array of addable array indices

        public Floor(int[] indices) {
            Indices = indices;
        }
    }

    [Serializable]
    public struct StartingValues {
        public int Life;
        public int Power;
        public int Defense;
        public int Stars;
        public int GoldKeys;
        public int BlueKeys;
        public int RedKeys;

        public StartingValues(int life, int power, int defense, int stars, int goldKeys, int blueKeys, int redKeys) {
            Life = life;
            Power = power;
            Defense = defense;
            Stars = stars;
            GoldKeys = goldKeys;
            BlueKeys = blueKeys;
            RedKeys = redKeys;
        }
    }

    [Serializable]
    public class StaticData {
        public TileType TileType;

        public StaticData(TileType tileType) {
            TileType = tileType;
        }
    }

    [Serializable]
    public class BoosterData {
        public int SpriteID;
        public StatType StatToBoost;
        public int AmountBoosted;

        public BoosterData(int spriteID, StatType statToBoost, int amountBoosted) {
            SpriteID = spriteID;
            StatToBoost = statToBoost;
            AmountBoosted = amountBoosted;
        }
    }

    [Serializable]
    public class EnemyData {
        public int SpriteID;
        public int Life;
        public int Power;
        public int Defense;
        public int Stars;

        public EnemyData(int spriteID, int life, int power, int defense, int stars) {
            SpriteID = spriteID;
            Life = life;
            Power = power;
            Defense = defense;
            Stars = stars;
        }
    }

    [Serializable]
    public class Addable {
        public AddableType AddableType;

        public StaticData _staticData;
        public BoosterData _boosterData;
        public EnemyData _enemyData;

        public StaticData StaticData {
            get {
                Util.Assert(AddableType == AddableType.STATIC, "Expected {0}, got {1} instead", AddableType.STATIC, this.AddableType);
                return _staticData;
            }
        }

        public BoosterData BoosterData {
            get {
                Util.Assert(AddableType == AddableType.BOOSTER, "Expected {0}, got {1} instead", AddableType.STATIC, this.AddableType);
                return _boosterData;
            }
        }

        public EnemyData EnemyData {
            get {
                Util.Assert(AddableType == AddableType.ENEMY, "Expected {0}, got {1} instead", AddableType.STATIC, this.AddableType);
                return _enemyData;
            }
        }

        public Addable(StaticData staticData) {
            AddableType = AddableType.STATIC;
            _staticData = staticData;
        }

        public Addable(BoosterData boosterData) {
            AddableType = AddableType.BOOSTER;
            _boosterData = boosterData;
        }

        public Addable(EnemyData enemyData) {
            AddableType = AddableType.ENEMY;
            _enemyData = enemyData;
        }
    }
}