using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public float maxObjectCount;
    public GameObject elevatorSignLights;
    private float _objCount = 0;
    private bool _isChange = false;
    public float moveTime;
    public Transform door;

    private Vector3 _originStartPos;
    private Vector3 _originEndPos;

    private float _floorDistance;
    private bool _isPlayerIn = false;

    private Coroutine _coroutine;
    private Coroutine _timeCoroutine;

    private BoxCollider _leftDoor;
    private BoxCollider _rightDoor;
    private void Start()
    {
        _originStartPos = transform.position;
        _originEndPos = transform.GetChild(0).position;
        if (door != null)
        {
            _leftDoor = door.GetChild(0)?.GetComponent<BoxCollider>();
            _rightDoor = door.GetChild(1)?.GetComponent<BoxCollider>();
        }
        _floorDistance = (_originStartPos - _originEndPos).magnitude / maxObjectCount;
    }

    private void Update()
    {
        if (_isChange)
        {
            if (_coroutine == null)
            {
                if (_leftDoor != null) _leftDoor.isTrigger = false;
                if (_rightDoor != null) _rightDoor.isTrigger = false;
                _coroutine = StartCoroutine(Moving(_originStartPos + Vector3.down * _floorDistance * _objCount));
                _isChange = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.gameObject.layer == LayerMask.NameToLayer("InteractiveObject"))
        {
            if (_timeCoroutine != null) StopCoroutine(_timeCoroutine);
            _timeCoroutine = StartCoroutine(Counting(other, true));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.gameObject.layer == LayerMask.NameToLayer("InteractiveObject"))
        {
            if (_timeCoroutine != null) StopCoroutine(_timeCoroutine);
            _objCount--;
            if (other.CompareTag("Player")) _isPlayerIn = false;
            _isChange = true;
            ChangeLight();
        }
    }
    private void ChangeLight()
    {
        for (int i = 0; i < elevatorSignLights.transform.childCount; i++)
        {
            if (i < _objCount)
            {
                elevatorSignLights.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
            }

            else
            {
                elevatorSignLights.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
                SoundManager.instance.SoundPlay("ElevatorOff", gameObject);
            }
        }
    }
    IEnumerator DoorInteraction(Vector3 endPos)
    {
        float currentTime = 0f;
        float doorSpeed = 0.5f;
        Vector3 startPos = door.localPosition;
        while (door.localPosition != endPos)
        {
            yield return null;
            currentTime += Time.deltaTime;
            door.localPosition = Vector3.Lerp(startPos, endPos, currentTime / doorSpeed);
        }
    }
    IEnumerator Moving(Vector3 endPos)
    {
        if (door != null)
            yield return StartCoroutine(DoorInteraction(Vector3.up * -1.2f + Vector3.forward * 1f));
        float currentTime = 0f;
        Vector3 startPos = transform.position;
        while (currentTime < moveTime)
        {
            yield return null;
            transform.position = Vector3.Lerp(startPos, endPos, currentTime / moveTime);
            if (_isPlayerIn && (endPos - startPos).y < 0)
            {
                GameManager.Instance.playerMovementManager.transform.position += Time.deltaTime * (Vector3.up * (endPos - startPos).y / moveTime);
            }
            currentTime += Time.deltaTime;
        }

        if (door != null) yield return StartCoroutine(DoorInteraction(Vector3.up * -1.2f + Vector3.forward * 3f));
        if (_leftDoor != null) _leftDoor.isTrigger = true;
        if (_leftDoor != null) _rightDoor.isTrigger = true;
        _coroutine = null;
    }
    IEnumerator Counting(Collider other, bool isEnter)
    {
        float current_Time = 0f;
        if (isEnter)
        {
            _objCount++;
            SoundManager.instance.SoundPlay("ElevatorOn", gameObject);
            if (other.CompareTag("Player")) _isPlayerIn = true;
        }
        ChangeLight();
        while (current_Time < 1f)
        {
            yield return null;
            current_Time += Time.deltaTime;
        }
        _isChange = true;
    }
}
