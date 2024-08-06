using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownObject : MonoBehaviour
{
    public GameObject connectedObject;
    public float minY;
    public float maxY;
    Rigidbody _connectedObjectRigidbody;
    Transform _connectedObjectTransform;
    TagContact _connectedObjectTagContact;
    Rigidbody _rb;
    TagContact _tagContact;
    float _originY;

    private void Awake()
    {
        _originY = (minY + maxY) / 2f;
        _rb = GetComponent<Rigidbody>();
        _tagContact = GetComponent<TagContact>();
        _connectedObjectRigidbody = connectedObject.GetComponent<Rigidbody>();
        _connectedObjectTransform = connectedObject.GetComponent<Transform>();
        _connectedObjectTagContact = connectedObject.GetComponent<TagContact>();
    }
    private void Update()
    {
        if (_rb.velocity.y != 0 || _connectedObjectRigidbody.velocity.y != 0)
        {
            if (_tagContact.isContactedArray[1]) //중력장에 닿았으면
            {
                movePlatform(this.transform, _connectedObjectTransform, _connectedObjectTagContact);
                _connectedObjectRigidbody.velocity = Vector3.zero;
            }
            else if (_connectedObjectTagContact.isContactedArray[1]) //중력장에 닿았으면
            {
                movePlatform(_connectedObjectTransform, this.transform, _tagContact);
                _rb.velocity = Vector3.zero;
            }
            else
            {
                if (CompareVelocity())
                {
                    movePlatform(this.transform, _connectedObjectTransform, _connectedObjectTagContact);
                }
                else
                {
                    movePlatform(_connectedObjectTransform, this.transform, _tagContact);
                }
            }
        }
    }

    void movePlatform(Transform movingTransform, Transform movedTransform, TagContact tagContact)
    {
        Vector3 localPos = movedTransform.localPosition;
        
        movedTransform.localPosition = new Vector3(localPos.x, _originY - (movingTransform.localPosition.y - _originY), localPos.z);
        
        if (tagContact.isContactedArray[0]) //Player가 타고 있다면
        {
            float diff = movedTransform.localPosition.y - localPos.y;
            Transform playerTransform = FindAnyObjectByType<PlayerMovementManager>().transform.transform;
            
            playerTransform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + diff, playerTransform.position.z);
        }
    }

    bool CompareVelocity()
    {
        return Mathf.Abs(_rb.velocity.y) > Mathf.Abs(_connectedObjectRigidbody.velocity.y);
    }

    void ClampY()
    {
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, Mathf.Clamp(this.transform.localPosition.y, minY, maxY), this.transform.localPosition.z);
        _connectedObjectTransform.localPosition = new Vector3(_connectedObjectTransform.localPosition.x, Mathf.Clamp(_connectedObjectTransform.localPosition.y, minY, maxY), _connectedObjectTransform.localPosition.z);
    }
}
