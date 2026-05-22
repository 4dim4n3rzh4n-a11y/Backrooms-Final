using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private GameObject interactUI; 

    void Start()
    {
        if (interactUI != null) interactUI.SetActive(false);
    }

    void Update()
    {
        if (Time.timeScale == 0f)
        {
            if (interactUI != null && interactUI.activeSelf)
            {
                interactUI.SetActive(false);
            }
            return; 
        }

        RaycastCheck();
    }

    private void RaycastCheck()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            if (hit.collider.CompareTag("Door"))
            {
                Door door = hit.collider.GetComponent<Door>() ?? hit.collider.GetComponentInParent<Door>();
                
                if (door != null)
                {
                    interactUI.SetActive(true); 

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        Debug.Log("🎯 Игрок нажал E глядя на дверь!");
                        door.OpenDoor();
                        interactUI.SetActive(false); 
                    }
                    return;
                }
            }
        }

        if (interactUI != null && interactUI.activeSelf)
        {
            interactUI.SetActive(false);
        }
    }
}