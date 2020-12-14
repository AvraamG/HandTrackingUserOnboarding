using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class InteractableCubeInstractions : MonoBehaviour
{
    private void OnEnable()
    {
        meshRenderer = this.GetComponent<MeshRenderer>();
        ManomotionManager.OnManoMotionFrameProcessed += HandleManoMotionFrameProcessed;
    }

    #region Instruction Rules
    //Set of rules that my Instructions will follow.
    // 1. -----  Creative challenge  ---- The users should have the intention of interaction with this cube
    // 2. The detecteed trigger is the one I am looking for (example Click). 
    // 3. Steps (1)* - 2 should happen 3 times.
    // 4. After step 3 is validated Hide the gesture Animator and info text.


    [SerializeField]
    const int maxInteractionTimes = 3;
    [SerializeField]
    ManoGestureTrigger interactionTrigger = ManoGestureTrigger.CLICK;
    private int currentInteractionCounter = 0;

    #endregion

    #region Components
    [SerializeField]
    GameObject gestureAnimation;
    [SerializeField]
    GameObject infoText;

    private MeshRenderer meshRenderer;
    #endregion



    void HandleManoMotionFrameProcessed()
    {
        HandInfo currentHandInfo = ManomotionManager.Instance.Hand_infos[0].hand_info;

        //This means that the current trigger gesture 
        if (currentHandInfo.gesture_info.mano_gesture_trigger == interactionTrigger)
        {

            if (currentInteractionCounter < maxInteractionTimes)
            {
                currentInteractionCounter++;

            }
            else
            {
                gestureAnimation.gameObject.SetActive(false);
                infoText.gameObject.SetActive(false);
                Handheld.Vibrate();
            }


            ChangeColor();
        }

    }

    void ChangeColor()
    {
        Color newColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        meshRenderer.material.color = newColor;

    }
}
