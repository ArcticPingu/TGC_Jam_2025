using UnityEngine;

public class movementTutorial : MonoBehaviour
{
    public GameObject Stage2;
    void Update()
    {
        if (PlayerController.doneWalk)
        {
            transform.position -= new Vector3(Time.deltaTime * 520, 0, 0);

            if (transform.position.x < -1000)
            {
                Stage2.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }

    void Awake()
    {
        if (PlayerController.doneWalk)
        {
            Stage2.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
