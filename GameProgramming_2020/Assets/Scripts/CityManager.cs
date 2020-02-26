﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GRIDCITY
{
    public enum blockType { Block, Arches, Columns, Dishpivot, DomeWithBase, HalfDome, SlitDome, Slope, Tile};

	public class CityManager : MonoBehaviour
    {

        #region Fields
        private static CityManager _instance;
        public Mesh[] meshArray;
        public Material[] materialArray;
        public Transform buildingPrefab;
        public BuildingProfile[] profileArray;
        private bool[,,] cityArray = new bool[11,11,11];

        private int x;
        private int y;
        private int z;

        public static CityManager Instance
        {
            get
            {
                return _instance;
            }
        }
        #endregion

        #region Properties	
        #endregion

        #region Methods
        #region Unity Methods

        // Use this for internal initialization
        void Awake () {
            if (_instance == null)
            {
                _instance = this;
            }

            else
            {
                Destroy(gameObject);
                Debug.LogError("Multiple CityManager instances in Scene. Destroying clone!");
            };
        }
		
		// Use this for external initialization
		void Start () {
			for (int i=-4;i<5;i+=2)
            {
                for (int j=-4;j<13;j+=2)
                {
                        int random = Random.Range(0, profileArray.Length);
                        Instantiate(buildingPrefab, new Vector3(i, 0.0f, j), Quaternion.identity).GetComponent<DeluxeTowerBlock>().SetProfile(profileArray[random]);
                }
            }
		}
		
		// Update is called once per frame
		void Update () {
			
		}

        public bool CheckSlot(int x, int y, int z)
        {
            if (x < 0 || x > 10 || y < 0 || y > 10 || z < 0 || z > 10)
            {
                return true;
            }
            else
            {
                return cityArray[x, y, z];
            }
        }

        public void SetSlot(int x, int y, int z, bool occupied)
        {
            if (!(x < 0 || x > 10 || y < 0 || y > 10 || z < 0 || z > 10))
            {
                cityArray[x, y, z] = occupied;
            }
        }

		#endregion
	#endregion
		
	}
}
