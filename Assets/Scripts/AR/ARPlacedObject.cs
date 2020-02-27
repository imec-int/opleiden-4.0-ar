using UnityEngine;

public class ARPlacedObject : MonoBehaviour
{
    [SerializeField]
    private GameObject Model;

    public void ShowModel(bool visibility){
        Model.SetActive(visibility);
    }
}
