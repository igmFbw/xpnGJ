using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class healthBar : MonoBehaviour
{
    [SerializeField] private Image healthSlot;
    public void setActive(bool active)
    {
        healthSlot.gameObject.SetActive(active);
    }
}