using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;


public class Jugador : MonoBehaviourPunCallbacks
{
    private Image avatar;
    private string nombre;
    private bool[] pago = new bool[10];
    private bool[] llega = new bool[10];
    private int billetera;
    [SerializeField]
    private Text t_dias;

    private int dias = 1;
    float probabilidad;

    private int evasores = 0;



    // Start is called before the first frame update
    void Start()
    {
        billetera = System.Convert.ToInt32(Parametros.param.monto);                
        t_dias.text = "Día "+ System.Convert.ToString(dias);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pagar(bool button)
    {       
        pago[dias] = button;
        if (pago[dias])
            billetera = billetera-System.Convert.ToInt32(Parametros.param.precio);
        

    }

    public void Llegar(bool button)
    {
        //if (button.name == "Button Pagar")
        llega[dias] = true;
        //else pago[dias] = false;
        if (llega[dias])
            billetera = billetera+ System.Convert.ToInt32(Parametros.param.ganancia);        
    }

    public bool SaberSiPaga(bool button)
    {        
            return button;
    }

    public void CalcularViaje()
    {
        probabilidad=1f;
        int i = 0, evasores = 0;
        float x = 0f;
        double uno = 1;
        while (i != dias)
        {
            if (pago[i] == false)
                evasores++;
            i++;
        }
        //probabilidad = 1 - 1 / (1 + uno ^ (13 * (x - 0.5)));
        //probabilidad=1/(1 ^ ( 13 *( x - 0.5 ) ) );

    }


    public void OtroDia(bool button)
    {
        if (button) Debug.Log("Se pagó hoy");
        else Debug.Log("No se pagó hoy");
        if (dias <= 10) {
            Pagar(button);
            Llegar(button);
            dias++;
            Debug.Log("Comienza Día " + dias);
            Debug.Log("Saldo "+PhotonNetwork.LocalPlayer.NickName+": "+ billetera);
            t_dias.text = "Día " + System.Convert.ToString(dias);
        }
        if (dias == 10)
        {
            Debug.Log("Finalizado");
            PhotonNetwork.LeaveRoom();
        }
    }
}
