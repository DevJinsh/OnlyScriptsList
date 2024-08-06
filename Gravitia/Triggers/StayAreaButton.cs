using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayAreaButton : MonoBehaviour
{
    public GameObject triggerObject; // Ʈ���� ������Ʈ
    public float triggerTime = 3.0f; // �ӹ����� �ð�
    private Color targetColor; // ��ǥ ����
    private Material material; // ������Ʈ�� ���͸���
    private Coroutine triggerCoroutine; // �ڷ�ƾ ���� ����

    // �ݶ��̴��� �浹�� ȣ��Ǵ� �Լ�
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InteractiveObj"))
        {
            triggerCoroutine = StartCoroutine(TriggerDoor());
            //AkSoundEngine.PostEvent("ColorSwitch", gameObject);
        }
    }

    // �ݶ��̴����� ��� �� ȣ��Ǵ� �Լ�
    private void OnTriggerExit(Collider other)
    {
        // �ڷ�ƾ ����
        if (other.CompareTag("InteractiveObj") && triggerCoroutine != null)
        {
            StopCoroutine(triggerCoroutine);
            //AkSoundEngine.PostEvent("ColorSwitchStop", gameObject);
            material.color = Color.red;
        }
    }

    // �ʱ�ȭ
    private void Start()
    {
        // ������Ʈ�� Material ������Ʈ ��������
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material;
        }
        targetColor = Color.green;
    }

    // Ʈ���� ������ �����ϴ� �ڷ�ƾ
    private IEnumerator TriggerDoor()
    {
        Color startColor = material.color;
        float elapsedTime = 0f;

        while (elapsedTime < triggerTime)
        {
            // �ð��� ���� ������ ���� (Lerp)�Ͽ� ����
            material.color = Color.Lerp(startColor, targetColor, elapsedTime / triggerTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        triggerObject.GetComponentInChildren<TriggerObject>().TriggerActive();
        // ���� ���ϴ� ���� ���� ����
        // ���� ��� �� ����, �ִϸ��̼� ��� ��
    }
}
