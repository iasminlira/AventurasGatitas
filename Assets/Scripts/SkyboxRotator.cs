using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
    public Transform player;
    public float rotationSpeed = 1f; // velocidade da rotação

    private Material skyboxMaterial;
    private float currentRotation = 0f;

    void Start()
    {
        // Faz uma cópia do material original pra não alterar globalmente
        skyboxMaterial = new Material(RenderSettings.skybox);
        RenderSettings.skybox = skyboxMaterial;
    }

    void Update()
    {
        if (player == null) return;

        // Aumenta rotação baseada no movimento do jogador em X
        currentRotation += player.GetComponent<Rigidbody>().velocity.x * rotationSpeed * Time.deltaTime;

        // Aplica rotação ao material do skybox
        skyboxMaterial.SetFloat("_Rotation", currentRotation);
    }
}
