// Decompiled with JetBrains decompiler
// Type: MusicPlaylist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 835427CF-0082-4424-81BC-F185068A307D
// Assembly location: D:\dev\!Decomp\TrafficRunner_build_hw16\TrafficRunner_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class MusicPlaylist : MonoBehaviour
{
  [SerializeField]
  private List<AudioSource> musicPrefabs;
  [SerializeField]
  private bool playMusic = true;
  private List<AudioSource> musicPlayList;
  private int currentMusicNumber = -1;

  private void Start()
  {
    this.musicPlayList = new List<AudioSource>();
    for (int index = 0; index < this.musicPrefabs.Count; ++index)
    {
      this.musicPlayList.Add(Object.Instantiate<AudioSource>(this.musicPrefabs[index], this.transform));
      this.musicPlayList[index].Stop();
    }
    this.ShuffleAndPlay();
  }

  private void Update()
  {
    if (!this.CanPlayMusic() || this.musicPlayList[this.currentMusicNumber].isPlaying)
      return;
    if (this.currentMusicNumber < this.musicPlayList.Count - 1)
      this.PlayMusic(this.currentMusicNumber + 1);
    else
      this.ShuffleAndPlay();
  }

  private bool CanPlayMusic() => this.playMusic && this.musicPlayList != null && this.musicPlayList.Count > 0;

  private void PlayMusic(int musicNumber)
  {
    if (!this.CanPlayMusic())
      return;
    if (this.currentMusicNumber != -1)
      this.musicPlayList[this.currentMusicNumber].Stop();
    this.currentMusicNumber = musicNumber;
    this.musicPlayList[this.currentMusicNumber].Play();
  }

  private void ShufflePlayList()
  {
    for (int index1 = 0; index1 < this.musicPlayList.Count; ++index1)
    {
      int index2 = Random.Range(index1, this.musicPlayList.Count);
      AudioSource musicPlay = this.musicPlayList[index1];
      this.musicPlayList[index1] = this.musicPlayList[index2];
      this.musicPlayList[index2] = musicPlay;
    }
  }

  private void ShuffleAndPlay()
  {
    if (!this.CanPlayMusic())
      return;
    AudioSource audioSource = (AudioSource) null;
    if (this.currentMusicNumber != -1)
      audioSource = this.musicPlayList[this.currentMusicNumber];
    this.ShufflePlayList();
    int musicNumber = 0;
    if ((Object) audioSource != (Object) null && (Object) this.musicPlayList[0] == (Object) audioSource && this.musicPlayList.Count > 1)
      musicNumber = 1;
    this.PlayMusic(musicNumber);
  }
}
