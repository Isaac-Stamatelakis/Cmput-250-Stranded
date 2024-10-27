using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerModule;

public class DateHealUpgrade : MonoBehaviour
{
    [SerializeField] private LineRenderer linePrefab;
    private int counter;
    public void addKill() {
        counter ++;
        if (counter >= PlayerUpgradeUtils.DATE_KILL_HEAL_COUNT) {
            healPlayer();
            counter = 0;
        }
    }

    public void Start() {
        
    }

    public void FixedUpdate() {
        
    }

    public void healPlayer() {
        Player player = Player.Instance;
        player.Heal(PlayerUpgradeUtils.DATE_KILL_HEAL_AMOUNT);

        Vector3 playerPosition = player.transform.position;
    
        LineRenderer lineRenderer = GameObject.Instantiate(linePrefab);
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, playerPosition);
        lineRenderer.transform.SetParent(transform,false);
        StartCoroutine(deleteLine(lineRenderer.gameObject));
    }

    private IEnumerator deleteLine(GameObject lineObject) {
        yield return new WaitForSeconds(0.25f);
        GameObject.Destroy(lineObject);
    }
    
}
