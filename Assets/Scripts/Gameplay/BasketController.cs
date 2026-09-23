using System;
using UnityEngine;

public class BasketController : MonoBehaviour
{
    [SerializeField] private float keyboardSpeed = 10f;
    [SerializeField] private float horizontalPadding = 1f;
    [SerializeField] private float bottomMargin = 1.2f;

    public event Action<FallingItem> ItemCaught;
    public bool ControlEnabled { get; set; } = true;

    private Camera cam;
    private float minX;
    private float maxX;

    private void Awake()
    {
        cam = Camera.main;

        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        minX = -halfWidth + horizontalPadding;
        maxX = halfWidth - horizontalPadding;

        transform.position = new Vector3(0f, -halfHeight + bottomMargin, 0f);
    }

    private void Update()
    {
        if (!ControlEnabled)
        {
            return;
        }

        float x = transform.position.x;

        if (Input.GetMouseButton(0))
        {
            // fare veya parmak: sepet, dokunulan noktaya gider.
            x = cam.ScreenToWorldPoint(Input.mousePosition).x;
        }
        else
        {
            // klavye: A/D ve ok tuşları "Horizontal" ekseninde tanımlıdır.
            x += Input.GetAxisRaw("Horizontal") * keyboardSpeed * Time.deltaTime;
        }

        x = Mathf.Clamp(x, minX, maxX);
        transform.position = new Vector3(x, transform.position.y, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out FallingItem item))
        {
            ItemCaught?.Invoke(item);
            item.Release();
        }
    }
}