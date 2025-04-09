using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private float x;
    private float y;
    private float width;
    private float height;
    private int[] coords;
    private bool occupied;
    private GameObject resource;
    public GridObject(float x, float y, float width, float height, int[] coords)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
        this.coords = coords;
    }


    public float getX() { return x; }
    public float getY() { return y; }
    public float getWidth() { return width; }
    public float getHeight() { return height; }
    public GameObject getResource() { return resource; }
    public bool isOccupied() {  return occupied; }
    public int[] getCoords() { return coords; }
    public int getCoordsX() { return coords[0]; }
    public int getCoordsY() { return coords[1]; }
    public int getCoordsTotal() 
    {
        int totalCoords = coords[0] * coords[1];
        return totalCoords;
    }

    public void setX(float x) { this.x = x;}
    public void setY(float y) {  this.y = y;}
    public void setWidth(float width) {  this.width = width;}
    public void setHeight(float height) {  this.height = height;}
    public void setResource(GameObject resource) {  this.resource = resource; }
    public void flipOccupied() { occupied = !occupied; }
    public void setCoords(int[] coords) { this.coords = coords; }

    public string printObject()
    {
        return "Element(" + x + "," + y + ") at W:H = " + width + ":" + height;
    }
}
