using UnityEngine;

public class sceneMovement : MonoBehaviour
{

    Transform transform;
    Vector3 posAtual;
    // Start is called before the first frame update
    void Start()
    {

    transform = GetComponent<Transform>();
    posAtual =transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        MovendoCena();
    }

    void MovendoCena(){

        posAtual.x -= 0.001f;
        transform.localPosition = posAtual;
        if (posAtual.x < -21){
            posAtual.x = 0;
        }
    }
}
