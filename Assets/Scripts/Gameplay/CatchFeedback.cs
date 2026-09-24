using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CatchFeedback : MonoBehaviour
{
    [SerializeField] private BasketController basket;
    [SerializeField] private AudioClip coinClip;
    [SerializeField] private AudioClip fruitClip;
    [SerializeField] private AudioClip bombClip;
    [SerializeField] private ParticleSystem coinBurst;
    [SerializeField] private ParticleSystem bombBurst;

    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        basket.ItemCaught += OnItemCaught;
    }

    private void OnDisable()
    {
        basket.ItemCaught -= OnItemCaught;
    }

    private void OnItemCaught(FallingItem item)
    {
        switch (item.Type)
        {
            case ItemType.Coin:
                source.PlayOneShot(coinClip);
                coinBurst.Play();
                return;

            case ItemType.Fruit:
                source.PlayOneShot(fruitClip);
                coinBurst.Play(); // ayrı bir meyve parçacığı yok, coin'inkini paylaşıyor
                return;

            case ItemType.Bomb:
                source.PlayOneShot(bombClip);
                bombBurst.Play();
                return;
        }
    }
}