using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Known bug: If blizzard is activated on one tower and activated on another one within that ability duration, the second ability won't even occur
//The first coroutine will pass Unfreeze(), the second coroutine (from the second tower) will wait ability duration to Unfreeze() even though it has been passed in the first one

public class JuulsHandler : MonoBehaviour
{
    public IEnumerator FreezeAllEnemies()
    {
        Freeze();
        yield return new WaitForSeconds(GetComponent<Tower>().abilityDuration);
        Unfreeze();
    }

    void Freeze()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            enemy.transform.GetChild(1).gameObject.SetActive(true);
            enemy.GetComponent<Enemy>().frozen = true;
        }
    }
    void Unfreeze()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<Enemy>().frozen = false;
            enemy.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
    void OnDestroy()
    {
        StopAllCoroutines();
    }
}

