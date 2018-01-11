using System;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IComparable<Entity> {

    [SerializeField]
    private EntityType type;

    public void TryDoAction(Player player) {
        if (IsActionPossible(player)) {
            DoAction(player);
        } else {
            OnUnsuccessfulAction();
        }
    }

    protected abstract bool IsActionPossible(Player player);

    protected abstract void DoAction(Player player);

    private void OnUnsuccessfulAction() {
    }

    public int CompareTo(Entity other) {
        return this.type.CompareTo(other.type);
    }
}