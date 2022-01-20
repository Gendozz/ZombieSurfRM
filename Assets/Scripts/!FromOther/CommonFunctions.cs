// Decompiled with JetBrains decompiler
// Type: CommonFunctions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using System;

public static class CommonFunctions
{
  public static int[] CreateIndexArray(int length)
  {
    int[] numArray = new int[length];
    for (int index = 0; index < length; ++index)
      numArray[index] = index;
    return numArray;
  }

  public static void ShuffleArray(int[] array, Random random)
  {
    for (int minValue = 0; minValue < array.Length; ++minValue)
    {
      int index = random.Next(minValue, array.Length);
      int num = array[minValue];
      array[minValue] = array[index];
      array[index] = num;
    }
  }
}
