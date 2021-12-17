using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MADD
{
    [System.Serializable]
    public class Game
    {
        public string gameCode; //the code that can be used to join the game if not started
        public bool started; //if all players agreed, the game started
        public int locationsAmount; //how many locations are in the game

        private List<Location> locations; //the actual locations on the backend
        private List<Player> players; //the players that are currently joined in the game
    }

    [System.Serializable]
    public class Location
    {
        public int id;
        public int index;
        public Player owner;
        public bool isBuildingUnit; //this will be true if a unit is being built at the moment
    }

    [System.Serializable]
    public class Player
    {
        public int id;
        public string name;
        public int resources; //used to build units

        private List<Location> locationsOwned;
        private List<Unit> unitsOwned;
    }

    [System.Serializable]
    public class Unit
    {
        public string id;
        public UnitType unitType;
    }

    [System.Serializable]
    public class UnitType
    {
        public int id;
        public string name;

        public Location targetLocation; //where is this unit headed at the moment (or where it is)

        //building related stats
        public int buildTime;
        public int cost;

        //travel related stats
        public int range;
        public int speed;
        
        //combat related stats
        public int power;
        public int health;
        public int attackTime;
    }
}