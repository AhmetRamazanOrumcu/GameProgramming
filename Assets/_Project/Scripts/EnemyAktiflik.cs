using UnityEngine;

public class ScriptAktifYap : MonoBehaviour
{
    [Header("Aktif edilecek script")]
    public MonoBehaviour aktifEdilecekScript; // Inspector'dan atanacak

    void Update()
    {
        // Eðer GameManager'daki tufekVar true ise script aktif olsun
        if (GameManager.Instance.tufekVar)
        {
            if (aktifEdilecekScript != null && !aktifEdilecekScript.enabled)
            {
                aktifEdilecekScript.enabled = true;
                Debug.Log(aktifEdilecekScript.name + " aktif edildi!");
            }
        }
        else
        {
            // Ýstersen false olunca kapatmak için buraya ekleyebilirsin
            // aktifEdilecekScript.enabled = false;
        }
    }
}
