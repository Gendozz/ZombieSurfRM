// Decompiled with JetBrains decompiler
// Type: RandomProvider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using System;
using System.Threading;

public static class RandomProvider
{
  private static int seed = Environment.TickCount;
  private static ThreadLocal<Random> randomWrapper = new ThreadLocal<Random>((Func<Random>) (() => new Random(Interlocked.Increment(ref RandomProvider.seed))));

  public static Random GetThreadRandom() => RandomProvider.randomWrapper.Value;
}
