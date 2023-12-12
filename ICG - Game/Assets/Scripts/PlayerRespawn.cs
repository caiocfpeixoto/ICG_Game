using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound; //Som quando pegar o checkpoint
    private Transform currentCheckpoint; // Ultimo checkpoint é guardado aqui
    private Health playerHealth;
    private UIManager uiManager;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void CheckRespawn()
    {
        // Verificar se o checkpoint está disponível
        if (currentCheckpoint == null)
        {
            // Mostrar tela de game over
            uiManager.GameOver();
            return; // Não executar o resto desta função
        }

        playerHealth.Respawn(); //Reinicia a vida do player e reseta a animação
        transform.position = currentCheckpoint.position; // Movimentar player para a posição do checkpoint

        //Mover camera para o checkpoint room
        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);
    }

    //Ativar checkpoints
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform; // Guarda o checkpoint que foi ativado como atual
            SoundManager.instance.PlaySound(checkpointSound);
            collision.GetComponent<Collider2D>().enabled = false; // desativa colisão do checkpoint
            collision.GetComponent<Animator>().SetTrigger("appear"); // trigger animação checkpoint
        }
    }
}
