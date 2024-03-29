﻿using Random = UnityEngine.Random;

namespace CodeBase.Services.Randomizer
{
  public class RandomService : IRandomService
  {
    public int Next(int min, int max) =>
      Random.Range(min, max);
    
    public float Next(float min, float max) =>
      Random.Range(min, max);
  }
}