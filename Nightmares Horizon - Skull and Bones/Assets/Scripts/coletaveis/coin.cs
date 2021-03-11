using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour
{


    private _GameController _GameController;
    public int valor;

    // Start is called before the first frame update
    void Start()
    {

        _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;
        valor =  1; //Random.Range(1, 5);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Coletar() {

        _GameController.gold += valor;
        Destroy(this.gameObject);
   
    }

}
