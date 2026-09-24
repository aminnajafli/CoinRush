using UnityEngine;
using UnityEngine.Pool;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameSession session;
    [SerializeField] private GameSettings settings;
    [SerializeField] private BasketController basket;
    [SerializeField] private FallingItem[] coinPrefabs;
    [SerializeField] private FallingItem bombPrefab;
    [SerializeField] private FallingItem[] fruitPrefabs;
    [SerializeField] private FallingItem powerUpPrefab;
    [SerializeField] private float horizontalPadding = 0.5f;

    private ObjectPool<FallingItem>[] coinPools;
    private ObjectPool<FallingItem> bombPool;
    private ObjectPool<FallingItem>[] fruitPools;
    private ObjectPool<FallingItem> powerUpPool;
    private float halfWidth;
    private float spawnY;
    private float timer;
    private float slowUntilTime;

    private void Awake()
    {
        Camera cam = Camera.main;
        halfWidth = cam.orthographicSize * cam.aspect - horizontalPadding;
        spawnY = cam.orthographicSize + 1f;

        bombPool = CreatePool(bombPrefab);
        powerUpPool = CreatePool(powerUpPrefab);

        coinPools = new ObjectPool<FallingItem>[coinPrefabs.Length];
        for (int i = 0; i < coinPrefabs.Length; i++)
        {
            coinPools[i] = CreatePool(coinPrefabs[i]);
        }

        fruitPools = new ObjectPool<FallingItem>[fruitPrefabs.Length];
        for (int i = 0; i < fruitPrefabs.Length; i++)
        {
            fruitPools[i] = CreatePool(fruitPrefabs[i]);
        }
    }

    private void OnEnable()
    {
        basket.ItemCaught += HandleItemCaught;
    }

    private void OnDisable()
    {
        basket.ItemCaught -= HandleItemCaught;
    }

    // Puan/can bu sınıfı ilgilendirmiyor (onu GameSession yönetiyor); sadece
    // power-up'ın süreye bağlı yavaşlatma etkisini burada tetikliyoruz.
    private void HandleItemCaught(FallingItem item)
    {
        if (item.Type != ItemType.PowerUp)
        {
            return;
        }

        slowUntilTime = Time.time + settings.powerUpSlowDuration;
    }

    private void Update()
    {
        if (!session.IsPlaying)
        {
            return;
        }

        timer -= Time.deltaTime;

        if (timer > 0f)
        {
            return;
        }

        float progress = session.Progress;
        Spawn(progress);

        float interval = Mathf.Lerp(settings.startSpawnInterval, settings.endSpawnInterval, progress);
        timer = interval * Random.Range(0.7f, 1.3f);
    }

    private void Spawn(float progress)
    {
        float bombChance = Mathf.Lerp(settings.startBombChance, settings.endBombChance, progress);
        float fruitChance = Mathf.Lerp(settings.startFruitChance, settings.endFruitChance, progress);
        float powerUpChance = settings.powerUpChance;

        float roll = Random.value;
        ObjectPool<FallingItem> pool;

        if (roll < bombChance)
        {
            pool = bombPool;
        }
        else if (roll < bombChance + fruitChance)
        {
            pool = fruitPools[Random.Range(0, fruitPools.Length)];
        }
        else if (roll < bombChance + fruitChance + powerUpChance)
        {
            pool = powerUpPool;
        }
        else
        {
            pool = coinPools[Random.Range(0, coinPools.Length)];
        }

        FallingItem item = pool.Get();

        float speed = Mathf.Lerp(settings.startFallSpeed, settings.endFallSpeed, progress);
        speed *= Random.Range(0.9f, 1.1f);

        if (Time.time < slowUntilTime)
        {
            speed *= settings.powerUpSlowMultiplier;
        }

        Vector2 position = new Vector2(Random.Range(-halfWidth, halfWidth), spawnY);
        item.Launch(position, speed);
    }

    private ObjectPool<FallingItem> CreatePool(FallingItem prefab)
    {
        ObjectPool<FallingItem> pool = null;

        pool = new ObjectPool<FallingItem>(
            createFunc: () =>
            {
                FallingItem item = Instantiate(prefab, transform);
                item.Bind(pool.Release);
                return item;
            },
            actionOnGet: item => item.gameObject.SetActive(true),
            actionOnRelease: item => item.gameObject.SetActive(false),
            actionOnDestroy: item => Destroy(item.gameObject),
            collectionCheck: false,
            defaultCapacity: 8,
            maxSize: 32);

        return pool;
    }
}