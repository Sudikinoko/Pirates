using System.Collections.Generic;
using UnityEngine;

public class TargetFinder : MonoBehaviour
{
    public static GameObject PickTarget(GameObject[] enemies, Transform position, SearchStrategy searchStrategy, float range, float turnAngle)
    {

        switch (searchStrategy)
        {
            case SearchStrategy.furthest:
                return GetFurthest(enemies, position, range, turnAngle);
            case SearchStrategy.nearest:
                return GetNearest(enemies, position, range, turnAngle);

            default: return null;
        }

    }

    private static GameObject GetNearest(GameObject[] enemies, Transform position, float range, float turnAngle)
    {
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        float distanceToEnemy;
        float angle;

        foreach (GameObject enemy in enemies)
        {
            angle = Vector3.Angle(enemy.transform.position - position.position, position.forward);
            distanceToEnemy = Vector3.Distance(position.position, enemy.transform.position);

            if (distanceToEnemy <= range && distanceToEnemy < shortestDistance && angle <= turnAngle)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;

    }

    private static GameObject GetFurthest(GameObject[] enemies, Transform position, float range, float turnAngle)
    {
        float furthestDistance = 0f;
        GameObject furthestEnemy = null;
        float distanceToEnemy;
        float angle;

        foreach (GameObject enemy in enemies)
        {
            angle = Vector3.Angle(enemy.transform.position - position.position, position.forward);
            distanceToEnemy = Vector3.Distance(position.position, enemy.transform.position);
            if (distanceToEnemy <= range && distanceToEnemy > furthestDistance)
            {
                furthestDistance = distanceToEnemy;
                furthestEnemy = enemy;
            }
        }

        return furthestEnemy;
    }

    public static GameObject[] FindEnemies(List<string> enemyTags)
    {
        //TODO Erweiterung für mehr Fraktionen

        List<GameObject> enemies = new List<GameObject>();

        foreach (string enemyTag in enemyTags)
        {
            enemies.AddRange(GameObject.FindGameObjectsWithTag(enemyTag));
        }
        return enemies.ToArray();
    }
}
