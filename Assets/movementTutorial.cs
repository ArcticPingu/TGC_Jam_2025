using UnityEngine;

public class movementTutorial : MonoBehaviour
{
    public GameObject Stage2;
    void Update()
    {
        if (PlayerController.doneWalk)
        {
            transform.position -= new Vector3(Time.deltaTime * 320, 0, 0);

            if (transform.position.x < -500)
            {
                Stage2.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }
}
