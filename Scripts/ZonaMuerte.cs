using UnityEngine;

public class ZonaMuerte : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("El player se cayo q nub");
        collision.GetComponent<Player>().RecibeDanio(Vector2.zero, 99);
    }
}
