using System.Collections;
using System.Collections.Generic;
using Enemies;
using GameObjects;
using UnityEngine;

namespace Managers
{
    [CreateAssetMenu]
    public class Wave : ScriptableObject
    {
        public List<Cluster> enemyClusters;

        //public Wave(Queue<Cluster> enemyClusters)
        //{
        //    this.enemyClusters = enemyClusters;
        //}

        public Cluster Spawn(int idx)
        {
            if(idx >= enemyClusters.Count) { return null; }
            Cluster cluster = enemyClusters[idx];
            return cluster;
        }
    }
}

