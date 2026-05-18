using UnityEngine;

public class TreeHit : MonoBehaviour
{
    public bool touched = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("TRIGGER con: " + other.name);

        if (other.GetComponent<EdgeCollider2D>() != null)
        {
            touched = true;
            Debug.Log("ÁRBOL TOCADO: " + gameObject.name);

            GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}