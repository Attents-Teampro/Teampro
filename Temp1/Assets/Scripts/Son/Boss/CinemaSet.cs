using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CinemaSet : MonoBehaviour
{
    PlayableDirector pd;
    TimelineAsset ta;
    public bool isTrue;
    // Start is called before the first frame update
    private void Awake()
    {
        pd = GetComponent<PlayableDirector>();
        
    }
    
    private void Start()
    {
        
    }
    public void SetCinemaBindings(GameObject[] g)
    {

        int count = 0;
        
        ta = pd.playableAsset as TimelineAsset;
        IEnumerable<TrackAsset> temp = ta.GetOutputTracks();



        foreach (TrackAsset track in temp)
        {
            if (track is CinemachineTrack)
            {
                pd.SetGenericBinding(track, g[count++]);
            }
        }
    }
}
