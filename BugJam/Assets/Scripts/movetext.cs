using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movetext : MonoBehaviour
{
    public int numWaitingFor;
    // Start is called before the first frame update
    void Start()
    {
        if (settingsHandler.instance.numPlayers != numWaitingFor)
        {
            gameObject.SetActive(false);
        }
    }
}
