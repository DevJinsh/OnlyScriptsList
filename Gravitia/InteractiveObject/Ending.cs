using Michsky.UI.Heat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera changeMainCamera;
    public CreditsManager credit;
    public GameObject endingLight;
    public GameObject level28;
    public GameObject level29;
    public GameObject levelLight;
    private bool canSkipSet = true;

    private void OnCollisionStay(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            if ((collision.contacts[0].point - transform.position).magnitude <= 0.35f)
            {
                GetComponentInParent<Animator>().SetTrigger("Ending");
                endingLight.SetActive(false);
                level28.SetActive(false);
                level29.SetActive(false);
                levelLight.SetActive(false);

                ChangePlayerGazePos changePlayerGaze = GetComponent<ChangePlayerGazePos>();
                changePlayerGaze.gazeTarget.transform.position = changePlayerGaze.pos;
                StartCoroutine(changePlayerGaze.AdjustYCouroutine(0f, changePlayerGaze.pos.y, 8f));

                GameManager.Instance.StopPlayerMove();
                CameraManager.Instance.ChangeMainCamera(changeMainCamera);
                collision.collider.transform.SetParent(transform);
                collision.collider.GetComponent<Rigidbody>().useGravity = false;

                if (canSkipSet)
                {
                    GameManager.Instance.animationManager.SetCanSkipTrue();
                    GameManager.Instance.animationManager.SetSkipAnimator(GetComponentInParent<Animator>());
                    GameManager.Instance.animationManager.SetPlayingAnimName("Ending_Start");
                    GameManager.Instance.animationManager.SetSkipStartTime(25f);
                    canSkipSet = false;
                }
            }
        }
    }

    public void ShowCredit()
    {
        transform.parent.gameObject.SetActive(false);
        credit.OpenPanel();
    }
}
