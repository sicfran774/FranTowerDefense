using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadShot : MonoBehaviour
{
    public bool activated;
    public float degreeSpread;

    private Color color;

    void Awake()
    {
        color = new Color(1f, 0f, 0.62f);
    }

    void Update()
    {
        if (activated)
        {
            GetComponent<Tower>().ChangeColor(color);
        }
    }

    public void ShootSpread(Vector2 direction, float projectileSpeed)
    {
        GameObject projectileLeft = GetComponent<Tower>().InstantiateAmmo(transform);
        GameObject projectileRight = GetComponent<Tower>().InstantiateAmmo(transform);

        if (GetComponent<Tower>().canShootAllTypes)
        {
            projectileLeft.GetComponent<Projectile>().canShootAllTypes = true;
            projectileRight.GetComponent<Projectile>().canShootAllTypes = true;
        }

        Vector2 directionLeft;
        Vector2 directionRight;

        if (direction.x > 0 && direction.y > 0)
        {
            directionLeft = new Vector2(direction.x - degreeSpread, direction.y + degreeSpread);
            directionRight = new Vector2(direction.x + degreeSpread, direction.y - degreeSpread);
        }
        else if (direction.x > 0 && direction.y < 0)
        {
            directionLeft = new Vector2(direction.x + degreeSpread, direction.y + degreeSpread);
            directionRight = new Vector2(direction.x - degreeSpread, direction.y - degreeSpread);
        }
        else if (direction.x < 0 && direction.y < 0)
        {
            directionLeft = new Vector2(direction.x + degreeSpread, direction.y - degreeSpread);
            directionRight = new Vector2(direction.x - degreeSpread, direction.y + degreeSpread);
        }
        else
        {
            directionLeft = new Vector2(direction.x - degreeSpread, direction.y - degreeSpread);
            directionRight = new Vector2(direction.x + degreeSpread, direction.y + degreeSpread);
        }

        projectileLeft.GetComponent<Rigidbody2D>().AddForce(directionLeft * projectileSpeed);
        projectileRight.GetComponent<Rigidbody2D>().AddForce(directionRight * projectileSpeed);
    }
}
