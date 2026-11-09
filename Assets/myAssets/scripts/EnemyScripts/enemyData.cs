using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string nombreEnemigo;
    [TextArea] public string descripcionEnemigo;
    public int vidaMax;
    public int vidaActual;
    public float speed;
    public int damageMin;
    public int damageMax;
    public GameObject prefab;
}
