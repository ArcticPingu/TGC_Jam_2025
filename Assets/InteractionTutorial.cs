using UnityEngine;

public class InteractionTutorial : MonoBehaviour
{
    void Update()
    {
        if (Interacter.doneInteract)
        {
            transform.position -= new Vector3(Time.deltaTime * 520, 0, 0);

            if (transform.position.x < -500)
            {
                gameObject.SetActive(false);
            }
        }
        else if(transform.position.x < 34)
        {
            transform.position += new Vector3(Time.deltaTime * 520, 0, 0);
        }
    }
}
