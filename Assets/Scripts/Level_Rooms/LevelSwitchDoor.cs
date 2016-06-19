using System.Linq;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelSwitchDoor : MonoBehaviour {
	private FloorGenerator floorGenerator;
	private GameObject levelText;

	public void OnTriggerEnter2D(Collider2D other) {
		buildNextLevel(other);
		ShowLevel showLevel = new ShowLevel();
		StartCoroutine(showLevel.Start());

		//StartCoroutine(showNextLevel());
    }

	public void buildNextLevel(Collider2D other) {
		if (other.CompareTag ("Player")) {
			floorGenerator = GameObject.FindGameObjectWithTag ("GameController").GetComponent<FloorGenerator> ();
			floorGenerator.numberOfRooms += 2;
			floorGenerator.level += 1;
			Debug.Log (floorGenerator.level);

			var r = floorGenerator._roomPrefabs.First ();
			r.transform.position = new Vector3 (0, 0f, 0);

			floorGenerator._firstRoom = r;
			floorGenerator.ClearFloor ();
			floorGenerator.TryGenerateFloor ();

			other.transform.position = Vector3.zero;
			floorGenerator.Grid.FirstRoom.OnPlayerEntersRoom (other.GetComponent<Player> ());

			GameObject.FindGameObjectWithTag ("MainCamera").transform.position = new Vector3 (0, 0, -10);
		}
	}
}