using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;


[System.Serializable]
public class SingletonSoundClip
{
	public AudioClip clip;
	public float volume = 1.0f;
	public float pitch = 1.0f;
	public bool loop = false;
	public bool playOnAwake = false;
	public Singleton.SoundClipType type;
	public AudioSource source;
}


public class Singleton : MonoBehaviour 
{
	public SingletonSoundClip[] soundClips;

	private GameObject canvas;
    private ActionMenu actionMenu;
	private GameObject backgroundObject;

	public static Singleton current 
	{ 
		get { return (Singleton)GameObject.Find("Singleton").GetComponent("Singleton"); } 
		set {}
	}

	public enum CursorType
	{
		None = -1,
		Look = 0,
		Activate = 1,
		Grab = 2
	}

	public enum SoundClipType
	{
		ChickenCalm,
		ChickenAnxious,
		LesleyWalking,
		ChestClose,
		ChestOpen,
		FairyDust,
		OpenDoor,
		UnlockDoor,
		ElfGiggle,
		ElfDying,
		SatanicChant,
		ThunderClap,
		KillTheDuke
	}

	// Use this for initialization
	void Start () 
	{
		backgroundObject = (GameObject)GameObject.Find("Background");

		canvas = (GameObject)GameObject.Find("Canvas");
		canvas.SetActive(false);

		actionMenu = (ActionMenu)GameObject.Find("ActionMenu").GetComponent("ActionMenu");
		actionMenu.gameObject.SetActive(false);

		foreach (SingletonSoundClip soundClip in soundClips)
		{
			if (soundClip.clip && !soundClip.source)
			{
				AudioSource source = (AudioSource)gameObject.AddComponent("AudioSource");
				source.clip = soundClip.clip;
				source.loop = soundClip.loop;
				source.volume = soundClip.volume;
				source.pitch = soundClip.pitch;
				source.playOnAwake = soundClip.playOnAwake;
				soundClip.source = source;
			}
		}
	}

	public class GameObjectZPositionComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			GameObject gameObjectX = x as GameObject;
			GameObject gameObjectY = y as GameObject;
			float gameObjectXZ = gameObjectX.transform.position.z;
			float gameObjectYZ = gameObjectY.transform.position.z;

			if (gameObjectX.tag == "Background")
			{
				gameObjectXZ += 10000f;
			}

			if (gameObjectY.tag == "Background")
			{
				gameObjectYZ += 10000f;
            }

			float resultF = gameObjectXZ - gameObjectYZ;

			if (resultF > 0f) return 1;
			else if (resultF == 0f) return 0;

			return -1;
        }
    }

	GameObject GameObjectUnderMousePosition(Vector2 mousePosition)
	{
		GameObject[] gameObjects = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
		ArrayList objectsUnderMouse = new ArrayList();
		
		foreach (GameObject gameObject in gameObjects)
		{
			Vector3 mousePositionWithZ = new Vector3(mousePosition.x, mousePosition.y, gameObject.transform.position.z);
			
			if (gameObject.tag == "InteractionObject")
			{
				Collider2D[] colliders = gameObject.GetComponents<Collider2D>();
				
				foreach (Collider2D collider in colliders)
				{
					Bounds bounds = collider.bounds;
					
					if (bounds.Contains(mousePositionWithZ))
					{
						objectsUnderMouse.Add(gameObject);
						break;
					}
				}
			} 
			else if (gameObject.renderer && !gameObject.transform.parent)
			{
				Bounds bounds = gameObject.renderer.bounds;
                
                if (bounds.Contains(mousePositionWithZ))
                {
                    objectsUnderMouse.Add(gameObject);
                }
            }
        }
        
        objectsUnderMouse.Sort(new GameObjectZPositionComparer());    
	
		if (objectsUnderMouse.Count > 0)
		{
			return (GameObject)objectsUnderMouse[0];
		}

		return null;
	}
    
    void Update () 
    {
		if (Input.GetButtonUp("Fire1")) 
        {
			if (canvas.activeSelf)
			{
				canvas.SetActive(false);
				return;
			}

			Vector2 mousePosition = Input.mousePosition;
			
			mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

			GameObject selectedGameObject = GameObjectUnderMousePosition(mousePosition);

			if (selectedGameObject)
			{
				if (Inventory.current.selectedItemType != InventoryItem.InventoryItemType.None)
				{
					actionMenu.HideMenu();

					if (selectedGameObject.tag != "Inventory")
					{
						InteractiveObject interactiveObject = selectedGameObject.GetComponent<InteractiveObject>();
						bool canInteract = false;

						if (interactiveObject) 
						{
							canInteract = interactiveObject.InteractWithItem(Inventory.current.selectedItemType);
						}

						if (!canInteract)
						{
							ShowMessageOverlay("That doesn't work that way.");
						}
					}
				} 
				else if (selectedGameObject.tag != "ActionMenu")
				{
					if (actionMenu.isShown)
					{
						actionMenu.HideMenu();
					} 
					else if (selectedGameObject.tag != "Inventory")
					{
						actionMenu.ShowMenu(mousePosition, selectedGameObject);
					}
				} 
				else 
				{
					actionMenu.HandleMouseUp(mousePosition);
				}
			}
		}
	}

	AudioSource FindSoundSource(SoundClipType type)
	{
		foreach (SingletonSoundClip soundClip in soundClips)
		{
			if (soundClip.type == type && soundClip.source)
			{
				return soundClip.source;
			}
		}

		return null;
	}

	public void PauseSound(SoundClipType type)
	{
		AudioSource source = FindSoundSource(type);

		if (source) 
		{
			source.Pause();
		}
	}

	public void PlaySound(SoundClipType type)
	{
		AudioSource source = FindSoundSource(type);
		
		if (source) 
		{
			source.Play();
		}
	}

	public void ShowMessageOverlay(string theText) 
	{
		Text textComponent = (Text)canvas.transform.Find("Text").gameObject.GetComponent("Text");
		
		textComponent.text = theText;
		
		canvas.SetActive(true);
	}
}
