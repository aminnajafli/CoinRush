using UnityEngine;
using UnityEngine.Pool;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameSession session;
    [SerializeField] private GameSettings settings;
    [SerializeField] private FallingItem[] coinPrefabs;
    [SerializeField] private FallingItem bombPrefab;
    [SerializeField] private FallingItem[] fruitPrefabs;
    [SerializeField] private float horizontalPadding = 0.5f;

    private ObjectPool<FallingItem>[] coinPools;
    private ObjectPool<FallingItem> bombPool;
    private ObjectPool<FallingItem>[] fruitPools;
    private float halfWidth;
    private float spawnY;
    private float timer;

    private void Awake()
    {
        Camera cam = Camera.main;
        halfWidth = cam.orthographicSize * cam.aspect - horizontalPadding;
        spawnY = cam.orthographicSize + 1f;

        bombPool = CreatePool(bombPrefab);

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
        else
        {
            pool = coinPools[Random.Range(0, coinPools.Length)];
        }

        FallingItem item = pool.Get();

        float speed = Mathf.Lerp(settings.startFallSpeed, settings.endFallSpeed, progress);
        speed *= Random.Range(0.9f, 1.1f);

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