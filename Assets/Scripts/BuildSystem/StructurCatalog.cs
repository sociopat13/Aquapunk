using Mirror.Examples.MultipleMatch;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Aquapunk
{
    public class StructurCatalog : MonoBehaviour
    {
        public List<GameObject> structures;
        public GameObject structureCell;
        public GameObject context;
        public StructureInfoCard info;

        public void LoadStructureCatalog()
        {
            foreach (Transform child in context.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (GameObject manager in structures)
            {
                GameObject objectStructureManager = Instantiate(manager);
                StructureManager newManager = objectStructureManager.GetComponent<StructureManager>();
                GameObject cell = Instantiate(structureCell, context.transform);
                objectStructureManager.transform.SetParent(cell.transform);
                cell.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = newManager.structure.title;
                cell.transform.Find("Icon").GetComponent<Image>().sprite = newManager.structure.icon;
                cell.GetComponent<Button>().onClick.AddListener(() => info.GetStructureInfo(newManager));
            }
        }
    }
}

