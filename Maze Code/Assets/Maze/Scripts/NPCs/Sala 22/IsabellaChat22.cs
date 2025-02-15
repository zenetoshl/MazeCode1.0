﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsabellaChat22 : Sign
{
    private Vector3 directionVector;
    private Transform myTransform;
    private Rigidbody2D myRigidbody;
    private Animator anim;
    public Collider2D bounds;
    protected JoyButtonAction joybutton;
    private int quantConversasIsabella = 5;
    private float timeChat = 0.2f;
    public GameObject lucas01;
    // Start is called before the first frame update
    void Start()
    {
        joybutton = FindObjectOfType<JoyButtonAction>();
        myTransform = GetComponent<Transform>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {          
            if (joybutton.Pressed && playerInRange)
            {
                if (quantConversasIsabella == 5)
                {
                    dialog = "Lucas: Oii Isabella.\n\nAperte para continuar...";
                    dialogBox.SetActive(true);
                    dialogText.text = dialog;
                    quantConversasIsabella--;
                }

                else if (quantConversasIsabella == 4)
                {
                    timeChat -= Time.deltaTime;
                    if (timeChat <= 0)
                    {
                        timeChat = 0.2f;
                        dialog = "Isabella: Oiii Lucas.\n\nAperte para continuar...";
                        dialogBox.SetActive(true);
                        dialogText.text = dialog;
                        quantConversasIsabella--;
                    }
                }
                else if (quantConversasIsabella == 3)
                {
                    timeChat -= Time.deltaTime;
                    if (timeChat <= 0)
                    {
                        timeChat = 0.2f;
                        dialog = "Isabella: Estou descansando um pouco, parece que para resolver o desafio pra sala final, eu preciso completar os desafios do último nível e encontrar o ultimo professor, porém parecem bem difíceis, então vou descansar um pouco.\nAperte para continuar...";
                        dialogBox.SetActive(true);
                        dialogText.text = dialog;
                        quantConversasIsabella--;
                    }
                }
                else if (quantConversasIsabella == 2)
                {
                    timeChat -= Time.deltaTime;
                    if (timeChat <= 0)
                    {
                        timeChat = 0.2f;
                        dialog = "Lucas: Eu estou empolgado, vou continuar minha jornada então.\n\nAperte para continuar...";
                        dialogBox.SetActive(true);
                        dialogText.text = dialog;
                        quantConversasIsabella--;
                    }
                }
            else if (quantConversasIsabella == 1)
            {
                timeChat -= Time.deltaTime;
                if (timeChat <= 0)
                {
                    timeChat = 0.2f;
                    dialog = "Isabella: Eu sei que você consegue, boa sorte !!\n\nAperte para continuar...";
                    dialogBox.SetActive(true);
                    dialogText.text = dialog;
                    quantConversasIsabella--;
                }
            }
            else
                {
                    timeChat -= Time.deltaTime;
                    if (timeChat <= 0)
                    {
                        dialog = "Lucas: Até mais!!";
                        dialogBox.SetActive(true);
                        dialogText.text = dialog;
                        //quantConversasIsabella = 3;
                    }
                }
            }

        }
        
}
