using System;
using UnityEditor;
using UnityEngine;

namespace Items {
    public abstract class Item 
    {
        /// <summary>
        /// Nama item yang bersangkutan
        /// </summary>
        public string name;
        /// <summary>
        /// deksripsi item yang bersangkutan
        /// </summary>
        public string description;
        /// <summary>
        /// Bernilai 1 jika item dapat dipick. <br/>
        /// kewajiban player untuk mengecek atribut ini
        /// </summary>
        public bool pickable;

        protected Item(string name, string description)
        {
            this.name = name;
            this.description = description;
            this.pickable = true;
        }
    }
}
