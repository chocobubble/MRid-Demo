using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AddressableAssets;

public class CircleSpawer : MonoBehaviour
{
    [SerializeField] private bool m_Spawning;
    [SerializeField] private Transform m_SpawnPosition;
    [SerializeField] private AssetReference m_LogPrefab;

    private IEnumerator Start()
    {
        float waitTime;

        while (m_Spawning)
        {
            waitTime = UnityEngine.Random.Range(2f, 5f);

            Addressables.InstantiateAsync(m_LogPrefab,
                m_SpawnPosition.position, Quaternion.identity,
                transform, true);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
