using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] private Entity _entityPrefab;
    
    private Entity _entityInstance;
    private LevelManager _levelManager;

    private void Awake()
    {
        _levelManager = GetComponentInParent<LevelManager>();
    }

    private void OnEnable()
    {
        _levelManager.TryAddSpawner(this);
    }

    private void OnDisable()
    {
        _levelManager.TryRemoveSpawner(this);
    }

    public void Spawn()
    {
        if (_entityInstance)
        {
            Debug.LogWarning($"{_entityInstance.name} was already created! Despawn this instance first!", gameObject);
        }
        else
        {
            Transform parent = GetOrCreateParent();
            _entityInstance = Instantiate(_entityPrefab, transform.position, transform.rotation, parent);
            _entityInstance.Spawner = this;
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void Despawn()
    {
        if (_entityInstance)
        {
            _entityInstance.gameObject.SetActive(false);
            Destroy(_entityInstance.gameObject);
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"Cannot destroy object. There is no instance of it!", gameObject);
        }
    }

    public void ResetEntity()
    {
        if (_entityInstance)
        {
            _entityInstance.gameObject.SetActive(false);
            Destroy(_entityInstance.gameObject);
            Transform parent = GetOrCreateParent();
            _entityInstance = Instantiate(_entityPrefab, transform.position, transform.rotation, parent);
            _entityInstance.Spawner = this;
        }
        else
        {
            Debug.LogWarning($"Cannot reset object. There is no instance of it!", gameObject);
        }
    }

    private Transform GetOrCreateParent()
    {
        foreach (Transform t in _levelManager.transform)
        {
            if (t.name == $"{_entityPrefab.name}Parent") return t;
        }

        Transform parent = new GameObject().transform;
        parent.parent = _levelManager.transform;
        parent.name = $"{_entityPrefab.name}Parent";
        return parent;
    }
}
