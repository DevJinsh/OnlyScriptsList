using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : TriggerObject
{
    public GameObject spawnObject;

    private Vector3 _spawnPosition;
    private List<GameObject> _items;
    [SerializeField] int _count;

    private void Start()
    {
        _spawnPosition = transform.GetChild(0).position;
    }
    public override void TriggerActive()
    {
        _items = new List<GameObject>();
        StartCoroutine(Active());
    }

    public override void TriggerInactive(float resetTime)
    {
        foreach(GameObject go in _items)
        {
            Destroy(go, resetTime * 2f);
        }
        _items.Clear();
    }

    void StartSpawn()
    {
        _items.Add(Instantiate(spawnObject));
        _items[_items.Count - 1].transform.position = _spawnPosition;
    }

    IEnumerator Active()
    {
        int cnt = 0;
        while (cnt < _count)
        {
            StartSpawn();
            yield return new WaitForSeconds(0.5f);
            cnt++;
        }
    }
}
