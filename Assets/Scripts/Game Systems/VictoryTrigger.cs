using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VictoryTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    public float targetSize = 1;
    public float zoomSpeed = 1;

    int lockId = 3;
    int winId = 29;

    private Vector2 victoryPos = new Vector2(4.5f, -15.35f);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            int show = -1;
            bool win = false;
            if (VictoryTracker.Instance.lockCount <= 0)
            {
                show = winId;
                win = true;
                collision.GetComponent<PlayerPlatformerController>().DoWin(victoryPos);

                StartCoroutine("Zoom");
            }
            else
                show = lockId;
            
            DialogueCreator.Instance.InitDialogue(show, win);
        }
    }

    IEnumerator Zoom()
    {
        var transposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        transposer.m_DeadZoneWidth = transposer.m_DeadZoneHeight = transposer.m_LookaheadTime = 0;
        vcam.transform.position = victoryPos;

        while (vcam.m_Lens.OrthographicSize > targetSize)
        {
            vcam.m_Lens.OrthographicSize -= Time.deltaTime * zoomSpeed;
            yield return null;
        }

        vcam.m_Lens.OrthographicSize = 1;
    }
}
