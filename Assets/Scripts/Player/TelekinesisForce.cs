using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelekinesisForce : MonoBehaviour
{
    //Variables para ajustar parametros de la fuerza telequinetica
    [SerializeField] private float pushRange;
    [SerializeField] private int pushForce;
    [SerializeField] private float pullRange;
    [SerializeField] private float recoilSpeed;
    [SerializeField] private float recoilDuration;
    [SerializeField] private LayerMask pushableLayer;
    

    //Variables ligadas al powerup
    private int originalForce;
    private float originalRecoilSpeed;
    private bool upgradeActive = false;
    private bool upgradeUsed = false; //Bandera para indicar si el jugador uso el powerup   

    //Variables para obtener Componentes.
    private Rigidbody2D playerRB;
    private MousePosition mousePosition;
    private PlayerBehaviour movementBehaviour;
    private PlayerEnergy playerEnergy;
    [SerializeField] private Transform pushOrigin;
    [SerializeField] private LineRenderer pushRenderer;

    //Variables ligadas al patron Observer
    private StatsManager statsManager;

    //Propiedad para gestion del PowerUp (Empujon y tirón potenciado)
    public bool isArchorPoint;
    public int PushForce
    {
        get { return pushForce; }
        set
        {
            if (!upgradeActive) //Bandera que se controla mas adelante desde el uso del pulso telekinetico, para reestablecer valores originales
            {
                originalForce = pushForce;
                originalRecoilSpeed = recoilSpeed;

                pushForce = value;
                recoilSpeed = value;
                upgradeActive = true;
                upgradeUsed = false;
            }
        }

    }

    //Metodos publicos para que desde el script del Jugador (que gestiona imputs) se acceda a los metodos de esta mecanica principal (privada).
    public void MouseGetPush()
    {
        MousePush();
    }
    public void MouseGetPull()
    {
        MousePull();
    }
    void Start()
    {
        statsManager = GetComponent<StatsManager>();
        playerEnergy = GetComponent<PlayerEnergy>(); //La necesito para estar pendiente de la energia del jugador.
        playerRB = GetComponent<Rigidbody2D>();
        mousePosition = GetComponent<MousePosition>();  //Dejo establecido el enlace al otro script. De aca puedo recurrir a otros metodos o propiedades.
                                                        //Principalmente, usado para obtener la pos del mouse, que daba problemas cuando lo calculaba en ambos scritps
        movementBehaviour = GetComponent<PlayerBehaviour>();
    }
    private void Update()
    {
    /*    ShowRaycastLine()*/;  //Para mostrar el Raycast al jugador, tal vez lo quite
                            //Spoiler del futuro, no lo saque, lo use para dar fuerza a la mecanica principal.
    }
    private void MousePush()
    {
        Vector2 direction = mousePosition.MouseWorlPos;  //Tomo del atributo publico del otro script, asi no se hacen multiples calculos para la pos del mouse.
        Debug.DrawRay(pushOrigin.position, direction * pushRange, Color.cyan, 1f);
        RaycastHit2D hit = Physics2D.Raycast(pushOrigin.position, direction, pushRange, pushableLayer);

        if (hit.collider != null)
        {
            Debug.Log("Se usó EMPUJAR: " + hit.collider.name);
            Rigidbody2D targetRB = hit.collider.GetComponent<Rigidbody2D>();
            IActiveable activeable = hit.collider.GetComponent<IActiveable>(); //Chequeo si el Raycast entra en contacto con un objeto cuyo script tenga la interfaz de activable
            if (activeable != null && playerEnergy.Energy > 0)//Si la tiene, y el jugador tiene energia, activa el metodo del script.
            {
                activeable.Activeable();
            }
            //Importante el else if dsps, porque sino me lo toma como un objeto pushable por su layer, y el jugado se mueve, lo cual no quiero.
            //O es un objeto interactuable, que solo se activa, o funciona como un objeto con masa.
            else if (targetRB != null && playerEnergy.Energy > 0)
            {


                if (targetRB.mass > playerRB.mass || targetRB.isKinematic) //Agregue lo del kinematic porque termine usando tilesmaps para algunos objetos estaticos que quiero actuen como metales
                {
                    //JUGADOR SE EMPUJA
                    isArchorPoint = true;
                    StartCoroutine(ApplyRecoil(-direction));
                    statsManager.NewEnergy(-20);
                    isArchorPoint = false;
                    return;
                }
                //if (targetRB.CompareTag("Spawner")) //Si es un spawner, que se destruya
                //{
                //    Destroy(targetRB.gameObject, 1f);
                //}
                else
                {
                    //OBJETO SE EMPUJA
                    targetRB.AddForce(direction * pushForce, ForceMode2D.Impulse);
                    statsManager.NewEnergy(-20);
                }
                //REVISO SI EL POWERUP ESTA ACTIVO  Y FUE USADO. Si solo esta activo, reestablezco el resto de valores en este condicional y cambio las banderas.
                if (upgradeActive && !upgradeUsed)
                {
                    pushForce = originalForce;
                    recoilSpeed = originalRecoilSpeed;

                    upgradeUsed = true;
                    upgradeActive = false;

                    Debug.Log("PowerUp consumido, valores restaurados");
                }
            }
            else
            {
                Debug.Log("No tienes energia!");
            }
        }
    }
    private void MousePull()
    {
        Vector2 direction = mousePosition.MouseWorlPos;
        RaycastHit2D hit = Physics2D.Raycast(pushOrigin.position, direction, pullRange, pushableLayer);

        if (hit.collider != null)
        {
            Debug.Log("Se usó TIRAR: " + hit.collider.name);

            Rigidbody2D targetRB = hit.collider.GetComponent<Rigidbody2D>();
            IActiveable activeable = hit.collider.GetComponent<IActiveable>();
            if (activeable != null && playerEnergy.Energy > 0)
            {
                activeable.Activeable();
            }
            else if (targetRB != null && playerEnergy.Energy > 0)
            {
                if (targetRB.mass > playerRB.mass || targetRB.isKinematic)
                {
                    //JUGADOR SE ATRAE
                    StartCoroutine(ApplyRecoil(direction));
                    statsManager.NewEnergy(-20);
                    return;
                }
                //if (targetRB.CompareTag("Spawner")) //Si es un spawner, que se destruya
                //{
                //    Destroy(targetRB.gameObject, 1f);
                //}
                else
                {
                    // OBJETO SE ATRAE
                    targetRB.AddForce(-direction * pushForce, ForceMode2D.Impulse);  //el pullDir tambien funciona, pero para mantener consistencia en la logica en ambos casos dejo el -direction a no ser que encuentre un problema a futuro
                    statsManager.NewEnergy(-20);
                }
            }
            if (upgradeActive && !upgradeUsed)
            {
                pushForce = originalForce;
                recoilSpeed = originalRecoilSpeed;
                upgradeUsed = true;
                upgradeActive = false;
                Debug.Log("PowerUp consumido, valores restaurados");
            }
            else
            {
                Debug.Log("No tienes energia!");
            }
        }
    }

    private IEnumerator ApplyRecoil(Vector2 direction) // Es un metodo corutina, NO interfaz
    {
        movementBehaviour.isRecoiling = true;
        movementBehaviour.canMove = false; //Anulo temporalmente la toma de imputs mientras se esta en retroceso.
        playerRB.velocity = direction.normalized * recoilSpeed;
        yield return new WaitForSeconds(recoilDuration); //USO DE CORRUTINA yield return
        // Esta es la parte clave: se espera un cierto tiempo (en segundos) sin detener el resto del juego
        // Por eso termine usando una corrutina (IEnumerator): permite esperar tiempo real entre instrucciones sin necesidad de pausar el jeugo
        //playerRB.velocity = Vector2.zero;
        movementBehaviour.isRecoiling = false;
        movementBehaviour.canMove = true;
    }

    private void ShowRaycastLine()
    {
        Vector2 direction = mousePosition.MouseWorlPos;
        Vector3 pushStart = pushOrigin.position;
        Vector3 pushEnd = pushStart + (Vector3)direction * pushRange;
        //Mismo check del hit con las capas que me interesa de antes, pero ahora solo para modificar color.
        RaycastHit2D hit = Physics2D.Raycast(pushOrigin.position, direction, pullRange, pushableLayer);

        if (hit.collider != null)
        {
            pushRenderer.startColor = new Color(0f, 1f, 1f, 1f);   // Cyan si el hit es Pushable.
            pushRenderer.endColor = new Color(0f, 1f, 1f, 0f);
            pushRenderer.SetPosition(0, pushStart);
            pushRenderer.SetPosition(1, pushEnd);
        }
        else
        {
            pushRenderer.startColor = new Color(1f, 0f, 0f, 1f); // Rojo si no es Pushable.
            pushRenderer.endColor = new Color(1f, 0f, 0f, 0f);

            pushRenderer.SetPosition(0, pushStart);
            pushRenderer.SetPosition(1, pushEnd);
        } 
    }
}


