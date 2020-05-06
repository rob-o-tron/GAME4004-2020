﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GRIDCITY
{
	public class GridCityManager : MonoBehaviour
    {

        #region Fields
        private static GridCityManager _instance;

        public NavMeshSurface navMesh;
        public Transform tilePrefab;
        public Transform slopeUpPrefab;
        public Transform slopeDownPrefab;
        public Transform agentPrefab;
        public Transform gridVisPrefab;
        public GameObject treePrefab;
        public Transform basicPrefab;

        public BuildingProfile[] gameProfileArray;
        public BuildingProfile wallProfile;
        public GameObject buildingPrefab;
        public GameObject roadPrefab;
        public Transform startLocation;
        public bool navMeshReady = false;


        private bool[,,] cityArray = new bool [150, 150, 150];   //increased array size to allow for larger city volume

        public static GridCityManager Instance
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
                Debug.LogError("Multiple NavigationCityManager instances in Scene. Destroying clone!");
            };
        }

        public void ResetArray()
        {
            for (int i = 0; i < 75; i++)
                for (int j = 0; j < 75; j++)
                    for (int k = 0; k < 75; k++)
                        cityArray[i, j, k] = false;
        }
		
		// Use this for external initialization
		void Start ()
        {
            /*
            //UPDATING PLANNING ARRAY TO ACCOUNT FOR MANUALLY PLACED|CITY GATE
            for (int ix = -2; ix < 3; ix++)
            {
                int iz = -17;
                for (int iy = 0; iy < 5; iy++)
                {
                    SetSlot(ix + 20, iy, iz + 20, true);
                }
            }


            //BUILD CITY WALLS
            for (int i = -17; i < 18; i += 34)
            {
                for (int j = -16; j < 17; j += 1)
                {
                    Instantiate(buildingPrefab, new Vector3(i, 0.05f, j), Quaternion.identity).GetComponent<GridTowerBlock>().SetProfile(wallProfile);
                }
                for (int j = -17; j < 18; j += 1)
                {
                    Instantiate(buildingPrefab, new Vector3(j, 0.05f, i), Quaternion.identity).GetComponent<GridTowerBlock>().SetProfile(wallProfile);
                }
            }
            */

        }

        private void Update()
        {

        }


        #endregion

        public bool CheckSlot(int x, int y, int z)
        {
            if (x < 0 || x > 149 || y < 0 || y > 149 || z < 0 || z > 149) return true;
            else
            {
                return cityArray[x, y, z];
            }

        }

        public void SetSlot(int x, int y, int z, bool occupied)
        {
            if (!(x < 0 || x > 149 || y < 0 || y > 149 || z < 0 || z > 149))
            {
                cityArray[x, y, z] = occupied;
                if (occupied)
                {
                    Instantiate(gridVisPrefab, new Vector3(x-75, y, z-75), Quaternion.identity,GameController.Instance.dummyPivot);
                }
            }

        }

        public void BuildTowers(int numTowers)
        {
            for (int i=0; i<numTowers; i++)
            {
                int RandomX = Random.Range(-15, 16);
                int RandomZ = Random.Range(-15, 16);
                int random = Random.Range(0, gameProfileArray.Length);
                if (!((RandomX>-5)&&(RandomX<5)&&(RandomZ>-5)&&(RandomZ<5)))
                    Instantiate(buildingPrefab, new Vector3(RandomX, 0.05f, RandomZ), Quaternion.identity).GetComponent<GridTowerBlock>().SetProfile(gameProfileArray[random]);

            }
        }

        public void BuildDome(float domeradius)
        {
            float radius = domeradius;
            int range = (int)domeradius;
            for (int j=0;j <= range; j++)
            {
                radius = domeradius * (Mathf.Cos(Mathf.PI / 2f * (j/ domeradius) ));
                for (int i=0;i<360;i++)
                {
                    float x = radius * Mathf.Sin(Mathf.PI * 2f * (((float)i) / 360f));
                    float z = radius * Mathf.Cos(Mathf.PI * 2f * (((float)i) / 360f));
                    float y = j;

                    int x_array = Mathf.RoundToInt(x + 75);
                    int z_array = Mathf.RoundToInt(z + 75);
                    int y_array = j;

                    if (!(CheckSlot(x_array, y_array, z_array)))
                    {
                        Instantiate(basicPrefab, new Vector3(Mathf.RoundToInt(x), j, Mathf.RoundToInt(z)), Quaternion.identity);
                        SetSlot(x_array, y_array, z_array, true);
                    }

                }
            }






        }



        public void BakeNavMesh()
        {
            navMesh.BuildNavMesh();
        }

        public void SetNavMeshReadyFlag(bool ready)
        {
            navMeshReady = ready;
        }

        public void BuildRoads()
        {
            Instantiate(roadPrefab, startLocation);
        }



        #endregion

    }
}