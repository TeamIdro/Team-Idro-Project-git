using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIElementiMagia : MonoBehaviour
{
    [SerializeField] private GameObject uiToSpawn;
    [ReadOnly] public List<GameObject> uiMagicImages;

    private void Awake()
    {
        uiMagicImages = new List<GameObject>();
    }

    public void AddUI(ElementoMagiaSO elementoDaAggiungere)
    {
        GameObject uiInstantieted = Instantiate(uiToSpawn, transform);
        uiInstantieted.GetComponent<Image>().sprite = elementoDaAggiungere.spriteElemento;
        uiMagicImages.Add(uiInstantieted);
    }
    public void ClearUI()
    {
        foreach (var item in uiMagicImages.ToArray())
        {
            Destroy(item.gameObject);
            uiMagicImages.Remove(item);
        }
        
    }
}

