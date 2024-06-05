using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class RoomGenerator : MonoBehaviour
{
    private int ROOM_NUMBER = 10;
    private List<Room> rooms = new List<Room>();
    private float xOffet = 30f;
    private float yOffet = 20f;
    private RoomDirection direction;

    private List<Room> farresRooms = new List<Room>();
    private List<Room> lastFaRooms = new List<Room>();
    private List<Room> oneDoorRooms = new List<Room>();

    private enum RoomDirection
    {
        left,
        right,
        up,
        dowm
    }

    public LayerMask roomLayer;
    public Transform roomGenerator;


    private void Start()
    {
        for (int i = 0; i < ROOM_NUMBER; i++)
        {
            var roomPrefab = Resources.Load<GameObject>("room");
            rooms.Add(Instantiate(roomPrefab, roomGenerator.position, quaternion.identity).GetComponent<Room>());

            ChangeRoomPosition();
        }

        CheckDoorActive();
        rooms[0].GetComponent<SpriteRenderer>().color = Color.green;
        FindBossRoom();
    }

    /// <summary>
    /// 测试
    /// </summary>
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    /// <summary>
    /// 重新修改生成点位置
    /// </summary>
    private void ChangeRoomPosition()
    {
        do
        {
            direction = (RoomDirection) Random.Range(0, 4);
            switch (direction)
            {
                case RoomDirection.left:
                    roomGenerator.position += new Vector3(-xOffet, 0, 0);
                    break;
                case RoomDirection.right:
                    roomGenerator.position += new Vector3(xOffet, 0, 0);
                    break;
                case RoomDirection.up:
                    roomGenerator.position += new Vector3(0, yOffet, 0);
                    break;
                case RoomDirection.dowm:
                    roomGenerator.position += new Vector3(0, -yOffet, 0);
                    break;
            }
        } while (Physics2D.OverlapCircle(roomGenerator.position, 0.2f, roomLayer));
    }
    

    /// <summary>
    /// 检查门的active
    /// </summary>
    private void CheckDoorActive()
    {
        foreach (var room in rooms)
        {
            var position = room.transform.position;
            if (Physics2D.OverlapCircle(new Vector2(position.x + xOffet, position.y), 0.2f, roomLayer))
            {
                room.openRight = true;
            }

            if (Physics2D.OverlapCircle(new Vector2(position.x - xOffet, position.y), 0.2f, roomLayer))
            {
                room.openLeft = true;
            }

            if (Physics2D.OverlapCircle(new Vector2(position.x, position.y + yOffet), 0.2f, roomLayer))
            {
                room.openUp = true;
            }

            if (Physics2D.OverlapCircle(new Vector2(position.x, position.y - yOffet), 0.2f, roomLayer))
            {
                room.openDown = true;
            }
            room.CalculateStep();
        }
    }


    private void FindBossRoom()
    {
        var endRoom = rooms[0];
        var maxStep = GetMaxStep();
        foreach (var room in rooms)
        {
            if (room.stepFromStart == maxStep)
                farresRooms.Add(room);

            if (room.stepFromStart == maxStep - 1)
                lastFaRooms.Add(room);
        }

        foreach (var farresRoom in farresRooms)
        {
            if (farresRoom.doorRoom==1)
            {
                oneDoorRooms.Add(farresRoom);
            }
        }

        foreach (var lastFaRoom in lastFaRooms)
        {
            if (lastFaRoom.doorRoom==1)
            {
                oneDoorRooms.Add(lastFaRoom);
            }
        }

        endRoom = oneDoorRooms.Count == 0 ? farresRooms[Random.Range(0, farresRooms.Count)] : oneDoorRooms[Random.Range(0, oneDoorRooms.Count)];
        endRoom.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }

    private int GetMaxStep()
    {
        var maxStep = 0;
        foreach (var room in rooms)
        {
            if (room.stepFromStart > maxStep)
            {
                maxStep = room.stepFromStart;
            }
        }

        return maxStep;
    }
}