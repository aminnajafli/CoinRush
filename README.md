# Coin Rush

Tek dokunuşla oynanan, 60 saniyelik bir refleks oyunu: altınları topla, bombalardan kaçın.

## Kurulum

1. Unity uygun sürümünü kurun.
2. Depoyu klonlayın: `git clone https://github.com/aminnajafli/CoinRush.git`
3. Unity Hub > Projects > Add > Add project from disk ile `CoinRush` klasörünü seçin ve açın.
4. `Assets/Scenes/Menu` sahnesini açıp Play'e basın.

## Kontroller

- Klavye: A/D veya sağ-sol ok tuşları
- Fare / dokunmatik: basılı tutup sürükleyin, sepet dokunulan noktaya gider

## Kurallar

Dört para türü (küçük coin +1, büyük coin +3, kağıt para +5, cüzdan +10), meyve
(elma, armut, muz, portakal, üzüm) +3 puan, bomba -1 can (3 can). Tur 60 saniye sürer
veya canlar bitince biter. Süre ilerledikçe nesneler hızlanır, daha sık üretilir ve bomba
oranı artar.

## Mimari

| Sınıf | Görevi |
| --- | --- |
| GameSession | Skor, can, süre ve tur bitişi kuralları; olayları yayınlar |
| BasketController | Girdiyi okur, sepeti hareket ettirir, yakalanan nesneyi olayla bildirir |
| ItemSpawner | Zorluk eğrisine göre nesne üretir, para/meyve/bomba arasında seçim yapıp her prefab için ayrı ObjectPool yönetir |
| FallingItem / KillZone | Düşen nesne ve ekran dışına çıkanı havuza döndüren bölge |
| GameSettings | Zorluk ve tur ayarları (ScriptableObject) |
| HighScoreStorage | PlayerPrefs ile kalıcı en yüksek skor |
| Hud / GameOverScreen / MainMenu | UI, oyun mantığını sadece olaylar üzerinden dinler |

## Tasarım kararları

- Tek GameManager yok; sorumluluklar ayrıldı ve sınıflar olaylarla haberleşiyor.
- Object pooling: `UnityEngine.Pool.ObjectPool<T>`; oyun sırasında Destroy çağrılmıyor.
- Nesneler Rigidbody2D hızıyla düşüyor, düşme için Update kullanılmıyor.
- Zorluk 0-1 arası ilerleme değeriyle `Lerp` edilir: üretim aralığı 1,0 sn -> 0,35 sn, hız 3 -> 8, bomba ihtimali %15 -> %40, meyve ihtimali %30 -> %20.
- Zorluk değerleri koddan ayrı, `GameSettings` dosyasında.
- HUD süre yazısı sadece saniye değişince güncellenir (gereksiz string üretimi yok).

## Varsayımlar

- Kaçırılan altın ceza getirmez.
- Tur bitince ekranda kalan nesneler puan/can değiştirmez.
- Ekran dikey (9:16) hedeflendi.

## Yetiştirilemeyenler / gelecek işler

- UI iyileştirmeleri
- Birim testi

## Kaynaklar

- Görseller: Altın, PowerUp ve bomba — Kenney "Generic Items" paketi (kenney.nl/assets/generic-items). Meyveler — ShortStudios "PIXEL FRUITS" paketi. Sepet — Canva.
- Sesler: kenney.nl, CC0.
- Bu çalışmada yapay zeka desteği kullanıldı.
