using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingNumbers : MonoBehaviour {
    public Text displayNumber;

    [SerializeField]
    private float moveSpeed;

    void Update() {
        transform.position = new Vector3(transform.position.x, (transform.position.y + (moveSpeed * Time.deltaTime)), transform.position.z);
    }

    public void SetDisplayText(int damage) {
        displayNumber.text = damage.ToString();
    }

    public void SetHealColor() {
        displayNumber.color = Color.green;
    }

    public void SetDamageColor() {
        displayNumber.color = Color.red;
    }

    public void SetNoColor() {
        displayNumber.color = Color.white;
    }    
}

