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
        public List<StructureManager> structures;
        public GameObject structureCell;
        public GameObject context;
        public StructureInfoCard info;

        public void LoadStructureCatalog()
        {
            foreach (Transform child in context.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (StructureManager manager in structures)
            {
                GameObject cell = Instantiate(structureCell, context.transform);
                cell.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = manager.structure.title;
                cell.transform.Find("Icon").GetComponent<Image>().sprite = manager.structure.icon;
                cell.GetComponent<Button>().onClick.AddListener(() => info.GetStructureInfo(manager));
            }
        }
    }
}

