using NUnit.Framework;
using System.Collections.Generic;

public class User
{

    public string id;
    public int highscore;
    public string username;
    public int lastShipPlayed;
    public int coins;
    public int resources;
    public List<ShipInfo> shipInfos = new List<ShipInfo>();

    public User(string userId, string name)
    {
        this.id = userId;
        highscore = 0;
        this.username = name;
        lastShipPlayed = 0;
        coins = 0;
        resources = 0;
        shipInfos.Add(new ShipInfo(0, true, 0, 0));
        shipInfos.Add(new ShipInfo(1, false, 30, 0));
        shipInfos.Add(new ShipInfo(2, false, 35, 0));
        shipInfos.Add(new ShipInfo(3, false, 40, 0));
        shipInfos.Add(new ShipInfo(4, false, 60, 0));
        shipInfos.Add(new ShipInfo(5, false, 80, 0));
        shipInfos.Add(new ShipInfo(6, false, 100, 0));
        shipInfos.Add(new ShipInfo(7, false, 175, 0));
        shipInfos.Add(new ShipInfo(8, false, 300, 0));
        shipInfos.Add(new ShipInfo(9, false, 450, 0));
        shipInfos.Add(new ShipInfo(10, false, 700, 0));
        shipInfos.Add(new ShipInfo(11, false, 1000, 0));
    }
    public void UpdateCurrentUser(int highscore)
    {
        this.coins = 0;
        this.resources = 0;
        this.shipInfos = new List<ShipInfo>();
        shipInfos.Add(new ShipInfo(0, true, 0, highscore));
        shipInfos.Add(new ShipInfo(1, false, 30, 0));
        shipInfos.Add(new ShipInfo(2, false, 35, 0));
        shipInfos.Add(new ShipInfo(3, false, 40, 0));
        shipInfos.Add(new ShipInfo(4, false, 60, 0));
        shipInfos.Add(new ShipInfo(5, false, 80, 0));
        shipInfos.Add(new ShipInfo(6, false, 100, 0));
        shipInfos.Add(new ShipInfo(7, false, 175, 0));
        shipInfos.Add(new ShipInfo(8, false, 300, 0));
        shipInfos.Add(new ShipInfo(9, false, 450, 0));
        shipInfos.Add(new ShipInfo(10, false, 700, 0));
        shipInfos.Add(new ShipInfo(11, false, 1000, 0));
    }
}
