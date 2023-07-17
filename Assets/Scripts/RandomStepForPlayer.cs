using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomStepForPlayer : MonoBehaviour
{
    [SerializeField] AudioSource playerStepSource;
    [SerializeField] List<AudioClip> passiPlayer;

    // Start is called before the first frame update
    private void Awake()
    {
         passiPlayer = Resources.LoadAll<AudioClip>("Audio/PassiPlayer").ToList();
    }
    // Update is called once per frame
   public void RandomStep()
    {
      // int index = Random.Range(0,passiPlayer.Count);
        //playerStepSource.clip = passiPlayer[index];
       // playerStepSource.Play();
    }
}
