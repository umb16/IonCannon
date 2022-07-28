using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class PlantsCollection
{
    [SerializeField] private Transform _root;
    [SerializeField] private GameObject[] _plants;
    [SerializeField] private GameObject[] _bigPlants;
    [SerializeField] private GameObject[] _plantsPacks;
    private GameObject BigPlant => _bigPlants[Random.Range(0, _bigPlants.Length)];
    private GameObject Plant => _plants[Random.Range(0, _plants.Length)];
    private GameObject PlantsPack => _plantsPacks[Random.Range(0, _plantsPacks.Length)];

    public GameObject CreateBigPlant(Vector3 pos) => Create(BigPlant, pos);
    public GameObject CreatePlantsPack(Vector3 pos) => Create(PlantsPack, pos);
    public GameObject CreatePlant(Vector3 pos) => Create(Plant, pos);
    private GameObject Create(GameObject prefab, Vector3 pos)
    {
        var obj = GameObject.Instantiate(prefab, _root);
        obj.transform.position = pos;
        return obj;
    }
}
