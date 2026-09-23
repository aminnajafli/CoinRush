using System;
using UnityEngine;

public enum ItemType
{
    Coin,
    Fruit,
    Bomb
}

[RequireComponent(typeof(Rigidbody2D))]
public class FallingItem : MonoBehaviour
{
    [SerializeField] private ItemType type;
    [SerializeField] private int points = 10;

    private Rigidbody2D body;
    private Action<FallingItem> releaseToPool;
    private bool inUse;

    public ItemType Type => type;
    public int Points => points;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // havuz, nesneyi oluştururken kendi Release fonksiyonunu buraya verir.
    public void Bind(Action<FallingItem> release)
    {
        releaseToPool = release;
    }

    public void Launch(Vector2 position, float speed)
    {
        transform.position = position;
        body.linearVelocity = Vector2.down * speed;
        inUse = true;
    }

    // sepet veya KillZone çağırır. aynı nesne iki kez iade edilemez.
    public void Release()
    {
        if (!inUse)
        {
            return;
        }

        inUse = false;
        body.linearVelocity = Vector2.zero;
        releaseToPool?.Invoke(this);
    }
}