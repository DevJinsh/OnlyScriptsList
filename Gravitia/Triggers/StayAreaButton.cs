using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayAreaButton : MonoBehaviour
{
    public GameObject triggerObject; // 트리거 오브젝트
    public float triggerTime = 3.0f; // 머무르는 시간
    private Color targetColor; // 목표 색상
    private Material material; // 오브젝트의 머터리얼
    private Coroutine triggerCoroutine; // 코루틴 참조 변수

    // 콜라이더와 충돌시 호출되는 함수
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InteractiveObj"))
        {
            triggerCoroutine = StartCoroutine(TriggerDoor());
            //AkSoundEngine.PostEvent("ColorSwitch", gameObject);
        }
    }

    // 콜라이더에서 벗어날 때 호출되는 함수
    private void OnTriggerExit(Collider other)
    {
        // 코루틴 중지
        if (other.CompareTag("InteractiveObj") && triggerCoroutine != null)
        {
            StopCoroutine(triggerCoroutine);
            //AkSoundEngine.PostEvent("ColorSwitchStop", gameObject);
            material.color = Color.red;
        }
    }

    // 초기화
    private void Start()
    {
        // 오브젝트의 Material 컴포넌트 가져오기
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material;
        }
        targetColor = Color.green;
    }

    // 트리거 동작을 관리하는 코루틴
    private IEnumerator TriggerDoor()
    {
        Color startColor = material.color;
        float elapsedTime = 0f;

        while (elapsedTime < triggerTime)
        {
            // 시간에 따라 색상을 보간 (Lerp)하여 변경
            material.color = Color.Lerp(startColor, targetColor, elapsedTime / triggerTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        triggerObject.GetComponentInChildren<TriggerObject>().TriggerActive();
        // 이후 원하는 동작 수행 가능
        // 예를 들어 문 열기, 애니메이션 재생 등
    }
}
