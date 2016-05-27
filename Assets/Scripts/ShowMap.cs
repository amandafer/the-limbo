using System.Linq;
using Assets.Scripts;
using UnityEngine;

public class ShowMap : MonoBehaviour  {
    public Texture2D _roomTexture, _unvisitedRoomTexture, _playerInRoomTexture, _bossRoomIcon;

    private FloorGenerator _floorGenerator;

	public void Awake() {
        _floorGenerator = GameObject.FindGameObjectWithTag("GameController").GetComponent<FloorGenerator>();
	}

    public void OnGUI() {
        if (_floorGenerator == null)
            return;

        var roomWidth = Screen.width/30;
        var roomHeight = Screen.height/30;

		var leftPadding = Screen.width - Screen.width/4;
        var topPadding = Screen.height-70;

        var iconWidth = Screen.width/20;
        var iconHeight = Screen.height/20;

        foreach (var room in _floorGenerator._floorGrid.Rooms.Where(r => r.IsVisibleOnMap)) {
            var roomTexture = room.PlayerHasVisited ? _roomTexture : _unvisitedRoomTexture;
            GUI.DrawTexture(
                new Rect(leftPadding + room.X*roomWidth, topPadding + -room.Y*roomHeight, roomWidth*2, roomHeight*2),
                room.PlayerIsInRoom ? _playerInRoomTexture : roomTexture, ScaleMode.ScaleToFit);

            if (room._roomType == RoomType.BossRoom) {
                GUI.DrawTexture(
                    new Rect(leftPadding + room.X*roomWidth + roomWidth/2, topPadding + -room.Y*roomHeight, iconHeight, iconWidth),
                    _bossRoomIcon, ScaleMode.ScaleToFit);
            }
        }
    }
}
