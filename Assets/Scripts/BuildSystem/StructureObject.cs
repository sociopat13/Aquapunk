using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Threading.Tasks;
using System;

namespace Aquapunk
{
    public class StructureObject : NetworkBehaviour
    {
        public Structure structure;

        private GameObject temporarily;
        private float cooldownTime;

        [Server]
        public void Build()
        {
            temporarily = Instantiate(structure.structureProcessBuild, gameObject.transform);
            Building();
        }

        [Server]
        private async Task Building()
        {
            await Task.Delay(TimeSpan.FromSeconds(structure.timeBuild));
            Destroy(temporarily);
            temporarily = Instantiate(structure.structureFinichBuild, gameObject.transform);

        }
    }
}

