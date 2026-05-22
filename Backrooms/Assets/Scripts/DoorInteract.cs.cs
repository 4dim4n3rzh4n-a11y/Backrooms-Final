using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    public float distance = 3f;
    public GameObject interactUI;
    private Door currentDoor;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance))
        {
            Door door = hit.collider.GetComponent<Door>();

            if (door != null)
            {
                interactUI.SetActive(true);
                currentDoor = door;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    currentDoor.OpenDoor();
                }

                return;
            }
        }

        interactUI.SetActive(false);
        currentDoor = null;
    }
}