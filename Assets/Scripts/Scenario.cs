//Name: Scenario
//Description: Defines variables required for a selected when generating a game scene

public class Scenario
{
    private int genCount;
    private int conCount;
    private bool blackWire = true;
    private bool redWire = true;
    private bool yellowWire = true;

    public void setBlack(bool black)
    {
        blackWire = black;
    }
    public bool getBlack() { return blackWire; }
    public void setRed(bool red)
    {
        redWire = red;
    }
    public bool getRed() { return redWire;}
    public void setYellow(bool yellow)
    {
        yellowWire = yellow;
    }
    public bool getYellow() {  return yellowWire;}

    public void setCon(int con) { conCount = con; }
    public int getCon() { return conCount; }
    public void setGen(int gen) { genCount = gen; }
    public int getGen() { return genCount; }

}
