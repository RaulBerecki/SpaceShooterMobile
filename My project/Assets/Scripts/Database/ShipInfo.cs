using UnityEngine;

public class ShipInfo
{
    public int id;
    public bool isOwned;
    public int price;
    public int highscoreShip;

    public ShipInfo(int id, bool isOwned, int price, int highscoreShip)
    {
        this.id = id;
        this.isOwned = isOwned;
        this.price = price;
        this.highscoreShip = highscoreShip;
    }
}
