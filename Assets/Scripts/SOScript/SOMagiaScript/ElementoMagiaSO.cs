using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Magia/ElementoMagiaSO", menuName = "Magia/ElementoMagiaSO")]
public class ElementoMagiaSO : ScriptableObject
{
	public Texture2D textureDellaMagia;
	public TipoMagia tipoDiMagia;

  
}
public enum TipoMagia
{
	Fuoco,
	Acqua,
	Vento,
	Terra,
	Fulmine,

}
