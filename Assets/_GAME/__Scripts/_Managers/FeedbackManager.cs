using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : BaseFeedbackManager
{
    public static FeedbackManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

}
