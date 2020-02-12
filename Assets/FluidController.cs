using UnityEngine;
using UnityEngine.Playables;

public class FluidController : MonoBehaviour
{

    [SerializeField]
    private float _time= 3;
    // Start is called before the first frame update
    PlayableDirector _director;
    void Awake()
    {
        _director=GetComponent<PlayableDirector>();

        _director.stopped += OnCompleted;
    }

    private void OnCompleted(PlayableDirector obj)
    {
        _director.time  = _time;
        _director.Play();
    }
}
