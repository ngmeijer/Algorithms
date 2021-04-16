using System;

public class RNG
{
    private Random random = new Random();
    
    public int RandomInt(int pMin, int pMax)
    {
        return random.Next(pMin, pMax);
    }
}