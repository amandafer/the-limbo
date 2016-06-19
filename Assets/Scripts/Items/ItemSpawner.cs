using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {
    [SerializeField]
    private List<ItemBase> _itemPrefabs = new List<ItemBase>();
    public List<ItemBase> ItemPrefabs {
        get { return _itemPrefabs; }
    }

    [SerializeField]
    private bool _spawnOnAwake = true;
    public bool SpawnOnAwake {
        get { return _spawnOnAwake; }
        set { _spawnOnAwake = value; }
    }

	/* Spawn on awake
	 * TODO: not using yet, might be used 
	public void Awake() {
        if(SpawnOnAwake)
			Spawn();
	}
	*/

	public void Spawn(RoomType thisRoomType) {
		var probability = Random.value;

		if ((thisRoomType != RoomType.BossRoom) && probability <= 0.8) {
			var item = (ItemBase)Instantiate (_itemPrefabs [Random.Range (0, _itemPrefabs.Count)]);
			item.transform.parent = transform;

			// The item will spawn in random positions
			var positionX = Random.Range (-4.5f, 4.5f);
			var positionY = Random.Range (-2.5f, 1.5f);
			item.transform.localPosition = new Vector3 (positionX, positionY);
		} else if (thisRoomType == RoomType.BossRoom) {
			var item = (ItemBase)Instantiate (_itemPrefabs [Random.Range (0, _itemPrefabs.Count)]);
			item.transform.parent = transform;
			item.transform.localPosition = new Vector3 (0f, 2.5f);
		}
    }
}