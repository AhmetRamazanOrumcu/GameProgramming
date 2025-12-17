using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    public GameObject gunEmpty; // Player > MainCamera > GunEmpty

    void Start()
    {
        // Tabanca
        if (GameManager.Instance.tabancaVar && GameManager.Instance.TabancaPrefab != null)
        {
            GameObject tabanca = Instantiate(
                GameManager.Instance.TabancaPrefab,
                gunEmpty.transform // parent olarak GunEmpty
            );
            tabanca.transform.localPosition = new Vector3(-0.441f, 0f, 0f); // tabanca offset
            tabanca.transform.localRotation = Quaternion.identity;
        }

        // Tüfek
        if (GameManager.Instance.tufekVar && GameManager.Instance.TufekPrefab != null)
        {
            GameObject tufek = Instantiate(
                GameManager.Instance.TufekPrefab,
                gunEmpty.transform // parent olarak GunEmpty
            );
            tufek.transform.localPosition = Vector3.zero; // tüfek tam merkez
            tufek.transform.localRotation = Quaternion.identity;
        }
    }
}
