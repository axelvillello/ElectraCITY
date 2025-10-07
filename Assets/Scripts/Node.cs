using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private GameObject self;
    private GameObject origin;
    private int resistance;
    private int currentPower;
    private int cost;
    private float distance;
    
    public Node(GameObject self, GameObject origin, int resistance)
    {
        setSelf(self);
        setOrigin(origin);
        setResistance(resistance);
        setDistance(Vector3.Distance(self.transform.position, origin.transform.position));

        switch (resistance)
        {
            case 0:
                setCost(9); //Yellow Wire
                break;
            case 1:
                setCost(6); //Red Wire
                break;
            case 3:
                setCost(3); //Black Wire
                break;
            default:
                setCost(999); //Disallowed Resistance Used
                break;
        }
    }

    public GameObject getSelf() { return self; }
    public int getPowerPercent() { return self.GetComponent<Consumers>().getPowerPercent(); }
    public bool getSelfPowered() { return self.GetComponent<Consumers>().isPowerOn(); }
    public GameObject getOrigin() { return origin; }
    public int getResistance() {  return resistance; }
    public int getCurrentPower() { return currentPower;}
    public int getDistance() { return (int)distance; }
    public int getCost() { return cost; }

    public void setSelf(GameObject self) { this.self = self; }
    public void addPowerPercent(int power)
    {
        self.GetComponent<Consumers>().addPowerPercent(power);

        if (power >= 0)
            self.GetComponent<Consumers>().transform.GetComponent<SquashAndStretch>().PlaySquashAndStretchEffect();
    }
    public void setPowerOn(bool power) { self.GetComponent<Consumers>().setPowerOn(power); }
    public void setOrigin(GameObject origin) {  this.origin = origin; }
    public void setResistance(int resistance) { this.resistance = resistance; }
    public void setCurrentPower(int power) { this.currentPower = power; }
    public void setDistance(float distance) { this.distance = distance; }
    public void setCost(int cost) { this.cost = cost; }

}
