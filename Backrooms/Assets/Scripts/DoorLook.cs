using UnityEngine;

public class DoorLook : MonoBehaviour
{
    public float distance = 3f;
    public GameObject pressEText;
    public Door door;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance))
        {
            if (hit.collider.CompareTag("Door"))
            {
                pressEText.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    door.OpenDoor();
                }
            }
            else
            {
                pressEText.SetActive(false);
            }
        }
        else
        {
            pressEText.SetActive(false);
        }
    }
}