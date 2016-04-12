using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace RoomService.NLogic.NRoom
{
    public class List : IEnumerable
    {
        private Dictionary<UInt32, Room> _roomList = new Dictionary<uint, Room>();

        public bool Insert(Room room)
        {
            try
            {
                _roomList.Add(room.Index, room);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Room Find(UInt32 room_index)
        {
            Room findRoom = null;
            bool result = _roomList.TryGetValue(room_index, out findRoom);
            if (result == true)
            {
                return findRoom;
            }

            return null;
        }

        public bool Remove(Room room)
        {
            try
            {
                _roomList.Remove(room.Index);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public int Count
        {
            get { return _roomList.Count; }
        }

        public IEnumerator GetEnumerator()
        {
            return _roomList.GetEnumerator();
        }

    }
}
