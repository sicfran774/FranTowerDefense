using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadShot : MonoBehaviour
{
    public bool activated;

    public void ShootSpread(Vector2 direction, float projectileSpeed)
    {
        GameObject projectileLeft = GetComponent<Tower>().InstantiateAmmo();
        GameObject projectileRight = GetComponent<Tower>().InstantiateAmmo();

        Vector2 directionLeft = new Vector2(direction.x - 1.2f, direction.y - 1.2f);
        Vector2 directionRight = new Vector2(direction.x + 1.2f, direction.y + 1.2f);

        projectileLeft.GetComponent<Rigidbody2D>().AddForce(directionLeft * projectileSpeed);
        projectileRight.GetComponent<Rigidbody2D>().AddForce(directionRight * projectileSpeed);
    }
}
