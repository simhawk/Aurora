using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
        public float orientation;
        public CivType civType;
        public string DisplayName;
        public Dice dice;

        public Dictionary<Resource, int> resources = new Dictionary<Resource, int> ();

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            resources.Add(Resource.Brick,0);    
            resources.Add(Resource.Wood,0);
            resources.Add(Resource.Rock,0);
            resources.Add(Resource.Wheat,0);
            resources.Add(Resource.Sheep,0);
        }

        public void addToResourceBank(Resource resource, int numberToAdd)
        {       
                if(isValidResource(resource))
                {
                        resources[resource] = resources[resource] + numberToAdd;
                }
                
        }

        public bool isValidResource(Resource resource) 
        {
                return resource == Resource.Brick 
                || resource == Resource.Wood 
                || resource == Resource.Wheat 
                || resource == Resource.Rock 
                || resource == Resource.Sheep;
        }
}
