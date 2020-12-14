using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsDetection : MonoBehaviour
{
    private void OnEnable()
    {
        ManomotionManager.OnManoMotionFrameProcessed += HandleManoMotionFrameProcessed;
    }

    #region Instruction Rules
    //Set of rules that my Instructions will follow.
    // 1. The Hand is Detected.
    // 2. The hand is not too close to the Screen.
    // 3. Steps 1 and 2 should be consistent for 60 Frames.
    // 4. Show the temporal 

    [SerializeField]
    const int maxGoodFrames = 60;
    [SerializeField]
    const float maxAcceptedBoundingBoxWidth = 0.9f;
    private int currentGoodFrames = 0;

    #endregion

    #region Components
    [SerializeField]
    Image radialProgressImage;

    #endregion

    void HandleManoMotionFrameProcessed()
    {
        HandInfo currentHandInfo = ManomotionManager.Instance.Hand_infos[0].hand_info;

        //This means that there is a Hand Detection
        if (currentHandInfo.gesture_info.mano_class != ManoClass.NO_HAND)
        {
            //This means that the hand is not so close 
            if (currentHandInfo.tracking_info.bounding_box.width <= maxAcceptedBoundingBoxWidth)
            {
                IncreaseGoodFrames();
            }

        }
        else
        {
            //Decide if I want to Decrease Good frames and visually update the progress.
        }

        if (currentGoodFrames == maxGoodFrames)
        {
            ValidateSuccess();
        }
    }

    private void IncreaseGoodFrames()
    {
        if (currentGoodFrames < maxGoodFrames)
        {
            currentGoodFrames++;
            radialProgressImage.fillAmount = (float)currentGoodFrames / maxGoodFrames;
        }

    }

    private void DecreaseGoodFrames()
    {
        if (currentGoodFrames > 0)
        {
            currentGoodFrames--;
            radialProgressImage.fillAmount = (float)currentGoodFrames / maxGoodFrames;
        }

    }

    private void ValidateSuccess()
    {
        Handheld.Vibrate();
        ManomotionManager.OnManoMotionFrameProcessed -= HandleManoMotionFrameProcessed;
        this.transform.gameObject.SetActive(false);

    }


}
