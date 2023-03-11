using UnityEngine;

public class BeatTrack : MonoBehaviour
{
    public float height, width;
    RectTransform trackScreen;
    // Start is called before the first frame update
    void Start()
    {
        
        gameObject.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y, 0);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
