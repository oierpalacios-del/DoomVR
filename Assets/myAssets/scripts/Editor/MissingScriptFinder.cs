using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class MissingScriptFinder
{
    ///<summary>Esta herramienta busca todos los objetos con missing scripts (scripts eliminados que aun estan referenciados).
    ///Para eliminar la referencia puede seleccionar todo y darle a eliminar</summary>
    [MenuItem("Tools/Limpieza/Buscar Missing Scripts")]
    public static void Buscar()
    {
        int encontrados = 0;
        foreach (var root in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            foreach (var t in root.GetComponentsInChildren<Transform>(true))
            {
                int n = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject);
                if(n > 0)
                {
                    encontrados += n;
                    Debug.LogWarning($"Faltan {n} scripts en: {t.name}", t.gameObject);
                }
            }
        }
        Debug.Log($"Busqueda terminada: {encontrados} missing scripts");
    }
    [MenuItem("Tools/Limpieza/Limpiar Missing Scripts")]
    private static void Eliminar()
    {
        foreach ( var go in Selection.gameObjects)
        {
            Undo.RegisterCompleteObjectUndo(go, "Eliminar missing");
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
        }
    }
}
