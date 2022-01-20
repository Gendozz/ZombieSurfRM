// Decompiled with JetBrains decompiler
// Type: Readme
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class Readme : ScriptableObject
{
  public Texture2D icon;
  public float iconMaxWidth = 128f;
  public string title;
  public string titlesub;
  public Readme.Section[] sections;
  public bool loadedLayout;

  [Serializable]
  public class Section
  {
    public string heading;
    public string text;
    public string linkText;
    public string url;
  }
}
