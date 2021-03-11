using UnityEngine;
using UnityEngine.SceneManagement;

public class trocaDeCena : MonoBehaviour
{

    public string cenaDestino;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {

        SceneManager.LoadScene(cenaDestino);

    }
}
