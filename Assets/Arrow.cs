using System;
using UnityEngine;
using UnityEngine.UI;
public class Arrow : MonoBehaviour
{
   public Color startColor;
   public Color endColor;

   public float speed;

   private Image img;
   private void Awake()
   {
      img = GetComponent<Image>();
   }

   private void Update()
   {
      img.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * speed, 1));
   }
}
